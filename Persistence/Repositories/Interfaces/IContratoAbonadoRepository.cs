using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IContratoAbonadoRepository : IBaseRepository<int, ContratoAbonado>
    {
        Task<bool> HasClosedCajaChicaById(int id);
        Task<bool> IsPaidById(int id);
        Task<bool> HasAnyInProgressByIdVehiculo(int idVehiculo);
    }
}
