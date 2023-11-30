namespace Dawem.Models.Response.Employees.Departments
{
    public class GetDepartmentsForTreeResponse
    {
        public List<GetDepartmentsForTreeResponseModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
