using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ViteMontevideo_API.Persistence.Context;
using ViteMontevideo_API.Persistence.Repositories.Interfaces;

namespace ViteMontevideo_API.Repositories
{
    public abstract class BaseRepository<TId, TEntity> : IBaseRepository<TId, TEntity> where TEntity : class, new()
    {
        protected readonly ILogger<BaseRepository<TId, TEntity>> _logger;
        protected readonly EstacionamientoContext _context;

        private const int MaxRegistros = 200;

        public BaseRepository(EstacionamientoContext context, ILogger<BaseRepository<TId, TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IQueryable<TEntity> Query() => _context.Set<TEntity>().AsNoTracking().AsQueryable();

        public IQueryable<TEntity> ApplyPageCursor(IQueryable<TEntity> query, int cursor, int count, Expression<Func<TEntity, int>> idSelector)
        {
            var memberExpression = idSelector.Body as MemberExpression 
                ?? throw new ArgumentException("El idSelector debe ser una expresión de propiedad.", nameof(idSelector));

            string propertyName = memberExpression.Member.Name;

            if (cursor > 0)
                query = query.Where(e => EF.Property<int>(e, propertyName) < cursor);

            return query.Take(count > MaxRegistros ? MaxRegistros : count);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll() =>
            await _context.Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

        public virtual async Task<TEntity?> GetById(TId id)
        {
            var entity = await _context.FindAsync<TEntity>(id);

            if(entity == null)
                _logger.LogWarning("No se encontro el recurso con ID: '{@id}'. {@method} - {Time}", id, nameof(GetById), DateTime.UtcNow);
            
            return entity;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            string entityType = entity.GetType().Name;

            _logger.LogInformation("Recurso creado en '{@method}' con la entidad '{@entity}' - {Time}", nameof(Insert), entityType, DateTime.UtcNow);
            return entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();

            string entityType = entity.GetType().Name;

            _logger.LogInformation("Recurso actualizado en '{@method}' con la entidad '{@entity}' - {Time}", nameof(Update), entityType, DateTime.UtcNow);
            return entity;
        }

        public virtual async Task Delete(TEntity entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();

            string entityType = entity.GetType().Name;

            _logger.LogInformation("Recurso eliminado en '{@method}' con la entidad '{@entity}'. {Time}",nameof(Delete), entityType, DateTime.UtcNow);
        }
    }
}
