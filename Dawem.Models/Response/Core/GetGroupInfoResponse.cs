using SmartBusinessERP.Models.Dtos.Core;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetGroupInfoResponse : BaseResponse
    {
        public GroupInfo? GroupInfo { get; set; }
    }
}
