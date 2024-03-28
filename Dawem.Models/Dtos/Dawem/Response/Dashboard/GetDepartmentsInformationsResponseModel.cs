namespace Dawem.Models.Response.Dashboard
{
    public class GetDepartmentsInformationsResponseModel
    {
        public List<DepartmentModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
