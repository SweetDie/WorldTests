using System;
using System.Collections.Generic;

namespace WorldTests.DAL.Interfaces
{
    public interface IDataGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Func<T, bool> predicate);
        T Get(Guid id);
        T Get(Func<T, bool> predicate);
        void Create(T newEntity);
        void Remove(T entity);
        void Edit(T entity);
    }
}
