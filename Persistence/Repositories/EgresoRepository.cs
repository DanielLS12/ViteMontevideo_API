using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class EgresoRepository : BaseRepository<int, Egreso>, IEgresoRepository
    {
        public EgresoRepository(EstacionamientoContext context, ILogger<BaseRepository<int, Egreso>> logger) : base(context, logger)
        {
        }

        public async Task<bool> HasClosedCajaChicaById(int id) =>
            await _context.Egresos.AnyAsync(e => e.IdEgreso == id && !e.CajaChica.Estado);
    }
}
