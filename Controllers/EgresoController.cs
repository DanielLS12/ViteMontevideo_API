#pragma warning disable CS8602 // Desreferencia de una referencia posiblemente NULL.
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Dtos.Egresos;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EgresoController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public EgresoController(EstacionamientoContext dbContext, IMapper mapper) 
        { 
            _dbContext = dbContext; 
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar() 
        {
            var egresos = _dbContext.Egresos
                .AsNoTracking()
                .OrderByDescending(c => c.IdEgreso)
                .ProjectTo<EgresoResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            return Ok(egresos);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id) 
        {
            var egreso = _dbContext.Egresos
                .AsNoTracking()
                .ProjectTo<EgresoResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(e => e.IdEgreso == id) ?? throw new NotFoundException("Egreso no encontrado");

            return Ok(egreso);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(EgresoRequestDto egresoDto)
        {
            var cajaChicaAbierta = _dbContext.CajasChicas.FirstOrDefault(cc => cc.Estado == true) ?? throw new NotFoundException("No hay caja chica abierta.");

            egresoDto.IdCaja = cajaChicaAbierta.IdCaja;

            var egreso = _mapper.Map<Egreso>(egresoDto);

            _dbContext.Egresos.Add(egreso);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("El egreso ha sido agregado.");

            return CreatedAtAction(nameof(Obtener),new {id = egreso.IdEgreso}, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] EgresoRequestDto egresoDto)
        {
            var dbEgreso = _dbContext.Egresos.Find(id) ?? throw new NotFoundException("Egreso no encontrado.");

            dbEgreso.Motivo = egresoDto.Motivo;
            dbEgreso.Monto = egresoDto.Monto;
            dbEgreso.Hora = egresoDto.Hora;
            dbEgreso.Fecha = egresoDto.Fecha;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El gasto ha sido actualizado.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var dbEgreso = _dbContext.Egresos.Find(id) ?? throw new NotFoundException("Egreso no encontrado.");

            _dbContext.Egresos.Remove(dbEgreso);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("El egreso ha sido eliminado.");

            return Ok(response);
        }
    }
}
