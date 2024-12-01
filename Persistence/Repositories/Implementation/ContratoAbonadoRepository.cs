using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class ContratoAbonadoRepository : BaseRepository<int, ContratoAbonado>, IContratoAbonadoRepository
    {
        public ContratoAbonadoRepository(EstacionamientoContext context, ILogger<BaseRepository<int, ContratoAbonado>> logger) : base(context, logger)
        {
        }

        public override async Task<ContratoAbonado?> GetById(int id) =>
            await _context.ContratosAbonados
                .Include(ca => ca.Vehiculo)
                .FirstOrDefaultAsync(ca => ca.IdContratoAbonado == id);

        public async Task<bool> HasAnyAbonoInProgressByIdVehiculo(int idVehiculo) =>
            await _context.ContratosAbonados.AnyAsync(ca => ca.IdVehiculo == idVehiculo && !ca.EstadoPago);
    }
}
