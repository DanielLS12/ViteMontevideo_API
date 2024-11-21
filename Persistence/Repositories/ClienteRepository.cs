using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class ClienteRepository : BaseRepository<int, Cliente>, IClienteRepository
    {
        public ClienteRepository(EstacionamientoContext context, ILogger<BaseRepository<int, Cliente>> logger) : base(context, logger)
        {
        }

        public async Task<bool> ExistsById(int id) =>
            await _context.Clientes.AnyAsync(c => c.IdCliente.Equals(id));

        public async Task<bool> HasComerciosAdicionalesById(int id) =>
            await _context.Clientes.AnyAsync(c => c.IdCliente == id && c.ComerciosAdicinales.Any());

        public async Task<bool> HasVehiculosById(int id) =>
            await _context.Clientes.AnyAsync(c => c.IdCliente == id && c.Vehiculos.Any());
    }
}
