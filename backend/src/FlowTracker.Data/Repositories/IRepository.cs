using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Repositories
{
    public interface IRepository<T>
    {
        void Add(T entity);
        T? Get(int id);
        List<T> GetAll();
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        bool Exits(int id);
        int Save();
    }
}
