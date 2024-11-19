using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Trabajadores;

namespace ViteMontevideo_API.Presentation.Controllers
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
            var data = _dbContext.Trabajadores
                .AsNoTracking()
                .OrderByDescending(c => c.IdTrabajador)
                .ProjectTo<TrabajadorResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            int cantidad = data.Count;

            return Ok(new DataResponse<TrabajadorResponseDto>(cantidad, data));
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(short id)
        {
            var trabajador = _dbContext.Trabajadores
                .AsNoTracking()
                .ProjectTo<TrabajadorResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(u => u.IdTrabajador == id) ?? throw new NotFoundException("Trabajador no encontrado.");

            return Ok(trabajador);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(TrabajadorCrearRequestDto trabajadorDto)
        {
            var cargo = _dbContext.Cargos.Find(trabajadorDto.IdCargo) ?? throw new NotFoundException("Cargo no encontrado.");

            var dniDuplicado = _dbContext.Trabajadores.Any(t => t.Dni == trabajadorDto.Dni);

            if (dniDuplicado)
                throw new ConflictException("Este dni ya se encuentra en otro trabajador.");

            trabajadorDto = LimpiarDatos(trabajadorDto);

            if (trabajadorDto.Correo != null)
            {
                var correoDuplicado = _dbContext.Trabajadores.Any(t => t.Correo == trabajadorDto.Correo);
                if (correoDuplicado)
                    throw new ConflictException("Este correo ya se encuentra en otro trabajador");
            }

            if (trabajadorDto.Telefono != null)
            {
                var telefonoDuplicado = _dbContext.Trabajadores.Any(t => t.Telefono == trabajadorDto.Telefono);
                if (telefonoDuplicado)
                    throw new ConflictException("Este número de teléfono ya se encuentra en otro trabajador");
            }

            trabajadorDto.IdCargo = cargo.IdCargo;
            trabajadorDto.Nombre = trabajadorDto.Nombre;
            trabajadorDto.ApellidoPaterno = trabajadorDto.ApellidoPaterno;
            trabajadorDto.ApellidoMaterno = trabajadorDto.ApellidoMaterno;

            var trabajador = _mapper.Map<Trabajador>(trabajadorDto);

            trabajador.Estado = true;

            _dbContext.Trabajadores.Add(trabajador);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El trabajador ha sido agregado.");

            return CreatedAtAction(nameof(Obtener), new { id = trabajador.IdTrabajador }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] short id, [FromBody] TrabajadorActualizarRequestDto trabajadorDto)
        {
            var dbTrabajador = _dbContext.Trabajadores.Find(id) ?? throw new NotFoundException("Trabajador no encontrado.");

            if (trabajadorDto.IdCargo != null)
            {
                var dbCargo = _dbContext.Cargos.Find(trabajadorDto.IdCargo) ?? throw new NotFoundException("Cargo no encontrado.");
                dbTrabajador.IdCargo = dbCargo.IdCargo;
            }

            if (trabajadorDto.Dni != null)
            {
                var dniDuplicado = _dbContext.Trabajadores.Any(t => t.Dni == trabajadorDto.Dni && t.IdTrabajador != id);

                if (dniDuplicado)
                    throw new ConflictException("Este dni ya se encuentra en otro trabajador.");
            }

            trabajadorDto = LimpiarDatos(trabajadorDto);

            if (trabajadorDto.Correo != null)
            {
                var correoDuplicado = _dbContext.Trabajadores.Any(t => t.Correo == trabajadorDto.Correo && t.IdTrabajador != id);
                if (correoDuplicado)
                    throw new ConflictException("Este correo ya se encuentra en otro trabajador");
            }

            if (trabajadorDto.Telefono != null)
            {
                var telefonoDuplicado = _dbContext.Trabajadores.Any(t => t.Telefono == trabajadorDto.Telefono && t.IdTrabajador != id);
                if (telefonoDuplicado)
                    throw new ConflictException("Este número de teléfono ya se encuentra en otro trabajador");
            }

            dbTrabajador.Nombre = trabajadorDto.Nombre ?? dbTrabajador.Nombre;
            dbTrabajador.ApellidoPaterno = trabajadorDto.ApellidoPaterno ?? dbTrabajador.ApellidoPaterno;
            dbTrabajador.ApellidoMaterno = trabajadorDto.ApellidoMaterno ?? dbTrabajador.ApellidoMaterno;
            dbTrabajador.Correo = string.IsNullOrWhiteSpace(trabajadorDto.Correo) ? null : trabajadorDto.Correo;
            dbTrabajador.Telefono = string.IsNullOrWhiteSpace(trabajadorDto.Telefono) ? null : trabajadorDto.Telefono;
            dbTrabajador.Dni = trabajadorDto.Dni ?? dbTrabajador.Dni;
            dbTrabajador.Estado = trabajadorDto.Estado ?? dbTrabajador.Estado;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El trabajador ha sido actualizado.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(short id)
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

        private static TrabajadorCrearRequestDto LimpiarDatos(TrabajadorCrearRequestDto trabajador)
        {
            trabajador.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.Nombre, @"\s+", " ").Trim());
            trabajador.ApellidoPaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoPaterno, @"\s+", " ").Trim());
            trabajador.ApellidoMaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoMaterno, @"\s+", " ").Trim());

            if (trabajador.Telefono != null)
                trabajador.Telefono = Regex.Replace(trabajador.Telefono, @"\s+", " ");

            if (trabajador.Correo != null)
                trabajador.Correo = Regex.Replace(trabajador.Correo, @"\s+", "").Trim();

            return trabajador;
        }

        private static TrabajadorActualizarRequestDto LimpiarDatos(TrabajadorActualizarRequestDto trabajador)
        {
            if (trabajador.Nombre != null)
                trabajador.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.Nombre, @"\s+", " ").Trim());

            if (trabajador.ApellidoPaterno != null)
                trabajador.ApellidoPaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoPaterno, @"\s+", " ").Trim());

            if (trabajador.ApellidoMaterno != null)
                trabajador.ApellidoMaterno = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(trabajador.ApellidoMaterno, @"\s+", " ").Trim());

            if (trabajador.Telefono != null)
                trabajador.Telefono = Regex.Replace(trabajador.Telefono, @"\s+", " ");

            if (trabajador.Correo != null)
                trabajador.Correo = Regex.Replace(trabajador.Correo, @"\s+", "").Trim();

            return trabajador;
        }
    }
}
