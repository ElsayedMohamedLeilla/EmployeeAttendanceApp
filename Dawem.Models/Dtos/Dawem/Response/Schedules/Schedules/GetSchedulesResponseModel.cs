namespace Dawem.Models.Response.Schedules.Schedules
{
    public class GetSchedulesResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int? EmployeesNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
