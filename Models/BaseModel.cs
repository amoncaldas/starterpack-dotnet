using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Starterpack.Models
{
    public class BaseModel<T> where T :  BaseModel<T>
    {
        public Int64 Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        private readonly DatabaseContext context;
        private DbSet<T> entities;
        
        public BaseModel()
        {
            context = getContext();
            entities = context.Set<T>();
        }        

        public static BaseModel<T> createInstance() {
            return new BaseModel<T>();
        }

        private static DatabaseContext getContext() {
            return (DatabaseContext) Startup.GetMeSomeServiceLocator.Instance.GetService(typeof(DatabaseContext));
        }

        private static DbSet<T> getEntity() {
            return getContext().Set<T>();
        }        

        public static T Get(long id)
        {
            return getEntity().SingleOrDefault(s => s.Id == id);
        } 

        public static long Get1(long id)
        {
            return id;
        } 

        public virtual IEnumerable<T> GetAll() {
            return entities.AsEnumerable();
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {

            IQueryable<T> query = entities.Where(predicate);
            return query;
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
