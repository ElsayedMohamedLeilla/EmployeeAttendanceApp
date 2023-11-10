using Dawem.Models.Dtos.Employees.Attendance;

namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
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
