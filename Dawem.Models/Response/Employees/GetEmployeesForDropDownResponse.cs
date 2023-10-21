namespace Dawem.Models.Response.Employees
{
    public class GetEmployeesForDropDownResponse
    {
        public List<GetEmployeesForDropDownResponseModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
