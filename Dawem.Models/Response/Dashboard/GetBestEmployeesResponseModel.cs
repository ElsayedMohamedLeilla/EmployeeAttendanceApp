namespace Dawem.Models.Response.Employees.Employee
{
    public class GetBestEmployeesResponseModel
    {
        public List<EmployeeModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
