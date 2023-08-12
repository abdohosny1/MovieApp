using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace MovieApp.Data.Base
{
    public interface IEntityBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GellAll();
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> condition = null);


        Task<IEnumerable<T>> GellAll(params Expression<Func<T, object>>[] includeProperty);

        //Task<IEnumerable<T>> GellAll(params Expression<Func<T, object>>[] includeProperty, Expression<Func<T, object>> OrderBy);

        IEnumerable<T> GetAllOrdered(Func<T, object> keySelector); // Method to retrieve all entities with ordering

        Task<T> GetById(int id);

        Task<T> GetById(int id, params Expression<Func<T, object>>[] includeProperty);

        Task<T> Add(T entity);

        Task<T> update(T entity);

        Task updateAsync(int id, T entity);

        Task<T> delete(T entity);

        Task DeleteAsync(int id);
    }
}
