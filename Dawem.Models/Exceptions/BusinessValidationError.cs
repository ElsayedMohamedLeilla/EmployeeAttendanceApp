using FluentValidation.Results;

namespace Dawem.Models.Exceptions
{
    public class BusinessValidationError : Exception
    {
        public string MessageCode;
        public new string Message;

        public BusinessValidationError(string messageCode)
        {
            MessageCode = messageCode;
        }
        public BusinessValidationError(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
        public BusinessValidationError(ValidationResult validateResult)
        {
            var error = validateResult.Errors.FirstOrDefault();
            MessageCode = error.ErrorCode;
            Message = error.ErrorMessage;
        }
    }
}
