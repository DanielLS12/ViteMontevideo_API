using ViteMontevideo_API.Persistence.Models;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface ICargoRepository : IBaseRepository<byte,Cargo>
    {
        Task<bool> ExistsById(byte id);
    }
}
