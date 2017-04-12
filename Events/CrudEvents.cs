using System;
using System.Collections.Generic;
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
        protected virtual void BeforeSave(ref T model) {

        }

        protected virtual void BeforeSave(ref T model,  ref bool trackModel) {

        }

        protected virtual void BeforeSearch(ref Expression<Func<T, bool>> predicate) {

        }

        protected virtual void BeforeSearch(ref Expression<Func<T, bool>> predicate, ref bool trackModels) {

        }

        protected virtual void BeforeDelete(long id) {

        }

        protected virtual void BeforeUpdate(ref T model) {

        }

        protected virtual void BeforeUpdate(ref T model, ref bool trackModel) {

        }

        protected virtual void BeforeValidate(ref Model<T> model) {

        }
       
        #endregion

        #region After Events       

        protected virtual void AfterGet(ref T model) {

        }

        protected virtual void AfterAll() {

        }
        
        protected virtual void AfterSave(ref T model) {

        }

        protected virtual void AfterSearch(Expression<Func<T, bool>> predicate, ref IEnumerable<T> models) {

        }

        protected virtual void AfterDelete(long id) {

        }

        protected virtual void AfterUpdate(ref T model) {

        }

        protected virtual void AfterValidate(ref Model<T> model) {

        }
       
        #endregion
    }
}