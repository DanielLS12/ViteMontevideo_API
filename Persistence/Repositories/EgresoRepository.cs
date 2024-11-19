using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using ViteMontevideo_API.Exceptions;
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

        public async Task Delete(Egreso egreso)
        {
            _context.Egresos.Remove(egreso);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Egreso eliminado en {nameof(Delete)} con ID '{egreso.IdEgreso}'");
        }

        public IQueryable<Egreso> ApplyPageCursor(IQueryable<Egreso> query, int cursor, int count, int MaxRegisters)
        {
            if (cursor > 0)
                query = query.Where(v => v.IdEgreso < cursor);

            return query.Take(count > MaxRegisters ? MaxRegisters : count);
        }

        public IQueryable<Egreso> Query() => _context.Egresos.AsNoTracking().AsQueryable();
    }
}
