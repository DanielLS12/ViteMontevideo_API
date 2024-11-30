using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class ServicioRepository : BaseRepository<int, Servicio>, IServicioRepository
    {
        public ServicioRepository(EstacionamientoContext context, ILogger<BaseRepository<int, Servicio>> logger) : base(context, logger)
        {
        }

        public async Task<Servicio?> GetServicioSalida(int idServicio) =>
            await _context.Servicios
                .Include(s => s.Vehiculo)
                .Include(s => s.Tarifa!)
                    .ThenInclude(t => t.Categoria)
                .Include(s => s.Tarifa!)
                    .ThenInclude(t => t.Actividad)
                .FirstOrDefaultAsync(s => s.IdServicio == idServicio && s.EstadoPago);

        public async Task<Servicio?> GetServicioEntrada(int idServicio)
        {
            return await _context.Servicios
                .Include(s => s.Vehiculo)
                    .ThenInclude(v => v.Tarifa)
                        .ThenInclude(t => t.Categoria)
                .Include(s => s.Vehiculo)
                    .ThenInclude(v => v.Tarifa)
                        .ThenInclude(t => t.Actividad)
                .FirstOrDefaultAsync(s => s.IdServicio == idServicio && !s.EstadoPago);
        }

        public async Task<Servicio?> GetServicioEntrada(string placa)
        {
            return await _context.Servicios
                .Include(s => s.Vehiculo)
                    .ThenInclude(v => v.Tarifa)
                        .ThenInclude(t => t.Categoria)
                .Include(s => s.Vehiculo)
                    .ThenInclude(v => v.Tarifa)
                        .ThenInclude(t => t.Actividad)
                .FirstOrDefaultAsync(s => s.Vehiculo.Placa.Contains(placa) && !s.EstadoPago);
        }

        public async Task<bool> HasAnyServicioInProgressByIdVehiculo(int idVehiculo) =>
            await _context.Servicios.AnyAsync(ca => ca.IdVehiculo == idVehiculo && !ca.EstadoPago);
    }
}
