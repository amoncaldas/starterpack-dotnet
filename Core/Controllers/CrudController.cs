using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using StarterPack.Core.Validation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Persistence;
using StarterPack.Core.Extensions;
using System;
using System.Reflection;

namespace StarterPack.Core.Controllers
{
    [Route("api/v1/[controller]")]
    public abstract partial class CrudController<T> : BaseController where T : Model<T>
    {
        /// <summary>
        /// GET api/v1/[recurso]
        /// </summary>
        /// <param name="model">
        /// Parametros QueryString que é transformada no objeto tipado para serem utilizados como critérios de pesquisa
        ///
        /// Além disso os seguintes parametros:
        /// - perPage e page - para realizar uma paginação
        /// - limit - para limitar a quantidade de recursos retornados (somente no caso de não utilizar paginação)
        /// - orderBy - nome do campo para realizar a ordenação
        /// - orderType - asc ou desc para ascendente ou descendente
        /// </param>
        /// <returns>
        /// Caso os atributos page e perPage tenham sido informados,
        /// retorna um objeto com as propriedades items (array com os recursos encontrados referente a página especificada) e total.
        /// Caso não sejam informados, retorna uma lista com todos os recursos encontrados.
        /// </returns>
        [HttpGet]
        public object Index([FromQuery]T model)
        {
            bool trackModels = false;
            BeforeAll();
            IQueryable<T> dataQuery = Model<T>.Query();

            ApplyFilters(ref dataQuery);

            IQueryable<T> countQuery = dataQuery;

            BeforeSearch(ref dataQuery, ref countQuery);
            BeforeSearch(ref dataQuery, ref countQuery, ref trackModels);

            if (!trackModels)
                dataQuery = dataQuery.AsNoTracking();

            //Realiza a ordenação
            if (HasParameter("orderBy")) {
                if(HasParameter("orderType") && GetParameter("orderType") == "desc") {
                    dataQuery = dataQuery.OrderByDescending(GetParameter("orderBy"));
                } else {
                    dataQuery = dataQuery.OrderBy(GetParameter("orderBy"));
                }
            }

            List<T> models;
            int? count = null;

            if (HasParameter("page") && HasParameter("perPage")) {
                dataQuery = dataQuery.Paginate(GetParameter<int>("page"), GetParameter<int>("perPage"));
                count = countQuery.Count();
            } else {
                if (HasParameter("limit")) {
                    dataQuery = dataQuery.Take(GetParameter<int>("limit"));
                }
            }

            models = dataQuery.Fetch();

            AfterSearch(ref dataQuery, models);
            AfterAll();

            if (count == null) {
                return models;
            } else {
                return new {
                    Items = models,
                    Total = count
                };
            }
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public virtual T Get(long id)
        {
            bool trackModel = false;
            BeforeAll();
            BeforeGet(id, ref trackModel);
            T model = GetSingle(id, trackModel);
            AfterGet(model);
            AfterAll();
            return model;
        }

        // POST api/users
        [HttpPost]
        public IActionResult Store([FromBody]T attributes)
        {
            BeforeAll();
            Validate(attributes);

            T model = (T) Activator.CreateInstance(typeof(T));
            model.FillAttributes(attributes);

            BeforeStore(model, attributes);
            BeforeSave(model, attributes);
            model.Save();
            AfterStore(model);
            AfterSave(model);
            AfterAll();
            return StatusCode(201, model);
        }

       // POST api/users
        [HttpPut("{id}")]
        public virtual IActionResult Update(long id, [FromBody]T attributes)
        {
            T model = GetSingle(id, true);
            model.FillAttributes(attributes);

            BeforeAll();
            Validate(model);
            BeforeUpdate(model, attributes);
            BeforeSave(model, attributes);
            model.Update();
            AfterUpdate(model);
            AfterSave(model);
            AfterAll();

            return StatusCode(201, model);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Destroy(long id)
        {
            BeforeAll();
            BeforeDelete(id);
            Model<T>.Delete(id);
            AfterDelete(id);
            AfterAll();
        }

        protected void Validate(T model) {
            ModelValidator<T> modelValidator = new ModelValidator<T>();
            SetValidationRules(model, modelValidator);
			ValidationResult results = modelValidator.Validate(model);
            BeforeValidate(model, modelValidator);
			Exception.ValidationException validationException = new Exception.ValidationException();
            validationException.Errors.AddRange(results.Errors);
            bool mustContinue = AfterValidate(model, validationException);

            if(!results.IsValid && mustContinue) {
                throw validationException;
            }
        }

        protected virtual T GetSingle(long id, bool tracked = true) {
            return Model<T>.Get(id, tracked);
        }

    }
}
