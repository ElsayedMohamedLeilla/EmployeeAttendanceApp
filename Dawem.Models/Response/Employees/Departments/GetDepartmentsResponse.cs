namespace Dawem.Models.Response.Employees
{
    public class GetDepartmentsResponse
    {
        public List<GetDepartmentsResponseModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
