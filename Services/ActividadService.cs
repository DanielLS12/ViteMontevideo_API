using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.Shared.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
{
    public class ActividadService : IActividadService
    {
        private readonly IActividadRepository _repository;

        public ActividadService(IActividadRepository repository)
        {
            _repository = repository;
        }

        public async Task<DataResponse<Actividad>> GetAll()
        {
            var actividades = await _repository.GetAll();
            int cantidad = actividades.Count();
            return new DataResponse<Actividad>(cantidad,actividades);
        }

        public async Task<Actividad> GetById(short id) =>
            await _repository.GetById(id) ?? throw new NotFoundException("Actividad no encontrada.");

        public async Task<ApiResponse> Insert(Actividad actividad)
        {
            actividad = LimpiarDatos(actividad);
            bool existsActividad = await _repository.ExistsByNombre(actividad.Nombre);

            if (existsActividad)
            {
                throw new BadRequestException("Ya existe una actividad con ese nombre.");
            }

            var createdActividad = await _repository.Insert(actividad);
            return ApiResponse.Success("La actividad ha sido creada.", createdActividad);
        }

        public async Task<ApiResponse> Update(short id,Actividad actividad)
        {
            actividad = LimpiarDatos(actividad);
            bool existsActividad = await _repository.ExistsByIdAndNombre(id,actividad.Nombre);
            if(existsActividad)
            {
                throw new BadRequestException("Ya existe una actividad con ese nombre.");
            }

            var dbActividad = await _repository.GetById(id) 
                ?? throw new NotFoundException("Actividad no encontrada.");

            dbActividad.Nombre = actividad.Nombre;

            var updatedActividad = await _repository.Update(dbActividad);
            return ApiResponse.Success("La actividad ha sido actualizada.", updatedActividad);
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

        private static Actividad LimpiarDatos(Actividad actividad)
        {
            actividad.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(actividad.Nombre, @"\s+", " ").Trim());
            return actividad;
        }
    }
}
