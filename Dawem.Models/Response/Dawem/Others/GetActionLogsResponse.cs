using Dawem.Models.Dtos.Dawem.Others;

namespace Dawem.Models.Response.Dawem.Others
{
    public class GetActionLogsResponse : BaseResponse
    {
        public List<ActionLogDTO> ActionLogs { get; set; }
    }
}
