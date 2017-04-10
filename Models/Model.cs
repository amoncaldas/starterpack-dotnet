using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StarterPack.Models
{
    public abstract class Model<T> where T :  Model<T>
    {
        public Int64 Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected readonly DbContext _context;
        protected DbSet<T> _entities;
        
        public Model()
        {
            _context = getContext();
            _entities = _context.Set<T>();
        }

        public Model(DbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public static Model<T> createInstance() {
            return (T) Activator.CreateInstance(typeof(T));
        }

        protected static DbContext getContext() {
            return (DbContext) Startup.GetServiceLocator.Instance.GetService(GetContextType());
        }

        protected static Type GetContextType() {
            return typeof(DatabaseContext);
        }

        protected static DbSet<T> getEntities(DbContext context = null) {
            var _context = context == null? getContext() : context;
            return _context.Set<T>();
        }        


        public static IEnumerable<T> GetAll() {
            return getEntities().AsEnumerable();
        }

        public static IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate) {
            IQueryable<T> query = getEntities().Where(predicate);
            return query;
        } 

        public static T Get(long id)
        {            
            return getEntities().SingleOrDefault(s => s.Id == id);
        }   

        public void Save() {
            Save((T)this);            
        }

        public static void Save(T entity) {
            var context = getContext();            
            getEntities(context).Add(entity);
            context.SaveChanges();
        }

        public static void Delete(long id) {    
            var context = getContext();           
            getEntities(context).Remove(Model<T>.Get(id));
            context.SaveChanges();
        }

        public void Delete() {
            _entities.Remove((T)this);
            _context.SaveChanges();
        }

        public static T UpdateAttributes(long id, T modelWithAttr) {
            //var context = getContext();           
            //T model = getEntities(context).AsNoTracking().SingleOrDefault(s => s.Id == id);
            //context.Entry(modelWithAttr).Context.Update(model);
            //context.SaveChanges();
            return modelWithAttr;
            
        } 

        public virtual void Update() {
            _context.SaveChanges();
        }     
    }
}
