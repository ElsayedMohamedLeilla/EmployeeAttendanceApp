using Dawem.Models.Dtos.Others;

namespace Dawem.Models.Response.Others
{
    public class GetActionLogsResponseModel
    {
        public List<ActionLogDTO>? ActionLogs { get; set; }
        public int TotalCount { get; set; }
    }
}
