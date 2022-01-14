namespace DatabaseApi.Model.Repositories;

public interface IRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> FindAsync(Guid id);
    Task CreateAsync(TEntity element);
    Task UpdateAsync(TEntity element);
    Task DeleteAsync(TEntity element);
    
    Task SaveChangesAsync();
}