namespace Dawem.Models.Exceptions
{
    public class CartBranchOverlapException : Exception
    {
        public string MessageCode;
        public new string Message;

        public CartBranchOverlapException()
        {

        }
        public CartBranchOverlapException(string messageCode)
        {
            MessageCode = messageCode;
        }
        public CartBranchOverlapException(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
    }
}
