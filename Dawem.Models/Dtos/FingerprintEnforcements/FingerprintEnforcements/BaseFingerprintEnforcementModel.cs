using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.FingerprintEnforcements.FingerprintEnforcements
{
    public class BaseFingerprintEnforcementModel
    {
        public ForType ForType { get; set; }       
        public bool? ForAllEmployees { get; set; }
        public DateTime FingerprintDate { get; set; }
        public int AllowedTime { get; set; }
        public TimeType TimeType { get; set; }
        public List<NotifyWay> NotifyWays { get; set; }
        public List<int> Employees { get; set; }
        public List<int> Groups { get; set; }
        public List<int> Departments { get; set; }
        public List<int> Actions { get; set; }
        public bool IsActive { get; set; }
    }
}
