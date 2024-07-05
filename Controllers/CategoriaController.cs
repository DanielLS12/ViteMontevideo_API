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
    public class CategoriaController : ControllerBase
    {
        private readonly EstacionamientoContext _dbContext;

        public CategoriaController(EstacionamientoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var data = _dbContext.Categorias
                .AsNoTracking()
                .ToList();
            var cantidad = data.Count;
            return Ok(new DataResponse<Categoria>(cantidad, data));
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
        public IActionResult Guardar(Categoria categoria)
        {
            categoria = LimpiarDatos(categoria);

            var nombreRepetido = _dbContext.Categorias.Any(c => c.Nombre ==  categoria.Nombre);

            if (nombreRepetido)
                throw new BadRequestException("Ya existe una categoria con ese nombre.");

            _dbContext.Categorias.Add(categoria);
            _dbContext.SaveChanges();

            var response = ApiResponse.SuccessCreated("La categoria ha sido creada.");

            return CreatedAtAction(nameof(Obtener), new { id = categoria.IdCategoria }, response);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public IActionResult Editar([FromRoute] int id,[FromBody] Categoria categoria)
        {
            var dbCategoria = _dbContext.Categorias.Find(id) ?? throw new NotFoundException("Categoria no encontrada.");

            categoria = LimpiarDatos(categoria);

            var categoriaRepetida = _dbContext.Categorias.SingleOrDefault(c => c.Nombre == categoria.Nombre);

            if (categoriaRepetida != null && dbCategoria.IdCategoria != categoriaRepetida.IdCategoria)
                throw new BadRequestException("Ya existe un categoria con ese nombre.");

            dbCategoria.Nombre = categoria.Nombre;

            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La categoría ha sido actualizada.");

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public IActionResult Eliminar(int id)
        {
            var dbCategoria = _dbContext.Categorias
                .Include(c => c.Tarifas)
                .SingleOrDefault(c => c.IdCategoria == id) ?? throw new NotFoundException("Categoria no encontrada.");

            if (dbCategoria.Tarifas.Any())
                throw new BadRequestException("No se puede eliminar esta categoría porque esta vinculada a una o varias tarifas.");

            _dbContext.Categorias.Remove(dbCategoria);
            _dbContext.SaveChanges();

            var response = ApiResponse.Success("La categoría ha sido eliminada.");

            return Ok(response);
        }

        private static Categoria LimpiarDatos(Categoria categoria)
        {
            categoria.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(categoria.Nombre, @"\s+", " ").Trim());
            return categoria;
        }
    }
}
