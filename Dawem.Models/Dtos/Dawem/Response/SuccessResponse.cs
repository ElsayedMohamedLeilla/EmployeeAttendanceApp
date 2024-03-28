using Dawem.Enums.Generals;
using Dawem.Translations;

namespace Dawem.Models.Response
{
    public class SuccessResponse<T>
    {
        public SuccessResponse(T data, int? totalCount = null, string message = LeillaKeys.DoneSuccessfully)
        {
            State = ResponseStatus.Success;
            Message = message;
            Data = data;
            TotalCount = totalCount;
        }
        public ResponseStatus State { get; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int? TotalCount { get; set; }
    }
}
