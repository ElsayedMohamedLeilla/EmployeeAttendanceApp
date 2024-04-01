using Dawem.Enums.Generals;

namespace Dawem.Models.DTOs.Dawem.Generic
{
    public class ErrorResponse
    {
        public ResponseStatus State { get; set; }
        public string Message { get; set; }

    }
}
