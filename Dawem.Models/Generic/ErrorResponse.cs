using Dawem.Enums.General;

namespace Dawem.Models.Generic
{
    public class ErrorResponse
    {
        public ResponseStatus State { get; set; }
        public string Message { get; set; }

    }
}
