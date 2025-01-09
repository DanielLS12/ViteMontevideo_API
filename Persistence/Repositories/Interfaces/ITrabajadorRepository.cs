using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface ITrabajadorRepository : IBaseRepository<short, Trabajador>
    {
        Task<bool> ExistsById(short id);
        Task<bool> Exists(short id = 0, string? dni = null, string? telefono = null, string? correo = null);
        Task<bool> HasCajasChicasById(short id);
    }
}
