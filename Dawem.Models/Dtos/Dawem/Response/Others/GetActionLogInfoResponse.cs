using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Others
{
    public class GetActionLogInfoResponse : BaseResponse
    {
        public ActionLogInfo? ActionLogInfo { get; set; }
    }
}
