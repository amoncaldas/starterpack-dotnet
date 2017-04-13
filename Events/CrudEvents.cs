using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Models;

namespace StarterPack.Controllers
{
    public abstract partial class CrudController<T> : Controller where T : Model<T>
    {
       
        #region Before Events       
        
        protected virtual void BeforeAll() {

        }

        protected virtual void BeforeAll(ref bool trackModel) {

        }

        protected virtual void BeforeGet(long id) {

        }

        protected virtual void BeforeGet(long id, ref bool trackModel) {

        }
        protected virtual void BeforeSave(T model) {

        }

        protected virtual void BeforeSave(T model,  ref bool trackModel) {

        }

        protected virtual void BeforeSearch(ref IQueryable<T> query) {

        }

        protected virtual void BeforeSearch(ref IQueryable<T> query, ref bool trackModels) {

        }

        protected virtual void ApplyFilters(ref IQueryable<T> query) {

        }

        protected virtual void BeforeDelete(long id) {

        }

        protected virtual void BeforeUpdate(T model) {

        }

        protected virtual void BeforeUpdate(T model, ref bool trackModel) {

        }

        protected virtual void BeforeValidate(Model<T> model) {

        }
       
        #endregion

        #region After Events       

        protected virtual void AfterGet(T model) {

        }

        protected virtual void AfterAll() {

        }
        
        protected virtual void AfterSave(T model) {

        }

        protected virtual void AfterSearch(ref IQueryable<T> query, IEnumerable<T> models) {

        }

        protected virtual void AfterSearch(IEnumerable<T> models) {

        }

        protected virtual void AfterDelete(long id) {

        }

        protected virtual void AfterUpdate(T model) {

        }

        protected virtual void AfterValidate(Model<T> model) {

        }
       
        #endregion
    }
}