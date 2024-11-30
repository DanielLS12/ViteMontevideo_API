using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class ComercioAdicionalRepository : BaseRepository<int, ComercioAdicional>, IComercioAdicionalRepository
    {
        public ComercioAdicionalRepository(EstacionamientoContext context, ILogger<BaseRepository<int, ComercioAdicional>> logger) : base(context, logger)
        {
        }

        public override async Task<ComercioAdicional?> GetById(int id) =>
            await _context.ComerciosAdicionales
                .Include(cad => cad.Cliente)
                .FirstOrDefaultAsync(cad => cad.IdComercioAdicional == id);
    }
}
