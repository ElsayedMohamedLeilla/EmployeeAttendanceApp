namespace Dawem.Models.Response.Schedules.SchedulePlans
{
    public class GetVacationBalancesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string EmployeeName { get; set; }
        public int Year { get; set; }
        public string VacationTypeName { get; set; }
        public float Balance { get; set; }
        public bool IsActive { get; set; }
    }
}
