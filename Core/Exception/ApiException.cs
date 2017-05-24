namespace StarterPack.Core.Exception
{
    /// <summary>
    /// Exception para ser utilizada nos Controllers
    /// Tem a possibilidade de expecificar o código http coerente com o erro
    /// </summary>
    public class ApiException : System.Exception
    {
        public int StatusCode { get; set; }

        public ApiException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
        public ApiException(System.Exception ex, int statusCode = 500) : base(ex.Message)
        {
            StatusCode = statusCode;
        }
    }
}
