using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class CajaChicaRepository : BaseRepository<int, CajaChica>, ICajaChicaRepository
    {
        public CajaChicaRepository(EstacionamientoContext context, ILogger<BaseRepository<int, CajaChica>> logger) : base(context, logger)
        {
        }

        public async Task<CajaChica?> GetOpenCajaChica() =>
            await _context.CajasChicas.FirstOrDefaultAsync(cc => cc.Estado);

        public async Task<bool> IsCajaChicaClosedById(int id) =>
            await _context.CajasChicas.AnyAsync(cc => cc.IdCaja == id && !cc.Estado);
    }
}
