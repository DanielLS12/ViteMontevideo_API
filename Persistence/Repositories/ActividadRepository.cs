using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class ActividadRepository : BaseRepository<short, Actividad>, IActividadRepository
    {
        public ActividadRepository(EstacionamientoContext context, ILogger<BaseRepository<short, Actividad>> logger)
        : base(context, logger) { }

        public async Task<bool> ExistsByNombre(string name) =>
            await _context.Actividades.AnyAsync(a => a.Nombre == name);
        

        public async Task<bool> ExistsByIdAndNombre(short id,string name) =>
            await _context.Actividades.AnyAsync(a => a.Nombre == name && id != a.IdActividad);
      

        public async Task<bool> HasTarifasById(short id) =>
            await _context.Actividades
                .Where(a => a.IdActividad.Equals(id))
                .AnyAsync(a => a.Tarifas.Any());

        public async Task<bool> ExistsById(short id) =>
            await _context.Actividades.AnyAsync(a => a.IdActividad == id);
    }
}
