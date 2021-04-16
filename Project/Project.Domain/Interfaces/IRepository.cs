using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();

        T GetById(int id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
