using System.Collections.Generic;
using System.Linq;
using StarterPack.Core.Validation;
using StarterPack.Core.Exception;
using StarterPack.Core.Persistence;

namespace StarterPack.Core.Controllers
{
    public abstract partial class CrudController<T> : BaseController where T : Model<T>
    {

        #region Before Events

        protected virtual void BeforeAll() {

        }

        protected virtual void BeforeGet(long id) {

        }

        protected virtual void BeforeGet(long id, ref bool trackModel) {

        }

        protected virtual void BeforeSave(T model, T attributes) {

        }

        protected virtual void BeforeStore(T model, T attributes) {

        }

        protected virtual void BeforeSearch(ref IQueryable<T> dataQuery, ref IQueryable<T> countQuery) {

        }

        protected virtual void BeforeSearch(ref IQueryable<T> dataQuery, ref IQueryable<T> countQuery, ref bool trackModel) {

        }

        protected virtual void ApplyFilters(ref IQueryable<T> query) {

        }

        protected virtual void BeforeDelete(long id) {

        }

        protected virtual void BeforeUpdate(T model, T modelAttributes) {

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

        protected virtual void BeforeValidate(T model, ModelValidator<T> validator) {

        }
        protected virtual bool AfterValidate(T model, ValidationException validationErrors) {
            return true;
        }

        #endregion

        protected abstract void SetValidationRules(T model, ModelValidator<T> validator);
    }
}
