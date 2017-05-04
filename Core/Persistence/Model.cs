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

namespace StarterPack.Models
{
    public abstract class Model<T> where T :  Model<T>
    {
        public virtual long? Id { get; set; }
        
        [FromQuery(Name = "created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime? CreatedAt { get; set; }

        [FromQuery(Name = "updated_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
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
        }

        public Model(DbContext context) : this()
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

        public static EntityEntry<T> Entry(T model) {
            return getContext().Entry<T>(model);
        }

        public static IEnumerable<T> GetAll(bool tracked = false) {
            if(tracked) {
                return getEntities().AsEnumerable();
            }
            return getEntities().AsNoTracking().AsEnumerable();
        }

        public static IQueryable<T> BuildQueryById(long? id) {
            return BuildQuery(s => s.Id == id);          
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

        private static void SetAttributes(ref T model, ExpandoObject attributes) {
            foreach (KeyValuePair<string, object> attribute in attributes)
            {
                String propertyName = StringHelper.SnakeCaseToTitleCase(attribute.Key);
                PropertyInfo property = model.GetType().GetProperty(propertyName);                

                if(property != null) {
                    MergetProperty(model, property, attribute.Value);
                }
            }            
        }

        public void MergeAttributes(T updatedProperties) {
            foreach (PropertyInfo property in updatedProperties.GetType().GetProperties())
            {   
                MergetProperty(this, property, property.GetValue(updatedProperties));
            }            
        }

        private static void MergetProperty(Model<T> model, PropertyInfo property, dynamic value) {
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
