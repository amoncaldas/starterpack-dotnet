namespace StarterPack.Core.Exception
{
    public class BusinessException : System.Exception
    {
        public BusinessException(string error) : base(error)
        {
        }
    }
}
