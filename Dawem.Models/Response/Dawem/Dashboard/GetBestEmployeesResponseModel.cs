namespace Dawem.Models.Response.Dawem.Dashboard
{
    public class GetBestEmployeesResponseModel
    {
        public List<EmployeeModel> Employees { get; set; }
        public int TotalCount { get; set; }
    }
}
