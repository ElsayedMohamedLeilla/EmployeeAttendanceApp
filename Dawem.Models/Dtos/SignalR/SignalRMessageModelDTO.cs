

namespace Dawem.Models.Dtos.SignalR
{
    public class SignalRMessageModelDTO
    {
        public string Title { get; set; }
        public string MessageDescription { get; set; }
        public string IconUrl { get; set; }
        public bool IsRead { get; set; }
        public string Priority { get; set; }
        public string NotificationType { get; set; }
        public string EmployeeName { get; set; }  
    }
}
