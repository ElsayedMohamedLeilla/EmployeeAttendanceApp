namespace Dawem.Models.Response.Employees.Employee
{
    public class GetDepartmentsInformationsResponseModel
    {
        public List<DepartmentModel> Departments { get; set; }
        public int TotalCount { get; set; }
    }
}
