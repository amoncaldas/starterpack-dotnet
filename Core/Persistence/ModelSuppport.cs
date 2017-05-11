using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations.Schema;
using StarterPack.Core.Persistence;
using StarterPack.Core.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace StarterPack.Core.Persistence
{    
    public  abstract partial class Model<T> where T :  Model<T>
    {
         /// <summary>
        /// Recupera o contexto de dados. Por padrão é recuperado o DefaultDbContext. 
        /// Caso o model vá utilizar um contexto de dados (conexão/banco) diferente deve 
        /// implementar o método protected "new static DbContext getContext() { /* return your db context */}"
        /// </summary>
        /// <returns></returns>
        protected static DbContext getContext() {            
            return DbContextAccessor.GetContext(typeof(DefaultDbContext));
        }        

        /// <summary>
        /// Recupera a referência às entidades de dados
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected static DbSet<T> getEntities(DbContext context = null) {
            var _context = context == null ? getContext() : context;
            return _context.Set<T>();
        }   

        /// <summary>
        /// Atualiza atributos de um model com valores definidos em um objeto do tipo <dynamic>, permitindo atualização de campos selecionados
        /// </summary>
        /// <param name="model"></param>
        /// <param name="attributes"></param>
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

        /// <summary>
        /// Realiza o merge de uma propriedade de um model e um objeto dynamic
        /// </summary>
        /// <param name="model"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public static void MergetProperty(Model<T> model, PropertyInfo property, dynamic value) 
        {
            if(property.Name != "Id" 
            && !model.DontFill.Contains(property.Name) 
            && (model.Fill.Contains(property.Name) || model.Fill.Contains("*"))) {
                if(value != null) {
                    property.SetValue(model, value);              
                }
            }
        }  

        /// <summary>
        /// Atualiza os atributos de uma intância de um model a partir de um objeto dynamic
        /// </summary>
        /// <param name="updatedProperties"></param>
        public virtual void UpdateAttributes(ExpandoObject updatedProperties) 
        {
            UpdateAttributes((T) this, updatedProperties);           
        }

        /// <summary>
        /// Atualiza os atributos de um model passado a partir de um objeto dynamic passado
        /// </summary>
        /// <param name="model"></param>
        /// <param name="updatedProperties"></param>
        public static void UpdateAttributes(T model, ExpandoObject updatedProperties) 
        {            
            SetAttributes(ref model, updatedProperties);
            model.UpdatedAt = DateTime.Now;
            
            getContext().SaveChanges();
        }

        /// <summary>
        /// Realiza o merge entre dois models
        /// </summary>
        /// <param name="updatedProperties"></param>
        public void MergeAttributes(T updatedProperties) 
        {
            foreach (PropertyInfo property in updatedProperties.GetType().GetProperties())
            {   
                MergetProperty(this, property, property.GetValue(updatedProperties));
            }            
        }
    }
}