#pragma warning disable CS8602
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Dtos.Tarifas;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TarifaController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;
        private readonly IMapper _mapper;

        public TarifaController(EstacionamientoContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var tarifas = _dbContext.Tarifas
                .AsNoTracking()
                .OrderByDescending(c => c.IdTarifa)
                .ProjectTo<TarifaResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            return Ok(tarifas);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var tarifa = _dbContext.Tarifas
                .AsNoTracking()
                .ProjectTo<TarifaResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(t => t.IdTarifa == id) ?? throw new NotFoundException("Tarifa no encontrada.");

            return Ok(tarifa);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(TarifaRequestDto tarifaDto)
        {
            if (TarifaYaExiste(tarifaDto))
                throw new BadRequestException("La tarifa ya existe.");

            var tarifa = _mapper.Map<Tarifa>(tarifaDto);

            _dbContext.Tarifas.Add(tarifa);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("La tarifa ha sido creada.");

            return CreatedAtAction(nameof(Obtener),new {id = tarifa.IdTarifa},response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] TarifaRequestDto tarifaDto)
        {
            var dbTarifa = _dbContext.Tarifas.Find(id) ?? throw new NotFoundException("Tarifa no encontrada.");

            if (TarifaYaExiste(tarifaDto, id))
                throw new BadRequestException("Esta tarifa ya existe.");

            dbTarifa.IdCategoria = tarifaDto.IdCategoria;
            dbTarifa.IdActividad = tarifaDto.IdActividad;
            dbTarifa.PrecioDia = tarifaDto.PrecioDia;
            dbTarifa.PrecioNoche = tarifaDto.PrecioNoche;
            dbTarifa.HoraDia = tarifaDto.HoraDia;
            dbTarifa.HoraNoche = tarifaDto.HoraNoche;
            dbTarifa.Tolerancia = tarifaDto.Tolerancia;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La tarifa ha sido actualizada.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(short id)
        {
            var dbTarifa = _dbContext.Tarifas.Find(id) ?? throw new NotFoundException("Tarifa no encontrada.");

            var tieneVehiculosVinculados = _dbContext.Vehiculos.Any(v => v.IdTarifa == id);

            var tieneServiciosVinculados = _dbContext.Servicios.Any(s => s.IdTarifa == id);

            if (tieneVehiculosVinculados || tieneServiciosVinculados)
                throw new BadRequestException("No se puede eliminar la tarifa debido a la presencia de vehículos y/o servicios vinculados a esta.");

            _dbContext.Tarifas.Remove(dbTarifa);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La tarifa ha sido eliminada.");

            return Ok(response);
        }

        private bool TarifaYaExiste(TarifaRequestDto tarifa, int? tarifaId = null)
        {
            var existeTarifa = _dbContext.Tarifas.FirstOrDefault(t =>
                            t.IdTarifa != tarifaId &&
                            t.IdCategoria == tarifa.IdCategoria &&
                            t.IdActividad == tarifa.IdActividad &&
                            (
                                (t.HoraDia == null && tarifa.HoraDia == null) ||
                                (t.HoraDia != null && tarifa.HoraDia != null)
                            ));

            return existeTarifa != null;
        }
    }
}
