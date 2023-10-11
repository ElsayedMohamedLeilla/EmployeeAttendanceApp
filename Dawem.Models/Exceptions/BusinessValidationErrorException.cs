using FluentValidation.Results;

namespace Dawem.Models.Exceptions
{
    public class BusinessValidationErrorException : Exception
    {
        public string MessageCode;
        public new string Message;
        public BusinessValidationErrorException(string messageCode)
        {
            MessageCode = messageCode;
        }
        public BusinessValidationErrorException(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
        public BusinessValidationErrorException(ValidationResult validateResult)
        {
            var error = validateResult.Errors.FirstOrDefault();
            MessageCode = error.ErrorCode;
            Message = error.ErrorMessage;
        }

    }
}
