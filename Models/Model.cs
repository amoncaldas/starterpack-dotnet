using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StarterPack.Models
{
    public abstract class Model<T> where T :  Model<T>
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        protected readonly DbContext context;
        protected DbSet<T> entities;
        
        public Model()
        {
            context = getContext();
            entities = context.Set<T>();
        }

        public Model(DbContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        protected static DbContext getContext() {
            // TODO: encapsulate the Startup.GetServiceLocator in another class
            return (DbContext) Startup.GetServiceLocator.Instance.GetService(GetContextType());
        }

        protected static Type GetContextType() {
            return typeof(DatabaseContext);
        }

        protected static DbSet<T> getEntities(DbContext context = null) {
            var _context = context == null? getContext() : context;
            return _context.Set<T>();
        }        


        public static IEnumerable<T> GetAll(bool tracked = false) {
            if(tracked) {
                return getEntities().AsEnumerable();
            }
            return getEntities().AsNoTracking().AsEnumerable();
        }

        public static IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, bool tracked) {           
            if(tracked) {
                return getEntities().AsNoTracking().Where(predicate);
            } 
            return getEntities().AsNoTracking().Where(predicate);
        } 

        public static T Get(long id)
        {            
            return getEntities().SingleOrDefault(s => s.Id == id);
        }   

        public void Save(bool saveChanges = true) {
            Save((T)this, saveChanges);            
        }

        public static void Save(T entity, bool saveChanges = true) {
            var context = getContext();            
            getEntities(context).Add(entity);

            if(saveChanges) {
                context.SaveChanges();
            }
            
        }
        
        public static void Add(T entity) {
            var context = getContext();            
            getEntities(context).Add(entity);
        }

        public static void Delete(long id, bool saveChanges = true) {    
            var context = getContext();           
            getEntities(context).Remove(Model<T>.Get(id));

            if(saveChanges) {
                context.SaveChanges();
            }
        }

        public void Delete(bool saveChanges = true) {
            entities.Remove((T)this);
            
            if(saveChanges) {
                context.SaveChanges();
            }
        }

        public static T UpdateAttributes(long id, T modelWithAttr) {
            //var context = getContext();           
            //T model = getEntities(context).AsNoTracking().SingleOrDefault(s => s.Id == id);
            //context.Entry(modelWithAttr).Context.Update(model);
            //context.SaveChanges();
            return modelWithAttr;            
        } 

        public virtual void Update(bool saveChanges = true) {
            var context = getContext();            
            getEntities(context).Update((T)this);

            if(saveChanges) {
                context.SaveChanges();
            }
        }     
    }
}
