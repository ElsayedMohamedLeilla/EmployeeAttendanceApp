using Dawem.Models.Dtos.Employees.Employees;

namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
{
    public class GetWeekAttendanceByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Notes { get; set; }
        public List<WeekAttendanceShiftUpdateModel> WeekShifts { get; set; }
    }
}
