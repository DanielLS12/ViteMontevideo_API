using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Dtos.Actividades.Requests;
using AutoMapper;
using ViteMontevideo_API.Services.Dtos.Actividades.Responses;

namespace ViteMontevideo_API.Services.Implementation
{
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _repository;
        private readonly IMapper _mapper;

        public ActividadService(IActividadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DataResponse<ActividadResponseDto>> GetAll()
        {
            var actividades = await _repository.GetAll();
            int cantidad = actividades.Count();
            return new DataResponse<ActividadResponseDto>(cantidad, _mapper.Map<List<ActividadResponseDto>>(actividades));
        }

        public async Task<ActividadResponseDto> GetById(short id)
        {
            var actividad = await _repository.GetById(id) 
                ?? throw new NotFoundException("Actividad no encontrada.");

            return _mapper.Map<ActividadResponseDto>(actividad);
        }

        public async Task<ApiResponse> Insert(ActividadRequestDto actividad)
        {
            actividad = LimpiarDatos(actividad);
            bool existsActividad = await _repository.ExistsByNombre(actividad.Nombre);

            if (existsActividad)
            {
                throw new BadRequestException("Ya existe una actividad con ese nombre.");
            }

            var createdActividad = await _repository.Insert(_mapper.Map<Actividad>(actividad));
            return ApiResponse.Success("La actividad ha sido creada.", _mapper.Map<ActividadResponseDto>(createdActividad));
        }

        public async Task<ApiResponse> Update(short id, ActividadRequestDto actividad)
        {
            actividad = LimpiarDatos(actividad);
            bool existsActividad = await _repository.ExistsByIdAndNombre(id, actividad.Nombre);
            if (existsActividad)
            {
                throw new BadRequestException("Ya existe una actividad con ese nombre.");
            }

            var updatedActividad = _mapper.Map<Actividad>(actividad);

            await _repository.Update(updatedActividad);
            return ApiResponse.Success("La actividad ha sido actualizada.", _mapper.Map<ActividadResponseDto>(updatedActividad));
        }

        public async Task<ApiResponse> DeleteById(short id)
        {
            var actividad = await _repository.GetById(id)
                ?? throw new NotFoundException("Actividad no encontrada.");

            var hasTariff = await _repository.HasTarifasById(id);
            if (hasTariff)
                throw new BadRequestException("No se puede eliminar esta actividad porque esta vinculada a una o varias tarifas.");

            await _repository.Delete(actividad);
            return ApiResponse.Success("La actividad ha sido eliminada");
        }

        private static ActividadRequestDto LimpiarDatos(ActividadRequestDto actividad)
        {
            actividad.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(actividad.Nombre, @"\s+", " ").Trim());
            return actividad;
        }
    }
}
