namespace Dawem.Models.Response.Employees.Attendances.WeeksAttendances
{
    public class GetWeekAttendancesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
    }
}
