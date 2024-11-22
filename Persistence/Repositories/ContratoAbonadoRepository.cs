using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class ContratoAbonadoRepository : BaseRepository<int, ContratoAbonado>, IContratoAbonadoRepository
    {
        public ContratoAbonadoRepository(EstacionamientoContext context, ILogger<BaseRepository<int, ContratoAbonado>> logger) : base(context, logger)
        {
        }

        public override async Task<ContratoAbonado> GetById(int id) =>
            await _context.ContratosAbonados
                .Include(ca => ca.Vehiculo)
                .FirstOrDefaultAsync(ca => ca.IdContratoAbonado == id)
            ?? throw new NotFoundException("Abono no encontrado.");

        public async Task<bool> IsPaidById(int id) =>
            await _context.ContratosAbonados.AnyAsync(ca => ca.IdContratoAbonado == id && ca.EstadoPago);

        public async Task<bool> HasAnyInProgressByIdVehiculo(int idVehiculo) =>
            await _context.ContratosAbonados.AnyAsync(ca => ca.IdVehiculo == idVehiculo && !ca.EstadoPago);

        public async Task<bool> HasClosedCajaChicaById(int id) =>
            await _context.ContratosAbonados.AnyAsync(ca => ca.IdContratoAbonado == id && ca.CajaChica != null && !ca.CajaChica.Estado);
    }
}
