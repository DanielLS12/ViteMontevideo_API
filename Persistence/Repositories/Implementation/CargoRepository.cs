using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class CargoRepository : BaseRepository<byte, Cargo>, ICargoRepository
    {
        public CargoRepository(EstacionamientoContext context, ILogger<BaseRepository<byte, Cargo>> logger) : base(context, logger)
        {
        }

        public async Task<bool> ExistsById(byte id) =>
            await _context.Cargos.AnyAsync(c => c.IdCargo == id);
    }
}
