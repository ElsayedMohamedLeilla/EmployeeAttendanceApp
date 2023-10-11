using SmartBusinessERP.Models.Dtos.Others;

namespace SmartBusinessERP.Models.Response.Others
{
    public class GetActionLogsResponse : BaseResponse
    {
        public List<ActionLogDTO>? ActionLogs { get; set; }
    }
}
