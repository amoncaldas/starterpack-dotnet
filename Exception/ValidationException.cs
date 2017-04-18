using System.Collections.Generic;
using FluentValidation.Results;

namespace StarterPack.Exception
{
    public class ValidationException : System.Exception
    {

        public List<ValidationFailure> Errors {get; set;}        

        public ValidationException() 
        {   
            Errors = new List<ValidationFailure>();
        }
    }
    
}