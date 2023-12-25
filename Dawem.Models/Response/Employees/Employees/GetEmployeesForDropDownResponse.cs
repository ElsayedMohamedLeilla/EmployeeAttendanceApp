namespace Dawem.Models.Response.Employees.Employees
{
    public class GetEmployeesForDropDownResponse
    {
        public List<GetEmployeesForDropDownResponseModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
