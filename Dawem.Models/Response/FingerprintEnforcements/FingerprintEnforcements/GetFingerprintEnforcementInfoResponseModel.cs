using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetFingerprintEnforcementInfoResponseModel
    {
        public int Code { get; set; }
        public ForType ForType { get; set; }
        public string ForTypeName { get; set; }
        public bool? ForAllEmployees { get; set; }
        public DateTime FingerprintDate { get; set; }
        public int AllowedTime { get; set; }
        public TimeType TimeType { get; set; }
        public List<string> NotifyWays { get; set; }
        public List<string> Employees { get; set; }
        public List<string> Groups { get; set; }
        public List<string> Departments { get; set; }
        public List<string> Actions { get; set; }
        public bool IsActive { get; set; }
    }
}
