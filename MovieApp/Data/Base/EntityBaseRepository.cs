using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MovieApp.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public EntityBaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T> delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Set<T>().FindAsync(id);

            EntityEntry entityEntry = _context.Entry<T>(data);
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GellAll()
        {
            var data = await _context.Set<T>().ToListAsync();
            return data;
        }

        public async Task<IEnumerable<T>> GellAll(params Expression<Func<T, object>>[] includeProperty)
        {
            IQueryable<T> quary = _context.Set<T>();
            quary = includeProperty.Aggregate(quary, (current, includeProperty)
                                   => current.Include(includeProperty));

            return await quary.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> condition = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            var entities = await query.ToListAsync();
            return entities;
        }

        public IEnumerable<T> GetAllOrdered(Func<T, object> keySelector)
        {
            var orderedData = _context.Set<T>().OrderBy(keySelector).ToList();
            return orderedData;
        }

        public async Task<T> GetById(int id)
        {
            var data = await _context.Set<T>().FindAsync(id);
            return data;
        }

        public async Task<T> GetById(int id, params Expression<Func<T, object>>[] includeProperty)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var property in includeProperty)
            {
                query = query.Include(property);
            }

            var entity = await query.ToListAsync();


            var entityWithId = entity.SingleOrDefault(e => GetId(e) == id);

            return entityWithId;
        }

        private int GetId(T entity)
        {
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null)
            {
                return (int)idProperty.GetValue(entity);
            }
            throw new InvalidOperationException("Entity does not have an Id property.");
        }

        public async Task<T> update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task updateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        IEnumerable<T> IEntityBaseRepository<T>.GetAllOrdered(Func<T, object> keySelector)
        {
            var orderedData = _context.Set<T>().OrderBy(keySelector).ToList();
            return orderedData;
        }
    }
}
