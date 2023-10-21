namespace Dawem.Models.Response.Employees
{
    public class GetDepartmentsForDropDownResponse
    {
        public List<GetDepartmentsForDropDownResponseModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
