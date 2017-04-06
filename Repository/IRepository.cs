using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using starterpack.Models;

namespace starterpack.Repository
{
    public interface IRepository<T> where T : BaseModel {
    
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();        
        T Get(long id);
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
        void Merge(T currentEntity, T newEntity);
    }
}