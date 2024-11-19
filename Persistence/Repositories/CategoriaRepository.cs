using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;
using ViteMontevideo_API.Repositories;

namespace ViteMontevideo_API.Persistence.Repositories
{
    public class CategoriaRepository : BaseRepository<short, Categoria> , ICategoriaRepository
    {
        public CategoriaRepository(EstacionamientoContext context, ILogger<BaseRepository<short, Categoria>> logger)
        : base(context, logger) { }

        public async Task<bool> ExistsById(short id) =>
            await _context.Categorias.AnyAsync(c => c.IdCategoria == id);

        public async Task<bool> ExistsByIdAndNombre(short id, string name) =>
            await _context.Categorias.AnyAsync(c => c.Nombre == name && c.IdCategoria != id);

        public async Task<bool> ExistsByNombre(string name) =>
            await _context.Categorias.AnyAsync(c => c.Nombre == name);

        public async Task<bool> HasTarifasById(short id) =>
            await _context.Categorias.AnyAsync(c => c.IdCategoria == id && c.Tarifas.Any());
    }
}
