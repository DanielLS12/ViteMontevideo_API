using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface ICajaChicaRepository : IBaseRepository<int, CajaChica>
    {
        Task<CajaChica?> GetByEstadoTrue();
    }
}
