using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ILogger<UsuarioRepository> _logger;
        private readonly EstacionamientoContext _context;

        public UsuarioRepository(ILogger<UsuarioRepository> logger, EstacionamientoContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Usuario>> GetAvailableUsers() =>
            await _context.Usuarios
                .AsNoTracking()
                .Where(u => u.Trabajadores.Count == 0 && u.Estado)
                .Select(u => new Usuario()
                {
                    IdUsuario = u.IdUsuario,
                    Nombre = u.Nombre,
                })
                .ToListAsync();

        public async Task<Usuario?> GetByUsername(string username) =>
            await _context.Usuarios
            .AsNoTracking()
            .Select(u => new Usuario()
            {
                IdUsuario = u.IdUsuario,
                Nombre = u.Nombre,
                Estado = u.Estado,
            })
            .FirstOrDefaultAsync(u => u.Nombre == username);

        public async Task<bool> ExistsUsername(string username) =>
            await _context.Usuarios.AnyAsync(u => u.Nombre.Contains(username));

        public async Task<string?> GetCargoByUsername(string username) =>
            await _context.Usuarios
                .Where(u => u.Nombre == username)
                .Join(
                    _context.Trabajadores,
                    u => u.IdUsuario,
                    t => t.IdUsuario,
                    (u, t) => t
                )
                .Join(
                    _context.Cargos,
                    t => t.IdCargo,
                    c => c.IdCargo,
                    (t, c) => c.Nombre
                )
                .FirstOrDefaultAsync();
                

        public Usuario? Login(string username, string password)
        {
            var usuario = _context.Usuarios
                .FromSqlRaw("EXEC dbo.Acceder @Nombre={0}, @Clave={1}", username, password)
                .AsEnumerable()
                .Select(u => new Usuario(){ IdUsuario = u.IdUsuario, Nombre = u.Nombre, Estado = u.Estado })
                .FirstOrDefault();

            if (usuario is not null)
                _logger.LogInformation("Sesión iniciada con {usuario} | {Time}", usuario.Nombre, DateTime.UtcNow);
            else
                _logger.LogError("Credenciales incorrectos con {usuario} | {Time}", username, DateTime.UtcNow);

            return usuario;
        }

        public Usuario? Register(string username, string password)
        {
            var usuario = _context.Usuarios
                .FromSqlRaw("EXEC dbo.Registrar @Nombre={0}, @Clave={1}", username, password)
                .AsEnumerable()
                .Select(u => new Usuario { IdUsuario = u.IdUsuario, Nombre = u.Nombre, Estado = u.Estado })
                .FirstOrDefault();

            if (usuario is not null)
                _logger.LogInformation("Usuario creado con {usuario} | {Time}", usuario.Nombre, DateTime.UtcNow);
            else
                _logger.LogError("Error inesperado al crear usuario | {Time}",DateTime.UtcNow);

            return usuario;
        }
    }
}
