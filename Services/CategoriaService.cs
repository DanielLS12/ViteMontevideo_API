using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.Shared.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Presentation.Dtos.Common;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<DataResponse<Categoria>> GetAll()
        {
            var categorias = await _repository.GetAll();
            int cantidad = categorias.Count();
            return new DataResponse<Categoria>(cantidad, categorias);
        }

        public async Task<Categoria> GetById(short id) => 
            await _repository.GetById(id) ?? throw new NotFoundException("Categoría no encontrada.");

        public async Task<ApiResponse> Insert(Categoria categoria)
        {
            categoria = LimpiarDatos(categoria);
            bool existsCategoria = await _repository.ExistsByNombre(categoria.Nombre);
            if(existsCategoria)
            {
                throw new BadRequestException("Ya existe una categoría con ese nombre.");
            }

            var createdCategoria = await _repository.Insert(categoria);
            return ApiResponse.Success("La categoría ha sido creada.", createdCategoria);
        }

        public async Task<ApiResponse> Update(short id, Categoria categoria)
        {
            categoria = LimpiarDatos(categoria);
            bool existsCategoria = await _repository.ExistsByIdAndNombre(id, categoria.Nombre);
            if(existsCategoria)
            {
                throw new BadRequestException("Ya existe una categoría con ese nombre.");
            }

            var dbCategoria = await _repository.GetById(id) 
                ?? throw new NotFoundException("Categoría no encontrada.");

            dbCategoria.Nombre = categoria.Nombre;

            var updatedCategoria = await _repository.Update(categoria);
            return ApiResponse.Success("La categoría ha sido actualizada.", updatedCategoria);
        }

        public async Task<ApiResponse> DeleteById(short id)
        {
            var dbCategoria = await _repository.GetById(id) 
                ?? throw new NotFoundException("Categoría no encontrada.");

            var hasTariff = await _repository.HasTarifasById(id);
            if(hasTariff)
            {
                throw new BadRequestException("No se puede eliminar esta categoría porque esta vinculada a una o varias tarifas.");
            }

            await _repository.Delete(dbCategoria);
            return ApiResponse.Success("La categoría ha sido eliminado");
        }

        private static Categoria LimpiarDatos(Categoria categoria)
        {
            categoria.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(categoria.Nombre, @"\s+", " ").Trim());
            return categoria;
        }
    }
}
