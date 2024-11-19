using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class CargoRepository : BaseRepository<byte, Cargo>, ICargoRepository
    {
        public CargoRepository(EstacionamientoContext context, ILogger<BaseRepository<byte, Cargo>> logger) : base(context, logger)
        {
        }
    }
}
