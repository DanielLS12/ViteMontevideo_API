using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IComercioAdicionalRepository : IBaseRepository<int, ComercioAdicional>
    {
        Task<bool> HasClosedCajaChicaById(int id);
    }
}
