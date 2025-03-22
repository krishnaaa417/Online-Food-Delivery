using ePizza.Domain.Models;
using ePizza.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ePizza.Repository.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected EpizzaHubDbContext _dbContext;

        public GenericRepository(EpizzaHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);    
           
        }

        public int CommitChanges()
        {
           return _dbContext.SaveChanges();
        }

        public void Delete(object id)
        {
            T entity = _dbContext.Set<T>().Find(id);
            if (entity != null)
            {
                _dbContext.Set<T>().Remove(entity);
            }
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = _dbContext.Set<T>();

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public T GetById(object id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public async Task<T> GetSingleItem(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
    }
}
