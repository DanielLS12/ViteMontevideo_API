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

        public async Task<bool> HasAnyInProgressByIdVehiculo(int idVehiculo) =>
            await _context.Servicios.AnyAsync(ca => ca.IdVehiculo == idVehiculo && !ca.EstadoPago);
    }
}
