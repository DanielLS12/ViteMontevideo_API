using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public IQueryable<TEntity> Query() => _context.Set<TEntity>().AsNoTracking().AsQueryable();

        public IQueryable<TEntity> ApplyPageCursor(IQueryable<TEntity> query, int cursor, int count, int MaxRegisters, Expression<Func<TEntity, int>> idSelector)
        {
            var memberExpression = idSelector.Body as MemberExpression 
                ?? throw new ArgumentException("El idSelector debe ser una expresión de propiedad.", nameof(idSelector));

            string propertyName = memberExpression.Member.Name;

            if (cursor > 0)
                query = query.Where(e => EF.Property<int>(e, propertyName) < cursor);

            return query.Take(count > MaxRegisters ? MaxRegisters : count);
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
                _logger.LogWarning("No se encontro el recurso con ID: '{@id}'. {@method} - {Time}",id,nameof(GetById),DateTime.UtcNow);
                throw new NotFoundException("No se encontro el recurso.");
            }
            return entity;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurso creado en '{@method}' con la entidad '{@entity}' - {Time}", nameof(Insert), nameof(TEntity), DateTime.UtcNow);
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurso actualizado en '{@method}' con la entidad '{@entity}' - {Time}", nameof(Update), nameof(TEntity), DateTime.UtcNow);
            return entity;
        }

        public virtual async Task DeleteById(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);
            if (entity == null)
            {
                _logger.LogWarning("No se encontró el recurso con ID: '{@id}'. '{@method}' - {Time}",id,nameof(DeleteById),DateTime.UtcNow);
                throw new NotFoundException("No se encontró el recurso.");
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recurso eliminado en '{@method}' con ID '{@id}'. {Time}",nameof(DeleteById),id,DateTime.UtcNow);
        }
    }
}
