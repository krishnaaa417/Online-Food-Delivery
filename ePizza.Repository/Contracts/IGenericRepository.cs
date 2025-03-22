using System.Linq.Expressions;

namespace ePizza.Repository.Contracts
{
    public interface IGenericRepository<T> where  T : class
    {

        void Add(T entity);

        IEnumerable<T> GetAll();

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

        Task<T> GetSingleItem(Expression<Func<T, bool>> filter = null);

        void Update(T entity);
        
        void Remove(T entity);

        void Delete(object id);

        T GetById(object id);

        int CommitChanges();
    }
}
