using Dawem.Enums.Generals;

namespace Dawem.Models.Response.Attendances.Schedules
{
    public class GetSchedulePlansResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int ScheduleName { get; set; }
        public SchedulePlanType SchedulePlanType { get; set; }
        public DateTime DateFrom { get; set; }
        public bool IsActive { get; set; }
    }
}
