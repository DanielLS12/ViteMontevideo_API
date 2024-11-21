using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IClienteRepository : IBaseRepository<int, Cliente>
    {
        Task<bool> ExistsById(int id);
        Task<bool> HasVehiculosById(int id);
        Task<bool> HasComerciosAdicionalesById(int id);
    }
}
