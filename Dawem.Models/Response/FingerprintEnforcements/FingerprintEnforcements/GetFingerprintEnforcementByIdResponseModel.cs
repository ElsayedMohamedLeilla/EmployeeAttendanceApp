using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Employees.AssignmentTypes
{
    public class GetFingerprintEnforcementByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
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
