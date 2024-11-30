using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IVehiculoRepository : IBaseRepository<int,Vehiculo>
    {
        Task<int?> GetIdVehiculoByPlaca(string placa);
        Task<Vehiculo?> GetByPlaca(string placa);
        Task<bool> ExistsById(int id);
        Task<bool> ExistsByPlaca(string Placa);
        Task<bool> ExistsByIdAndPlaca(int id, string placa);
        Task<bool> HasAbonadosById(int id);
        Task<bool> HasServiciosById(int id);
    }
}
