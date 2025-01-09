using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Services.Dtos.CajasChicas.Responses;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface ICajaChicaRepository : IBaseRepository<int, CajaChica>
    {
        Task<List<InformeCajaChica>> GetAllInformes(DateTime fecha);
        Task<CajaChica?> GetOpenCajaChica();
        Task<bool> ExistsOpenCajaChica();
        Task<bool> IsCajaChicaClosedById(int id);
        Task<bool> HasContratosAbonadosOrEgresosOrServicios(int id);
    }
}
