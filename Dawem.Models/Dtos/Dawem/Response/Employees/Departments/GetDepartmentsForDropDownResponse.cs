namespace Dawem.Models.Response.Employees.Departments
{
    public class GetDepartmentsForDropDownResponse
    {
        public List<GetDepartmentsForDropDownResponseModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
