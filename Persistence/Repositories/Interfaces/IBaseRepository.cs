using System.Linq.Expressions;

namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IBaseRepository<TId, TEntity> where TEntity : class
    {
        IQueryable<TEntity> Query();
        IQueryable<TEntity> ApplyPageCursor(IQueryable<TEntity> query, int cursor, int count, Expression<Func<TEntity, int>> idSelector);
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity?> GetById(TId id);
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(TEntity entity);
    }
}
