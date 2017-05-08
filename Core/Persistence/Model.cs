using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Dynamic;
using StarterPack.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using StarterPack.Core.Persistence;

namespace StarterPack.Models
{
    public abstract class Model<T> where T :  Model<T>
    {
        public virtual long? Id { get; set; }
        
        [FromQuery(Name = "created_at")]
        public virtual DateTime? CreatedAt { get; set; }

        [FromQuery(Name = "updated_at")]
        public virtual DateTime? UpdatedAt { get; set; }

        [NotMapped]
        protected virtual List<string> Fill { get; set; }

        [NotMapped]
        protected virtual List<string> DontFill { get; set; }

        protected readonly DbContext context;
        protected DbSet<T> entities;
        
        public Model()
        {
            context = getContext();
            entities = context.Set<T>();

            this.Fill = new List<string> {};
            this.DontFill = new List<string>() { "Password" };

            this.CreatedAt = this.UpdatedAt = DateTime.Now;
        }

        public Model(DbContext context) : this()
        {           
            this.context = context;
            entities = context.Set<T>();            
        }

        protected static DbContext getContext() {            
            return DatabaseContext.Context(GetContextType());
        }

        protected static Type GetContextType() {
            return typeof(DatabaseContext);
        }

        protected static DbSet<T> getEntities(DbContext context = null) {
            var _context = context == null ? getContext() : context;
            return _context.Set<T>();
        }        

        public static EntityEntry<T> Entry(T model) {
            return getContext().Entry<T>(model);
        }

        public static T Get(long id, bool tracked = true)
        {      
            if(tracked) {
                return getEntities().SingleOrDefault(s => s.Id == id);
            }
            return getEntities().AsNoTracking().SingleOrDefault(s => s.Id == id);
        }

        public static IQueryable<T> BuildQueryById(long id, bool tracked = true) {
            return BuildQuery(s => s.Id == id, tracked);          
        }            

        public static IEnumerable<T> GetAll(bool tracked = false) {
            if(tracked) {
                return getEntities().AsEnumerable();
            }
            return getEntities().AsNoTracking().AsEnumerable();
        }

        public static IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate, bool tracked = false) {  
            if(tracked) {
                return BuildQuery(predicate).AsEnumerable();
            }         
            
            return BuildQuery(predicate).AsNoTracking().AsEnumerable();
        }         

        public static IQueryable<T> Query(bool tracked = false) { 
            if(tracked) {
                return getEntities();
            }
            return getEntities().AsNoTracking();
        } 

        public static IQueryable<T> BuildRawQuery(string sql, params object[] parameters) { 
           return getEntities().FromSql(sql, parameters);
        }         

        public static IQueryable<T> BuildQuery(Expression<Func<T, bool>> predicate, bool tracked = false) {
            return Query(tracked).Where(predicate);          
        }         

        public static IQueryable<T> PaginatedQuery(int page, int perPage) {           
            return Query().Take(perPage).Skip((page-1) * perPage);
        } 

        public void Save(bool applyChanges = true) {
            if(this.Id != null) {
                Update(applyChanges);
            }
            else {
                Save((T)this, applyChanges);            
            }            
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
            T model = Model<T>.Get(id);   
          
            getEntities(context).Remove(model);

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
            UpdatedAt = DateTime.Now;

            if(applyChanges) {
                context.SaveChanges();
            }
        } 

        public virtual void UpdateAttributes(ExpandoObject updatedProperties) 
        {
            UpdateAttributes((T) this, updatedProperties);
        }

        public static void UpdateAttributes(long id, ExpandoObject updatedProperties)
        {            
            T model = Get(id);
            UpdateAttributes(model, updatedProperties);
        } 

        public void MergeAttributes(T updatedProperties) 
        {
            foreach (PropertyInfo property in updatedProperties.GetType().GetProperties())
            {   
                MergetProperty(this, property, property.GetValue(updatedProperties));
            }            
        }

        private static void UpdateAttributes(T model, ExpandoObject updatedProperties) 
        {            
            SetAttributes(ref model, updatedProperties);
            model.UpdatedAt = DateTime.Now;
            
            getContext().SaveChanges();
        }

        private static void SetAttributes(ref T model, ExpandoObject attributes) 
        {
            foreach (KeyValuePair<string, object> attribute in attributes)
            {
                String propertyName = StringHelper.SnakeCaseToTitleCase(attribute.Key);
                PropertyInfo property = model.GetType().GetProperty(propertyName);                

                if(property != null) {
                    MergetProperty(model, property, attribute.Value);
                }
            }            
        }        

        private static void MergetProperty(Model<T> model, PropertyInfo property, dynamic value) 
        {
            if(property.Name != "Id" 
            && !model.DontFill.Contains(property.Name) 
            && (model.Fill.Contains(property.Name) || model.Fill.Contains("*"))) {
                if(value != null) {
                    property.SetValue(model, value);              
                }
            }
        }                      
    }
   
}
