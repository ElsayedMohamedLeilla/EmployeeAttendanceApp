using SmartBusinessERP.Models.Dtos.Others;

namespace SmartBusinessERP.Models.Response.Core
{
    public class GetActionLogInfoResponse : BaseResponse
    {
        public ActionLogInfo? ActionLogInfo { get; set; }
    }
}
