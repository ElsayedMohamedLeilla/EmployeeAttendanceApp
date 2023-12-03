namespace Dawem.Models.Generic
{
    public class ErrorResponseGenaric<T> : ErrorResponse
    {
        public T Data { get; set; }

    }
}
