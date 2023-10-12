using Dawem.Models.Dtos.Core;

namespace Dawem.Models.Response.Core
{
    public class GetUnitInfoResponse : BaseResponse
    {
        public UnitInfo? UnitInfo { get; set; }
    }
}
