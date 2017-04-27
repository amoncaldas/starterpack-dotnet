using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarterPack.Core.Validation;
using StarterPack.Core.Exception;
using StarterPack.Models;

namespace StarterPack.Core.Controllers
{
    public abstract partial class CrudController<T> : Controller where T : Model<T>
    {
       
        #region Before Events       
        
        protected virtual T GetSingle(long id) {
            return Model<T>.Get(id);
        }

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

        protected virtual void BeforeStore(T model) {

        }

        protected virtual void BeforeStore(T model,  ref bool trackModel) {

        }        

        protected virtual void BeforeSearch(ref IQueryable<T> query) {

        }

        protected virtual void BeforeSearch(ref IQueryable<T> query, ref bool trackModels) {

        }

        protected virtual void ApplyFilters(ref IQueryable<T> query) {

        }

        protected virtual void BeforeDelete(long id) {

        }

        protected virtual void BeforeUpdate(T model, T modelAttributes, ref bool trackModel) {

        }

       
       
        #endregion

        #region After Events       

        protected virtual void AfterGet(T model) {

        }

        protected virtual void AfterAll() {

        }
        
        protected virtual void AfterSave(T model) {

        }

        protected virtual void AfterStore(T model) {

        }        

        protected virtual void AfterSearch(ref IQueryable<T> query, List<T> models) {

        }

        protected virtual void AfterDelete(long id) {

        }

        protected virtual void AfterUpdate(T model) {

        }        
       
        #endregion



        #region validation events

        protected virtual void BeforeValidate(Model<T> model, ModelValidator<T> validator) {

        }
        protected virtual bool AfterValidate(Model<T> model, ValidationException validationErrors) {
            return true;
        }       

        #endregion

        protected abstract void SetValidationRules(Model<T> model, ModelValidator<T> validator);
    }
}