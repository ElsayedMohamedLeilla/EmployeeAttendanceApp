using Dawem.Enums.Generals;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dawem.Models.Response.Schedules.SchedulePlans
{
    public class GetSchedulePlansResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string ScheduleName { get; set; }
        public string SchedulePlanTypeName { get; set; }
        public DateTime DateFrom { get; set; }
        public bool IsActive { get; set; }
    }
}
