using System.Collections.Generic;
using FluentValidation.Results;

namespace StarterPack.Core.Exception
{
    public class ValidationException : System.Exception
    {
        public List<ValidationFailure> Errors {get; set;}        

        public ValidationException() 
        {   
            Errors = new List<ValidationFailure>();
        }
       
        public ValidationException(string error) 
        {   
            Errors = new List<ValidationFailure>();
            Errors.Add(new ValidationFailure(null, error));
        }

        public ValidationException(string property, string error) 
        {   
            Errors = new List<ValidationFailure>();
            Errors.Add(new ValidationFailure(property, error));
        }

        public ValidationException(IList<ValidationFailure> errors) 
        {   
            Errors = (List<ValidationFailure>)errors;            
        }
    }
    
}