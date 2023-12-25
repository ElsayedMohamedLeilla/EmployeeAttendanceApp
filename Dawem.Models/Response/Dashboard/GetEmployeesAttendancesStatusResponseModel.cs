namespace Dawem.Models.Response.Dashboard
{
    public class GetEmployeesAttendancesStatusResponseModel
    {
        public List<EmployeeModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
