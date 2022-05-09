using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WorldTests.DAL.Interfaces;

namespace WorldTests.DAL.Repository
{
    public class GenericRepository<T> : IDataGenericRepository<T> where T : class
    {
        private DbContext _dbContext;
        private DbSet<T> _set;

        public GenericRepository(DbContext context)
        {
            _dbContext = context;
            _set = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _set.AsNoTracking().ToList();
        }

        public IEnumerable<T> GetAll(Func<T, bool> predicate)
        {
            return _set.AsNoTracking().Where(predicate);
        }

        public T Get(Guid id)
        {
            return _set.Find(id);
        }

        public T Get(Func<T, bool> predicate)
        {
            return _set.AsNoTracking().FirstOrDefault(predicate);
        }

        public void Create(T newEntity)
        {
            _set.Add(newEntity);
            _dbContext.SaveChanges();
        }

        public void Remove(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Deleted;
            _dbContext.SaveChanges();
        }

        public void Edit(T entity)
        {
            try
            {
                //_dbContext.Update(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
    }
}
