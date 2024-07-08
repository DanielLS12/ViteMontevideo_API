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
        public IActionResult Listar([FromQuery] string placa, [FromQuery] CursorParams parametros) 
        {
            const int MaxRegistros = 50;
            var query = _dbContext.Vehiculos.AsQueryable();
            
            // Filtraje
            if (!string.IsNullOrWhiteSpace(placa) && placa.Length >= 3)
                query = query.Where(v => v.Placa.Contains(placa.ToUpper()));

            int cantidad = query.Count();

            // Cursor
            if (parametros.Cursor > 0)
                query = query.Where(v => v.IdVehiculo < parametros.Cursor);

            query = query.Take(parametros.Count > MaxRegistros ? MaxRegistros : parametros.Count);

            // Listar
            var data = query
                .AsNoTracking()
                .OrderByDescending(v => v.IdVehiculo)
                .ProjectTo<VehiculoResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            var siguiente = data.Any() ? data.LastOrDefault()?.IdVehiculo : 0;

            if (siguiente == 0)
                cantidad = 0;

            Response.Headers.Add("X-Pagination", $"Next Cursor={siguiente}");

            return Ok(new { cantidad, siguiente, data });
        }

        [HttpGet("{id:int}")]
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
            var vehiculo = _dbContext.Vehiculos
                .AsNoTracking()
                .ProjectTo<VehiculoResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(v => v.Placa == placa) ?? throw new NotFoundException("Vehículo no encontrado.");

            return Ok(vehiculo);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(VehiculoCrearRequestDto vehiculoDto)
        {
            var tarifaExiste = _dbContext.Tarifas.Any(t => t.IdTarifa == vehiculoDto.IdTarifa);
            if (!tarifaExiste)
                throw new NotFoundException("La tarifa que intento vincular al vehículo no existe.");

            if(vehiculoDto.IdCliente != null)
            {
                bool existeCliente = _dbContext.Clientes.Any(c => c.IdCliente == vehiculoDto.IdCliente);
                if (!existeCliente)
                    throw new NotFoundException("El cliente que intento vincular al vehículo no existe.");
            }

            var existePlaca = _dbContext.Vehiculos.Any(v => v.Placa == vehiculoDto.Placa);
            if (existePlaca)
                throw new ConflictException("La placa ya existe en otro vehículo.");

            // Las placas siempre deben estar en mayúsculas.
            vehiculoDto.Placa = vehiculoDto.Placa.ToUpper();

            var vehiculo = _mapper.Map<Vehiculo>(vehiculoDto);

            _dbContext.Vehiculos.Add(vehiculo);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El vehículo ha sido agregado.");

            return CreatedAtAction(nameof(ObtenerPorId), new {id = vehiculo.IdVehiculo} ,response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar(int id,[FromBody] VehiculoActualizarRequestDto vehiculoDto)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            var dbVehiculo = _dbContext.Vehiculos.Find(id) ?? throw new NotFoundException("Vehículo no encontrado.");

            var existePlaca = _dbContext.Vehiculos.Any(v => v.Placa == vehiculoDto.Placa && v.IdVehiculo != id);
            if (existePlaca)
                throw new ConflictException("La placa ya existe en otro vehículo.");

            if (vehiculoDto.IdTarifa != null)
            {
                var tarifaExiste = _dbContext.Tarifas.Any(t => t.IdTarifa == vehiculoDto.IdTarifa);
                if (!tarifaExiste)
                    throw new NotFoundException("La tarifa que intento vincular al vehículo no existe.");
                dbVehiculo.IdTarifa = (short)vehiculoDto.IdTarifa;
            }

            if (vehiculoDto.IdCliente != null)
            {
                bool existeCliente = _dbContext.Clientes.Any(c => c.IdCliente == vehiculoDto.IdCliente);
                if (!existeCliente)
                    throw new NotFoundException("El cliente que intento vincular al vehículo no existe.");
                dbVehiculo.IdCliente = vehiculoDto.IdCliente;
            }

            dbVehiculo.Placa = vehiculoDto.Placa.ToUpper();

            _dbContext.SaveChanges();

            transaction.Commit();

            var response = ApiResponse.Success("El vehículo ha sido actualizado.");

            return Ok(response);
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
