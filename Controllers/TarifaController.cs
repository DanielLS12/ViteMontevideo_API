#pragma warning disable CS8602
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
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
            var data = _dbContext.Tarifas
                .AsNoTracking()
                .OrderByDescending(c => c.IdTarifa)
                .ProjectTo<TarifaResponseDto>(_mapper.ConfigurationProvider)
                .ToList();

            int cantidad = data.Count;

            return Ok(new DataResponse<TarifaResponseDto>(cantidad,data));
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(short id)
        {
            var tarifa = _dbContext.Tarifas
                .AsNoTracking()
                .ProjectTo<TarifaResponseDto>(_mapper.ConfigurationProvider)
                .FirstOrDefault(t => t.IdTarifa == id) ?? throw new NotFoundException("Tarifa no encontrada.");

            return Ok(tarifa);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(TarifaCrearRequestDto tarifaDto)
        {
            var categoria = _dbContext.Categorias.Find(tarifaDto.IdCategoria) ?? throw new NotFoundException("Categoria no encontrada.");

            var actividad = _dbContext.Actividades.Find(tarifaDto.IdActividad) ?? throw new NotFoundException("Actividad no encontrada.");

            if ((tarifaDto.HoraDia.HasValue && !tarifaDto.HoraNoche.HasValue) || (!tarifaDto.HoraDia.HasValue && tarifaDto.HoraNoche.HasValue))
                throw new BadRequestException("Si hora día tiene valor, entonces hora noche también debe tenerlo, y viceversa.");

            if(tarifaDto.HoraDia.HasValue && tarifaDto.HoraNoche.HasValue)
            {
                if (tarifaDto.HoraDia >= tarifaDto.HoraNoche)
                    throw new BadRequestException("La hora día no puede ser mayor o igual a la hora noche.");
            }

            if(!tarifaDto.HoraDia.HasValue && !tarifaDto.HoraNoche.HasValue)
            {
                if (tarifaDto.PrecioDia != tarifaDto.PrecioNoche)
                    throw new BadRequestException("Si no existen horas día y noche es porque tiene la modalidad de 'Hora', por lo tanto, sus precios deben ser iguales.");
            }

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
        public IActionResult Editar([FromRoute] short id, [FromBody] TarifaActualizarRequestDto tarifaDto)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            var dbTarifa = _dbContext.Tarifas.Find(id) ?? throw new NotFoundException("Tarifa no encontrada.");

            if ((tarifaDto.HoraDia.HasValue && !tarifaDto.HoraNoche.HasValue) || (!tarifaDto.HoraDia.HasValue && tarifaDto.HoraNoche.HasValue))
                throw new BadRequestException("Si hora día tiene valor, entonces hora noche también debe tenerlo, y viceversa.");

            if (tarifaDto.HoraDia.HasValue && tarifaDto.HoraNoche.HasValue)
            {
                if (tarifaDto.HoraDia >= tarifaDto.HoraNoche)
                    throw new BadRequestException("La hora día no puede ser mayor o igual a la hora noche.");
            }

            if (!tarifaDto.HoraDia.HasValue && !tarifaDto.HoraNoche.HasValue)
            {
                if (tarifaDto.PrecioDia != tarifaDto.PrecioNoche)
                    throw new BadRequestException("Si no existen horas día y noche es porque tiene la modalidad de 'Hora', por lo tanto, sus precios deben ser iguales.");
            }

            dbTarifa.PrecioDia = tarifaDto.PrecioDia ?? dbTarifa.PrecioDia;
            dbTarifa.PrecioNoche = tarifaDto.PrecioNoche ?? dbTarifa.PrecioNoche;
            dbTarifa.HoraDia = tarifaDto.HoraDia;
            dbTarifa.HoraNoche = tarifaDto.HoraNoche;
            dbTarifa.Tolerancia = tarifaDto.Tolerancia ?? dbTarifa.Tolerancia;

            var existeTarifa = TarifaYaExiste(dbTarifa, dbTarifa.IdTarifa);

            if (existeTarifa)
            {
                transaction.Rollback();
                throw new BadRequestException("La tarifa ya existe");
            }

            _dbContext.SaveChanges();

            transaction.Commit();

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

        private bool TarifaYaExiste(TarifaCrearRequestDto tarifa)
        {
            var existeTarifa = _dbContext.Tarifas.Any(t =>
                            t.IdCategoria == tarifa.IdCategoria &&
                            t.IdActividad == tarifa.IdActividad &&
                            (
                                (t.HoraDia == null && tarifa.HoraDia == null) ||
                                (t.HoraDia != null && tarifa.HoraDia != null)
                            ));

            return existeTarifa;
        }

        private bool TarifaYaExiste(Tarifa tarifa, int id)
        {
            var existeTarifa = _dbContext.Tarifas.Any(t =>
                t.IdTarifa != id &&
                t.IdCategoria == tarifa.IdCategoria &&
                t.IdActividad == tarifa.IdActividad &&
                (
                    (t.HoraDia == null && tarifa.HoraDia == null) ||
                    (t.HoraDia != null && tarifa.HoraDia != null)
                ));

            return existeTarifa;
        }
    }
}
