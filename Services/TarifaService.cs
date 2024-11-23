using AutoMapper;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Presentation.Dtos.Tarifas;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
{
    public class TarifaService : ITarifaService
    {
        private readonly ITarifaRepository _tarifaRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IActividadRepository _actividadRepository;
        private readonly IMapper _mapper;

        public TarifaService(ITarifaRepository tarifaRepository, ICategoriaRepository categoriaRepository, IActividadRepository actividadRepository, IMapper mapper)
        {
            _tarifaRepository = tarifaRepository;
            _categoriaRepository = categoriaRepository;
            _actividadRepository = actividadRepository;
            _mapper = mapper;
        }

        public async Task<DataResponse<TarifaResponseDto>> GetAll()
        {
            var tarifas = await _tarifaRepository.GetAll();
            int cantidad = tarifas.Count();
            var data = _mapper.Map<List<TarifaResponseDto>>(tarifas);
            return new DataResponse<TarifaResponseDto>(cantidad, data);
        }

        public async Task<TarifaResponseDto> GetById(short id)
        {
            var tarifa = await _tarifaRepository.GetById(id);
            return _mapper.Map<TarifaResponseDto>(tarifa);

        }

        public async Task<ApiResponse> Insert(TarifaCrearRequestDto tarifa)
        {
            if (tarifa.HoraDia.HasValue != tarifa.HoraNoche.HasValue)
                throw new BadRequestException("Si se proporciona un valor para 'Hora Día', también debe proporcionarse un valor para 'Hora Noche'. Ambos campos deben ser nulos o tener valores.");

            if (tarifa.HoraDia.HasValue && tarifa.HoraNoche.HasValue && tarifa.HoraDia >= tarifa.HoraNoche)
                throw new BadRequestException("La hora día no puede ser mayor o igual a la hora noche.");

            if (!tarifa.HoraDia.HasValue && !tarifa.HoraNoche.HasValue && tarifa.PrecioDia != tarifa.PrecioNoche)
                throw new BadRequestException("Si no existen horas día y noche es porque tiene la modalidad de 'Hora', por lo tanto, sus precios deben ser iguales.");

            bool existsCategoria = await _categoriaRepository.ExistsById(tarifa.IdCategoria);
            if (!existsCategoria)
                throw new BadRequestException("No es posible registrar la tarifa porque no existe la categoría que se desea vincular.");

            bool existsActividad = await _actividadRepository.ExistsById(tarifa.IdActividad);
            if (!existsActividad)
                throw new BadRequestException("No es posible registrar la tarifa porque no existe la actividad que se desea vincular.");

            bool existsTarifa = await _tarifaRepository.ExistsByCategoriaActividadAndTipo(tarifa.IdCategoria, tarifa.IdActividad, tarifa.HoraDia.HasValue);
            if (existsTarifa)
                throw new BadRequestException("Ya existe una tarifa con tales características.");

            var dbTarifa = _mapper.Map<Tarifa>(tarifa);
            dbTarifa = await _tarifaRepository.Insert(dbTarifa);
            dbTarifa = await _tarifaRepository.GetById(dbTarifa.IdTarifa);
            var createdTarifa = _mapper.Map<TarifaResponseDto>(dbTarifa);
            return ApiResponse.Success("La tarifa ha sido creada.", createdTarifa);
        }

        public async Task<ApiResponse> Update(short id, TarifaActualizarRequestDto tarifa)
        {
            if (tarifa.HoraDia.HasValue != tarifa.HoraNoche.HasValue)
                throw new BadRequestException("Si se proporciona un valor para 'Hora Día', también debe proporcionarse un valor para 'Hora Noche'. Ambos campos deben ser nulos o tener valores.");

            if (tarifa.HoraDia.HasValue && tarifa.HoraNoche.HasValue && tarifa.HoraDia >= tarifa.HoraNoche)
                throw new BadRequestException("La hora día no puede ser mayor o igual a la hora noche.");

            if (!tarifa.HoraDia.HasValue && !tarifa.HoraNoche.HasValue && tarifa.PrecioDia != tarifa.PrecioNoche)
                throw new BadRequestException("Si no existen horas día y noche es porque tiene la modalidad de 'Hora', por lo tanto, sus precios deben ser iguales.");

            var dbTarifa = await _tarifaRepository.GetById(id);

            if (dbTarifa.HoraDia.HasValue != tarifa.HoraDia.HasValue || dbTarifa.HoraNoche.HasValue != tarifa.HoraNoche.HasValue)
                throw new BadRequestException($"No se puede cambiar la modalidad de la tarifa. La tarifa actual es '{(dbTarifa.HoraDia.HasValue ? "Turno" : "Hora")}' y se intenta cambiar a '{(tarifa.HoraDia.HasValue ? "Turno" : "Hora")}'.");

            dbTarifa.PrecioDia = tarifa.PrecioDia;
            dbTarifa.PrecioNoche = tarifa.PrecioNoche;
            dbTarifa.Tolerancia = tarifa.Tolerancia;

            await _tarifaRepository.Update(dbTarifa);
            dbTarifa = await _tarifaRepository.GetById(id);
            var updatedTarifa = _mapper.Map<TarifaResponseDto>(dbTarifa);
            return ApiResponse.Success("La tarifa ha sido actualizada.", updatedTarifa);
        }

        public async Task<ApiResponse> DeleteById(short id)
        {
            var dbTarifa = await _tarifaRepository.GetById(id);

            bool hasVehicles = await _tarifaRepository.HasVehiculosById(id);
            bool hasServices = await _tarifaRepository.HasServiciosById(id);

            if (hasVehicles || hasServices)
                throw new BadRequestException("No se puede eliminar esta tarifa porque esta vinculado a algunos vehículo(s) y/o servicio(s).");

            await _tarifaRepository.Delete(dbTarifa);
            return ApiResponse.Success("La tarifa ha sido eliminada.");
        }
    }
}
