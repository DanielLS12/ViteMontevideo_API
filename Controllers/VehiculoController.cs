#pragma warning disable CS8602
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Dtos.Cursor;
using ViteMontevideo_API.Dtos.Vehiculos;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class VehiculoController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public VehiculoController(EstacionamientoContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar([FromQuery] string? placa, [FromQuery] CursorParams pararms) 
        {
            var count = _dbContext.Vehiculos.Count();
            const int maxCountRows = 30;

            var vehiculosQuery = _dbContext.Vehiculos
                .AsNoTracking()
                .OrderByDescending(v => v.IdVehiculo)
                .ProjectTo<VehiculoResponseDto>(_mapper.ConfigurationProvider);

            if(!string.IsNullOrWhiteSpace(placa) && placa.Length >= 3)
            {
                vehiculosQuery = vehiculosQuery.Where(v => v.Placa.Contains(placa.ToUpper()));
                count = vehiculosQuery.Count();
            }

            if (pararms.Cursor > 0)
                vehiculosQuery = vehiculosQuery.Where(v => v.IdVehiculo < pararms.Cursor);

            var vehiculos = vehiculosQuery.Take(pararms.Count > maxCountRows ? maxCountRows : pararms.Count).ToList();

            var nextCursor = vehiculos.Any() ? vehiculos.LastOrDefault()?.IdVehiculo : 0;

            Response.Headers.Add("X-Pagination", $"Next Cursor={nextCursor}");

            return Ok(new {cantidad = count, siguiente = nextCursor, data = vehiculos});
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            var vehiculo = _dbContext.Vehiculos
                .AsNoTracking()
                .ProjectTo<VehiculoResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(v => v.IdVehiculo == id) ?? throw new NotFoundException("Vehículo no encontrado.");

            return Ok(vehiculo);
        }

        [HttpGet("{placa}")]
        public IActionResult ObtenerPorPlaca(string placa)
        {
            var vehiculoPorPlaca = _dbContext.Vehiculos
                .AsNoTracking()
                .ProjectTo<VehiculoResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(v => v.Placa == placa.ToUpper()) ?? throw new NotFoundException("Vehículo no encontrado.");

            return Ok(vehiculoPorPlaca);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(VehiculoRequestDto vehiculoDto)
        {
            if(vehiculoDto.IdCliente != null)
            {
                bool existeCliente = _dbContext.Clientes.Any(c => c.IdCliente == vehiculoDto.IdCliente);
                if (!existeCliente)
                    throw new NotFoundException("El cliente que intento vincular al vehículo no existe.");
            }

            var existePlaca = _dbContext.Vehiculos.Any(v => v.Placa == vehiculoDto.Placa);
            if (existePlaca)
                throw new ConflictException("La placa ya existe en otro vehículo.");

            var vehiculo = _mapper.Map<Vehiculo>(vehiculoDto);

            _dbContext.Vehiculos.Add(vehiculo);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El vehículo ha sido agregado.");

            return CreatedAtAction(nameof(ObtenerPorId), new {id = vehiculo.IdVehiculo} ,response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] VehiculoRequestDto vehiculoDto)
        {
            if (vehiculoDto.IdCliente != null)
            {
                bool existeCliente = _dbContext.Clientes.Any(c => c.IdCliente == vehiculoDto.IdCliente);
                if (!existeCliente)
                    throw new NotFoundException("El cliente que intento vincular al vehículo no existe.");
            }

            var dbVehiculo = _dbContext.Vehiculos.Find(id) ?? throw new NotFoundException("Vehículo no encontrado.");

            var existePlaca = _dbContext.Vehiculos.Any(v => v.Placa == vehiculoDto.Placa && v.IdVehiculo != id);
            if (existePlaca)
                throw new ConflictException("La placa ya existe en otro vehículo.");

            dbVehiculo.IdTarifa = vehiculoDto.IdTarifa;
            dbVehiculo.IdCliente = vehiculoDto.IdCliente;
            dbVehiculo.Placa = vehiculoDto.Placa.ToUpper();

            _dbContext.SaveChanges();

            return Ok("Vehiculo actualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var dbVehiculo = _dbContext.Vehiculos.Find(id) ?? throw new NotFoundException("Vehículo no encontrado.");

            var tieneAbonadosVinculados = _dbContext.ContratosAbonados.Any(ca => ca.IdVehiculo == id);

            var tieneServiciosVinculados = _dbContext.Servicios.Any(s => s.IdVehiculo == id);

            if (tieneAbonadosVinculados || tieneServiciosVinculados)
                throw new BadRequestException("No se puede eliminar este vehículo porque ya ha realizado algún servicio.");

            _dbContext.Vehiculos.Remove(dbVehiculo);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El vehículo ha sido eliminado.");

            return Ok(response);
        }
    }
}
