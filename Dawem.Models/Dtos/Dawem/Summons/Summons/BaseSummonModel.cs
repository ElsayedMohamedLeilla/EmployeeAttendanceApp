using Dawem.Enums.Generals;

namespace Dawem.Models.Dtos.Dawem.Summons.Summons
{
    public class BaseSummonModel
    {
        public ForType ForType { get; set; }
        public bool? ForAllEmployees { get; set; }
        public DateTime LocalDateAndTime { get; set; }
        public int AllowedTime { get; set; }
        public TimeType TimeType { get; set; }
        public List<NotifyWay> NotifyWays { get; set; }
        public List<int> Employees { get; set; }
        public List<int> Groups { get; set; }
        public List<int> Departments { get; set; }
        public List<int> Sanctions { get; set; }
        public bool IsActive { get; set; }
    }
}
