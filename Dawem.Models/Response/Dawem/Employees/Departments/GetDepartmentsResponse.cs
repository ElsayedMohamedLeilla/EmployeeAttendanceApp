namespace Dawem.Models.Response.Dawem.Employees.Departments
{
    public class GetDepartmentsResponse
    {
        public List<GetDepartmentsResponseModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
