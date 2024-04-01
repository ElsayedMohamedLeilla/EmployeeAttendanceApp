namespace Dawem.Models.DTOs.Dawem.Generic.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException()
        {

        }
        public UnAuthorizedException(string message) : base(message)
        {

        }
    }
}
