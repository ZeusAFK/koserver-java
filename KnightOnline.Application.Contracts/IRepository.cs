using KnightOnline.Domain.SharedKernel; // For Entity
using System.Linq.Expressions;

namespace KnightOnline.Application.Contracts.Infrastructure
{
    public interface IRepository<T, TId> where T : Entity<TId> where TId : notnull
    {
        Task<T?> GetByIdAsync(TId id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        // Potentially other common methods like FindAsync(Expression<Func<T, bool>> predicate)
    }
}
