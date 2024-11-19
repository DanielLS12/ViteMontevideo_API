namespace ViteMontevideo_API.Persistence.Repositories.Interfaces
{
    public interface IPageCursorRepository<TEntity> where TEntity : class 
    {
        IQueryable<TEntity> Query();
        IQueryable<TEntity> ApplyPageCursor(IQueryable<TEntity> query, int cursor, int count, int MaxRegisters);

    }
}
