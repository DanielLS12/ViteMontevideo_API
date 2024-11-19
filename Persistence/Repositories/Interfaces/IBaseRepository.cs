namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IBaseRepository<TId, TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> GetById(TId id);
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task DeleteById(TId id);
    }
}
