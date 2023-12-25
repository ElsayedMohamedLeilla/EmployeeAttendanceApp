namespace Dawem.Models.Response.Employees.Employees
{
    public class GetEmployeesResponse
    {
        public List<GetEmployeesResponseModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
