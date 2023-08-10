using System.Linq.Expressions;

namespace DnDAPI.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Create(T entity);
        Task Remove(T entity);
        Task SaveAsync();
    }
}
