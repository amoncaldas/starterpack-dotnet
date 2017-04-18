using System.Collections.Generic;

namespace StarterPack.Exception
{
    public class ApiValidationException : System.Exception
    {
        public int StatusCode { get; set; }

        public List<ApiError> ApiErrors {get; set;}        

        public ApiValidationException()  
        {            
            StatusCode = 422;
            ApiErrors = new List<ApiError>();
        }

        private static ApiValidationException _instance;

        public static ApiValidationException Instance
        {
            get 
            {
                if (_instance == null)
                {
                    _instance = new ApiValidationException();
                }
                return _instance;
            }
        }
    }
    
}