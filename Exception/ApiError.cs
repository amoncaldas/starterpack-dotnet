namespace Starterpack.Exception
{
    public class ApiError
    {
        public string error { get; set; }

        public ApiError(string error)
        {
            this.error = error;
        }
    }
}