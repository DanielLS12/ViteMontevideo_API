using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> GetAvailableUsers();
        Task<string?> GetCargoByUsername(string username);
        Task<Usuario?> GetByUsername(string username);
        Usuario? Register(string username, string password);
        Usuario? Login(string username, string password);
        Task<bool> ExistsUsername(string username);
    }
}
