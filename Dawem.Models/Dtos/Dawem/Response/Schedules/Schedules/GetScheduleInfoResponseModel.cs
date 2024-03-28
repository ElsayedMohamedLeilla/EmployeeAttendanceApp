using Dawem.Models.Dtos.Dawem.Schedules.Schedules;

namespace Dawem.Models.Response.Schedules.Schedules
{
    public class GetScheduleInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public List<ScheduleShiftTextModel> ScheduleDays { get; set; }
    }
}
