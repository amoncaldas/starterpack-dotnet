namespace StarterPack.Core.Exception
{
    public class ApiError
    {
        public string error { get; set; }

        public string source { get; set; }

        public ApiError(string error)
        {
            this.error = error;
        }

        public ApiError(string error, string source)
        {
            this.source = source;
            this.error = error;
        }
    }
}