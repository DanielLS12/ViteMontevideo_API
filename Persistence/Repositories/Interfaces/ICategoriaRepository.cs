using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface ICategoriaRepository : IBaseRepository<short,Categoria>
    {
        Task<bool> ExistsById(short id);
        Task<bool> ExistsByNombre(string name);
        Task<bool> ExistsByIdAndNombre(short id, string name);
        Task<bool> HasTarifasById(short id);
    }
}
