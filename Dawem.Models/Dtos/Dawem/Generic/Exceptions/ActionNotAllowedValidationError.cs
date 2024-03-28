namespace Dawem.Models.Generic.Exceptions
{
    public class ActionNotAllowedValidationError : Exception
    {
        public string MessageCode;
        public new string Message;

        public ActionNotAllowedValidationError(string messageCode)
        {
            MessageCode = messageCode;
        }
        public ActionNotAllowedValidationError(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
    }
}
