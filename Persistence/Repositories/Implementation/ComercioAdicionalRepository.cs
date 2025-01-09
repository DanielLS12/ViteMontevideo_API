using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class ComercioAdicionalRepository : BaseRepository<int, ComercioAdicional>, IComercioAdicionalRepository
    {
        public ComercioAdicionalRepository(EstacionamientoContext context, ILogger<BaseRepository<int, ComercioAdicional>> logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<ComercioAdicional>> GetAll(int idCajaChica) =>
            await _context.ComerciosAdicionales
                .AsNoTracking()
                .Include(cad => cad.Cliente)
                .Where(cad => cad.IdCaja == idCajaChica)
                .ToListAsync();

        public override async Task<ComercioAdicional?> GetById(int id) =>
            await _context.ComerciosAdicionales
                .Include(cad => cad.Cliente)
                .FirstOrDefaultAsync(cad => cad.IdComercioAdicional == id);
    }
}
