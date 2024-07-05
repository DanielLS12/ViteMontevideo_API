using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Dtos.Trabajadores;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TrabajadorController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public TrabajadorController(EstacionamientoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar() 
        {
            var trabajadores = _dbContext.Trabajadores
                .AsNoTracking()
                .OrderByDescending(c => c.IdTrabajador)
                .ProjectTo<TrabajadorResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            return Ok(trabajadores);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id_trabajador)
        {
            var trabajador = _dbContext.Trabajadores
                .AsNoTracking()
                .ProjectTo<TrabajadorResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(u => u.IdTrabajador == id_trabajador);

            return Ok(trabajador);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(TrabajadorRequestDto trabajadorDto)
        {
            var cargo = _dbContext.Cargos.Find(trabajadorDto.IdCargo) ?? throw new NotFoundException("Cargo no encontrado.");

            trabajadorDto.IdCargo = cargo.IdCargo;
            trabajadorDto.Nombre = trabajadorDto.Nombre.Trim();
            trabajadorDto.ApellidoPaterno = trabajadorDto.ApellidoPaterno.Trim();
            trabajadorDto.ApellidoMaterno = trabajadorDto.ApellidoMaterno.Trim();

            var trabajador = _mapper.Map<Trabajador>(trabajadorDto);

            _dbContext.Trabajadores.Add(trabajador);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El trabajador ha sido agregado.");

            return CreatedAtAction(nameof(Obtener), new { id = trabajador.IdTrabajador }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] TrabajadorRequestDto trabajadorDto)
        {
            var dbTrabajador = _dbContext.Trabajadores.Find(id) ?? throw new NotFoundException("Trabajador no encontrado.");

            var dbCargo = _dbContext.Cargos.Find(trabajadorDto.IdCargo) ?? throw new NotFoundException("Cargo no encontrado.");

            dbTrabajador.IdCargo = dbCargo.IdCargo;
            dbTrabajador.Nombre = trabajadorDto.Nombre.Trim();
            dbTrabajador.ApellidoPaterno = trabajadorDto.ApellidoPaterno.Trim();
            dbTrabajador.ApellidoMaterno = trabajadorDto.ApellidoMaterno.Trim();
            dbTrabajador.Correo = string.IsNullOrWhiteSpace(trabajadorDto.Correo) ? null : trabajadorDto.Correo.Trim();
            dbTrabajador.Telefono = string.IsNullOrWhiteSpace(trabajadorDto.Telefono) ? null : trabajadorDto.Telefono.Trim();
            dbTrabajador.Dni = trabajadorDto.Dni;
            dbTrabajador.Estado = trabajadorDto.Estado;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El trabajador ha sido actualizado.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var trabajador = _dbContext.Trabajadores.Find(id) ?? throw new NotFoundException("Trabajador no encontrado.");

            var tieneCajasChicasVinculadas = _dbContext.CajasChicas.Any(cc => cc.IdTrabajador == id);

            if (tieneCajasChicasVinculadas)
                throw new BadRequestException("No se puede eliminar este trabajador debido a su presencia en algunas cajas chicas.");

            _dbContext.Trabajadores.Remove(trabajador);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El trabajador ha sido eliminado.");

            return Ok(response);
        }
    }
}
