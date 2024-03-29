namespace Dawem.Models.Response.Dawem.Dashboard
{
    public class GetDepartmentsInformationsResponseModel
    {
        public List<DepartmentModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
