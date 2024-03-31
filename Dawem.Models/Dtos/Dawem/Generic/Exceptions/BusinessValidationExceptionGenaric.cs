using FluentValidation.Results;

namespace Dawem.Models.DTOs.Dawem.Generic.Exceptions
{
    public class BusinessValidationExceptionGenaric<T> : Exception
    {
        public string MessageCode;
        public new string Message;
        public new T Data { get; set; }

        public BusinessValidationExceptionGenaric(string messageCode)
        {
            MessageCode = messageCode;
        }
        public BusinessValidationExceptionGenaric(string messageCode, string message) : base(message)
        {
            MessageCode = messageCode;
            Message = message;
        }
        public BusinessValidationExceptionGenaric(ValidationResult validateResult)
        {
            var error = validateResult.Errors.FirstOrDefault();
            MessageCode = error.ErrorCode;
            Message = error.ErrorMessage;
        }
    }
}
