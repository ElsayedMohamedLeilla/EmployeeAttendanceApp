namespace Dawem.Models.Response.Dawem.Employees.Employees
{
    public class GetEmployeesResponse
    {
        public List<GetEmployeesResponseModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
