namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeesResponse
    {
        public List<GetEmployeesResponseModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
