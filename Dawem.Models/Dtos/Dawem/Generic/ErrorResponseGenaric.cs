namespace Dawem.Models.DTOs.Dawem.Generic
{
    public class ErrorResponseGenaric<T> : ErrorResponse
    {
        public T Data { get; set; }

    }
}
