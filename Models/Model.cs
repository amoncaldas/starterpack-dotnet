using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Dynamic;
using StarterPack.Core;
using Microsoft.AspNetCore.Mvc;

namespace StarterPack.Models
{
    public abstract class Model<T> where T :  Model<T>
    {
        public long Id { get; set; }
        
        [FromQuery(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [FromQuery(Name = "updated_at")]
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

        private static DbContext getContext() {
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

        public static IQueryable<T> BuildQuery(Expression<Func<T, bool>> predicate) {
            return Query().Where(predicate);          
        } 

        public static IQueryable<T> Query() { 
           return getEntities();
        } 

        public static IQueryable<T> PaginatedQuery(int page, int perPage) {           
            return Query().Take(perPage).Skip((page-1) * perPage);
        } 

        public static IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool tracked = false) {  
            if(tracked) {
                return BuildQuery(predicate).AsEnumerable();
            }         
            
            return BuildQuery(predicate).AsNoTracking().AsEnumerable();
        } 

        public static T Get(long id)
        {            
            return getEntities().SingleOrDefault(s => s.Id == id);
        }   

        public void Save(bool applyChanges = true) {
            Save((T)this, applyChanges);            
        }

        public static void Save(T entity, bool applyChanges = true) {
            var context = getContext();            
            getEntities(context).Add(entity);

            if(applyChanges) {
                context.SaveChanges();
            }            
        }
        
        public static void Add(T entity) {
            var context = getContext();            
            getEntities(context).Add(entity);
        }

        public static void Delete(long id, bool applyChanges = true) {    
            var context = getContext();           
            getEntities(context).Remove(Model<T>.Get(id));

            if(applyChanges) {
                context.SaveChanges();
            }
        }

        public void Delete(bool applyChanges = true) {
            entities.Remove((T)this);
            
            if(applyChanges) {
                context.SaveChanges();
            }
        }

        public virtual void Update(bool applyChanges = true) { 
            var context = getContext();            
            getEntities(context).Update((T)this);

            if(applyChanges) {
                context.SaveChanges();
            }
        } 

        public virtual void UpdateAttributes(ExpandoObject updatedProperties) {
            T model = (T)this;

            SetAttributes(ref model, updatedProperties);
            getContext().SaveChanges();
        }

        public static void UpdateAttributes(long id, ExpandoObject updatedProperties)
        {
            T model = Get(id);

            SetAttributes(ref model, updatedProperties);
            getContext().SaveChanges();
        } 

        private static void SetAttributes(ref T model, ExpandoObject updatedProperties) {
            foreach (KeyValuePair<string, object> property in updatedProperties)
            {
                String propertyName = StringHelper.SnakeCaseToTitleCase(property.Key);
                PropertyInfo p = model.GetType().GetProperty(propertyName);

                if(p != null)
                    p.SetValue(model, property.Value);              
            }            
        }
    }
}
