using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Others
{
    public class ActionLogInfo : ActionLogDTO
    {
        public ActionLogInfo()
        {
        }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public string? UserGlobalName { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
}