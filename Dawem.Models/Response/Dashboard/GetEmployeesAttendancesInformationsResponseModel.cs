namespace Dawem.Models.Response.Dashboard
{
    public class GetEmployeesAttendancesInformationsResponseModel
    {
        public int DayTotalAttendanceCount { get; set; }
        public int DayTotalVacationsCount { get; set; }
        public int DayTotalAbsencesCount { get; set; }
        public int DayTotalDelaysCount { get; set; }
    }
}
