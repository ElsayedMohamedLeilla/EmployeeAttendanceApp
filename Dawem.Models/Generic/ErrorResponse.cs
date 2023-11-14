using Dawem.Enums.Generals;

namespace Dawem.Models.Generic
{
    public class ErrorResponse
    {
        public ResponseStatus State { get; set; }
        public string Message { get; set; }

    }
}
