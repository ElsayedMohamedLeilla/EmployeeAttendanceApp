using Dawem.Enums.Generals;

namespace Dawem.Models.Generic
{
    public class SearchResult<T>
    {
        public T? Result { get; set; }
        public int TotalCount { get; set; }
        public ResponseStatus State { get; set; }
        public string? MessageCode { get; set; }
        public string? Message { get; set; }
    }
}
