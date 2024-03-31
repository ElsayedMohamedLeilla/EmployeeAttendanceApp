using FluentValidation.Results;
namespace Dawem.Models.DTOs.Dawem.Generic.Exceptions
{
    public class ForbiddenException : Exception
    {
        public string MessageCode;
        public new string Message;

        public ForbiddenException(string messageCode)
        {
            MessageCode = messageCode;
        }
        public ForbiddenException(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
        public ForbiddenException(ValidationResult validateResult)
        {
            var error = validateResult.Errors.FirstOrDefault();
            MessageCode = error.ErrorCode;
            Message = error.ErrorMessage;
        }
    }
}
