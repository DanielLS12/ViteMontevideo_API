using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Services.Dtos.Common;
using ViteMontevideo_API.Services.Interfaces;

namespace ViteMontevideo_API.Services.Implementation
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _repository;

        public CargoService(ICargoRepository repository)
        {
            _repository = repository;
        }

        public async Task<DataResponse<Cargo>> GetAll()
        {
            var cargos = await _repository.GetAll();
            int cantidad = cargos.Count();
            return new DataResponse<Cargo>(cantidad, cargos);
        }
    }
}
