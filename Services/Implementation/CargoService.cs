using AutoMapper;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Dtos.Cargos.Requests;
using ViteMontevideo_API.Services.Dtos.Cargos.Responses;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Exceptions;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services.Implementation
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _repository;
        private readonly IMapper _mapper;

        public CargoService(ICargoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DataResponse<Cargo>> GetAll()
        {
            var cargos = await _repository.GetAll();
            int cantidad = cargos.Count();
            return new DataResponse<Cargo>(cantidad, cargos);
        }

        public async Task<ApiResponse> Insert(CargoRequestDto cargo)
        {
            bool existsCargo = await _repository.ExistsByNombre(cargo.Nombre);
            if (existsCargo)
                throw new ConflictException("Ya existe un cargo con ese nombre.");

            var createdCargo = await _repository.Insert(_mapper.Map<Cargo>(cargo));
            return ApiResponse.Success("El cargo ha sido creado.", _mapper.Map<CargoResponseDto>(createdCargo));
        }

        public async Task<ApiResponse> Update(byte id, CargoRequestDto cargo)
        {
            if (id < 3)
                throw new BadRequestException("No esta permitido actualizar el cargo admin o cajero");

            bool existsCargo = await _repository.ExistsByIdAndNombre(id, cargo.Nombre);
            if (existsCargo)
                throw new ConflictException("Ya existe un cargo con ese nombre.");

            var updatedCargo = _mapper.Map<Cargo>(cargo);

            await _repository.Update(updatedCargo);
            return ApiResponse.Success("El cargo ha sido actualizado.", _mapper.Map<CargoResponseDto>(updatedCargo));
        }

        public async Task<ApiResponse> DeleteById(byte id)
        {
            if (id < 3)
                throw new BadRequestException("No esta permitido eliminar el cargo admin o cajero");

            var cargo = await _repository.GetById(id)
                ?? throw new NotFoundException("Cargo no encontrado.");

            await _repository.Delete(cargo);
            return ApiResponse.Success("El cargo ha sido eliminado.");
        }
    }
}
