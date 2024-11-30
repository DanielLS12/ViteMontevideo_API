using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IServicioRepository : IBaseRepository<int, Servicio>
    {
        Task<Servicio?> GetServicioEntrada(int idServicio);
        Task<Servicio?> GetServicioEntrada(string placa);
        Task<Servicio?> GetServicioSalida(int idServicio);
        Task<bool> HasAnyServicioInProgressByIdVehiculo(int idVehiculo);
    }
}
