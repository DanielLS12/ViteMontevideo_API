using System.Globalization;
using System.Text.RegularExpressions;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using AutoMapper;
using ViteMontevideo_API.Services.Dtos.Categorias.Requests;
using ViteMontevideo_API.Services.Dtos.Categorias.Responses;

namespace ViteMontevideo_API.Services.Implementation
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DataResponse<CategoriaResponseDto>> GetAll()
        {
            var categorias = await _repository.GetAll();
            int cantidad = categorias.Count();
            return new DataResponse<CategoriaResponseDto>(cantidad, _mapper.Map<List<CategoriaResponseDto>>(categorias));
        }

        public async Task<CategoriaResponseDto> GetById(short id)
        {
            var categoria = await _repository.GetById(id)
                ?? throw new NotFoundException("Categoría no encontrada.");

            return _mapper.Map<CategoriaResponseDto>(categoria);
        }

        public async Task<ApiResponse> Insert(CategoriaRequestDto categoria)
        {
            categoria = LimpiarDatos(categoria);
            bool existsCategoria = await _repository.ExistsByNombre(categoria.Nombre);
            if (existsCategoria)
            {
                throw new BadRequestException("Ya existe una categoría con ese nombre.");
            }

            var createdCategoria = await _repository.Insert(_mapper.Map<Categoria>(categoria));
            return ApiResponse.Success("La categoría ha sido creada.", _mapper.Map<CategoriaResponseDto>(createdCategoria));
        }

        public async Task<ApiResponse> Update(short id, CategoriaRequestDto categoria)
        {
            categoria = LimpiarDatos(categoria);
            bool existsCategoria = await _repository.ExistsByIdAndNombre(id, categoria.Nombre);
            if (existsCategoria)
            {
                throw new BadRequestException("Ya existe una categoría con ese nombre.");
            }

            var updatedCategoria = _mapper.Map<Categoria>(categoria);

            await _repository.Update(updatedCategoria);
            return ApiResponse.Success("La categoría ha sido actualizada.", _mapper.Map<CategoriaResponseDto>(updatedCategoria));
        }

        public async Task<ApiResponse> DeleteById(short id)
        {
            var dbCategoria = await _repository.GetById(id)
                ?? throw new NotFoundException("Categoría no encontrada.");

            var hasTariff = await _repository.HasTarifasById(id);
            if (hasTariff)
            {
                throw new BadRequestException("No se puede eliminar esta categoría porque esta vinculada a una o varias tarifas.");
            }

            await _repository.Delete(dbCategoria);
            return ApiResponse.Success("La categoría ha sido eliminado");
        }

        private static CategoriaRequestDto LimpiarDatos(CategoriaRequestDto categoria)
        {
            categoria.Nombre = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(categoria.Nombre, @"\s+", " ").Trim());
            return categoria;
        }
    }
}
