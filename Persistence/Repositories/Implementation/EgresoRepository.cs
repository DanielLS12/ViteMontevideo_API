using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Models;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Persistence.Repositories.Implementation
{
    public class EgresoRepository : BaseRepository<int, Egreso>, IEgresoRepository
    {
        public EgresoRepository(EstacionamientoContext context, ILogger<BaseRepository<int, Egreso>> logger) : base(context, logger)
        {
        }
    }
}
