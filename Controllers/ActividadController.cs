using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.ActionFilters;
using ViteMontevideo_API.Dtos.Common;
using ViteMontevideo_API.Middleware.Exceptions;
using ViteMontevideo_API.models;

namespace ViteMontevideo_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ActividadController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;

        public ActividadController(EstacionamientoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var data = _dbContext.Actividades
                .AsNoTracking()
                .ToList();
            var cantidad = data.Count;

            return Ok(new DataResponse<Actividad>(cantidad,data));
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(short id)
        {
            var actividad = _dbContext.Actividades
                .AsNoTracking()
                .SingleOrDefault(a => a.IdActividad == id) ?? throw new NotFoundException("Actividad no encontrada.");

            return Ok(actividad);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Guardar(Actividad actividad)
        {
            actividad = LimpiarDatos(actividad);

            var nombreRepetido = _dbContext.Actividades.Any(c => c.Nombre == actividad.Nombre);

            if (nombreRepetido)
                throw new BadRequestException("Ya existe una actividad con ese nombre.");

            _dbContext.Actividades.Add(actividad);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("La actividad ha sido creada.");

            return CreatedAtAction(nameof(Obtener), new { id = actividad.IdActividad }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] short id,[FromBody] Actividad actividad)
        {
            var dbActividad = _dbContext.Actividades.Find(id) ?? throw new NotFoundException("Actividad no encontrada.");

            actividad = LimpiarDatos(actividad);

            var categoriaRepetida = _dbContext.Categorias.SingleOrDefault(c => c.Nombre == actividad.Nombre);

            if (categoriaRepetida != null && dbActividad.IdActividad != categoriaRepetida.IdCategoria)
                throw new BadRequestException("Ya existe un actividad con ese nombre.");

            dbActividad.Nombre = actividad.Nombre;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La actividad ha sido actualizada.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(short id)
        {
            var dbActividad = _dbContext.Actividades.Find(id) ?? throw new NotFoundException("Actividad no encontrada.");

            var tieneTarifasVinculadas = _dbContext.Tarifas.Any(t => t.IdActividad == id);

            if (tieneTarifasVinculadas)
                throw new BadRequestException("No se puede eliminar esta actividad porque esta vinculada a una o varias tarifas.");

            _dbContext.Actividades.Remove(dbActividad);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La actividad ha sido eliminada.");

            return Ok(response);
        }

        private static Actividad LimpiarDatos(Actividad actividad)
        {
            actividad.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(actividad.Nombre, @"\s+", " ").Trim());
            return actividad;
        }
    }
}
