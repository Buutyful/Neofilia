namespace Neofilia.Server.Data.Repository;

public interface IRepository<TEntity> where TEntity : class //in the future create an entity interface 
{
    public Task<List<TEntity>> Get();
    public Task<TEntity> GetById(int id);
    public Task Add(TEntity entity);
    public Task Update(int id, TEntity entity);
    public Task Delete(int id);
}
