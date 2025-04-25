using Core.Entities;

namespace Infrastructure;

public interface IGenericRepository<T> where T : BaseEntity
{

    Task<T> GetbyIdAsync(int id);

    Task<IReadOnlyList<T>> ListAllAsync();

    void Add(T entity);

    void Update(T entity);

    void Remove(T Entity);

    Task<bool> SaveAllAsync();

    bool Exits(int id);


}
