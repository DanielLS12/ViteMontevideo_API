using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class TarifaRepository : BaseRepository<short, Tarifa>, ITarifaRepository
    {
        public TarifaRepository(EstacionamientoContext context, ILogger<BaseRepository<short, Tarifa>> logger) : base(context, logger)
        {
        }

        public override async Task<IEnumerable<Tarifa>> GetAll() =>
            await _context.Tarifas
                .Include(t => t.Categoria)
                .Include(t => t.Actividad)
                .AsNoTracking()
                .OrderByDescending(t => t.IdTarifa)
                .ToListAsync();

        public override async Task<Tarifa> GetById(short id) =>
            await _context.Tarifas
                .Include(t => t.Categoria)
                .Include(t => t.Actividad)
                .FirstOrDefaultAsync(t => t.IdTarifa == id) 
            ?? throw new NotFoundException("Tarifa no encontrada.");

        public async Task<bool> ExistsByCategoriaActividadAndTipo(short categoryId, short activityId, bool isHora) =>
            await _context.Tarifas.AnyAsync(t => t.IdCategoria == categoryId && t.IdActividad == activityId &&
            (isHora ? t.HoraDia != null : t.HoraDia == null));

        //public async Task<bool> ExistsByCategoriaActividadTipoAndId(short categoryId, short activityId, bool isHora, short id) =>
        //    await _context.Tarifas.AnyAsync(t => t.IdCategoria == categoryId && t.IdActividad == activityId &&
        //    (isHora ? t.HoraDia != null : t.HoraDia == null) && t.IdTarifa != id);

        public async Task<bool> ExistsById(short id) =>
            await _context.Tarifas.AnyAsync(t => t.IdTarifa == id);

        public async Task<bool> HasVehiculosById(short id) =>
            await _context.Tarifas.AnyAsync(t => t.IdTarifa == id && t.Vehiculos.Any());

        public async Task<bool> HasServiciosById(short id) =>
            await _context.Tarifas.AnyAsync(t => t.IdTarifa == id && t.Servicios.Any());
    }
}
