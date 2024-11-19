using Microsoft.EntityFrameworkCore;
using ViteMontevideo_API.Exceptions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Repositories
{
    public abstract class BaseRepository<TId, TEntity> : IBaseRepository<TId, TEntity> where TEntity : class, new()
    {
        protected readonly ILogger<BaseRepository<TId, TEntity>> _logger;
        protected readonly EstacionamientoContext _context;

        public BaseRepository(EstacionamientoContext context, ILogger<BaseRepository<TId, TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll() =>
            await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

        public virtual async Task<TEntity> GetById(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            if(entity == null)
            {
                _logger.LogWarning($"No se encontro el recurso con ID: '{id}'. {nameof(GetById)}");
                throw new NotFoundException("No se encontro el recurso.");
            }
            return entity;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Recurso creado en {nameof(Insert)}");
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Recurso actualizado en {nameof(Update)}");
            return entity;
        }

        public virtual async Task DeleteById(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            if (entity == null)
            {
                _logger.LogWarning($"No se encontró el recurso con ID: '{id}'. {nameof(DeleteById)}");
                throw new NotFoundException("No se encontró el recurso.");
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Recurso eliminado en {nameof(DeleteById)} con ID '{id}'");
        }
    }
}
