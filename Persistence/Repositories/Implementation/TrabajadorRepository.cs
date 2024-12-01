using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class TrabajadorRepository : BaseRepository<short, Trabajador>, ITrabajadorRepository
    {
        public TrabajadorRepository(EstacionamientoContext context, ILogger<BaseRepository<short, Trabajador>> logger) : base(context, logger)
        {
        }

        public async Task<bool> Exists(short id = 0, string? dni = null, string? telefono = null, string? correo = null)
        {
            if (string.IsNullOrWhiteSpace(dni) && string.IsNullOrWhiteSpace(telefono) && string.IsNullOrWhiteSpace(correo))
                return false;

            var query = _context.Trabajadores.AsQueryable();

            if (id != 0)
                query = query.Where(t => t.IdTrabajador != id);

            if (!string.IsNullOrWhiteSpace(dni))
                query = query.Where(t => t.Dni == dni);

            if (!string.IsNullOrWhiteSpace(telefono))
                query = query.Where(t => t.Telefono == telefono);

            if (!string.IsNullOrWhiteSpace(correo))
                query = query.Where(t => t.Correo == correo);

            return await query.AnyAsync();
        }

        public override async Task<IEnumerable<Trabajador>> GetAll() =>
            await _context.Trabajadores
                .Include(t => t.Cargo)
                .AsNoTracking()
                .ToListAsync();

        public override async Task<Trabajador?> GetById(short id) =>
            await _context.Trabajadores
                .Include(t => t.Cargo)
                .FirstOrDefaultAsync(t => t.IdTrabajador == id);

        public async Task<bool> HasCajasChicasById(short id) =>
            await _context.Trabajadores.AnyAsync(t => t.IdTrabajador == id && t.CajasChicas.Any());
    }
}
