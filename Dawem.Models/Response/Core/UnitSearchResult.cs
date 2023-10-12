using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class UnitSearchResult : BaseResponse
    {
        public List<UnitDTO?>? Units { get; set; }
    }
}
