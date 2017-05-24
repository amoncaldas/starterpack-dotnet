namespace StarterPack.Core.Exception
{
    /// <summary>
    /// Exception para ser utilizada quando acontece um erro referente a alguma regra de negócio
    /// </summary>
    public class BusinessException : System.Exception
    {
        public BusinessException(string error) : base(error)
        {
        }
    }
}
