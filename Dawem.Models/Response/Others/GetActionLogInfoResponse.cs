using Dawem.Models.Dtos.Others;

namespace Dawem.Models.Response.Others
{
    public class GetActionLogInfoResponse : BaseResponse
    {
        public ActionLogInfo? ActionLogInfo { get; set; }
    }
}
