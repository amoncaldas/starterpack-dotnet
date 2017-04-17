
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StarterPack.Exception;

namespace StarterPack.Core.Validation
{
    public class ValidationState
    {
        protected ModelStateDictionary State;

        public ValidationState(ModelStateDictionary state) {
            this.State = state;
        }
        

        public void AddValidationError(string source, string message) {            
            this.State.AddModelError(source,message);
        }

        public void AddValidationError(ApiValidationException apiValidationException) {            
            this.State.AddModelError(apiValidationException.Source,apiValidationException.Message);
        }


        public bool ValidateModel(object model){          
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();      
            // Add the results to the State   here     
            return Validator.TryValidateObject(model, context, results); 
        }

        public void clearValidationState(string key) {
            State.ClearValidationState(key);
        }

        public List<ApiError> getErrors(){
            List<ApiError> errors = new List<ApiError>();            
           
            foreach (KeyValuePair<string, ModelStateEntry> modelState in this.State.ToList())
            {
                foreach (ModelError modelError in modelState.Value.Errors)
                {                
                    errors.Add(new ApiError(modelError.ErrorMessage, modelState.Key));
                }                            
            }
            
            return errors;
        }

        public bool HasError(){
            return State.ErrorCount > 0;
        }
        
    }
}