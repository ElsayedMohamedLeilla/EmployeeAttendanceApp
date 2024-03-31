namespace Dawem.Models.DTOs.Dawem.Generic.Exceptions
{
    public class NotFoundException : Exception
    {
        public string MessageCode;
        public new string Message;
        public NotFoundException(string messageCode)
        {
            MessageCode = messageCode;
        }
        public NotFoundException(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
    }
}
