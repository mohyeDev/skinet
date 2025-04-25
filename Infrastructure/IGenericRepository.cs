using Core.Entities;
using Core.Interfaces;

namespace Infrastructure;

public interface IGenericRepository<T> where T : BaseEntity
{

    Task<T> GetByIdAsync(int id);

    Task<IReadOnlyList<T>> ListAllAsync();

    void Add(T entity);

    void Update(T entity);

    void Remove(T Entity);

    Task<bool> SaveAllAsync();

    bool Exists(int id);

    Task<T?> GetEntityWithSpec(ISpecification<T> spec);

    Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec) ;
    
}
