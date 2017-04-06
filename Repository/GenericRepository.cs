using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using starterpack.Models;

namespace starterpack.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : BaseModel
    {

        private readonly DatabaseContext context;
        private DbSet<T> entities;

        public GenericRepository(DatabaseContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll() {
            return entities.AsEnumerable();
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {

            IQueryable<T> query = entities.Where(predicate);
            return query;
        }

        public virtual T Get(long id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }    

        public virtual void Insert(T entity) {
            entities.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(T entity) {
            entities.Remove(entity);
            context.SaveChanges();
        }

        public virtual void Update(T entity) {
            context.SaveChanges();
        }

        public virtual void Merge(T currentEntity, T newEntity) {
            context.Entry(currentEntity).Context.Update(newEntity);
            context.SaveChanges();
        }        

    }
}