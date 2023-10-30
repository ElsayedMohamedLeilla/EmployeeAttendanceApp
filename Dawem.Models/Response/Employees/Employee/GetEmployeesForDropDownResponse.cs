namespace Dawem.Models.Response.Employees.Employee
{
    public class GetEmployeesForDropDownResponse
    {
        public List<GetEmployeesForDropDownResponseModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
