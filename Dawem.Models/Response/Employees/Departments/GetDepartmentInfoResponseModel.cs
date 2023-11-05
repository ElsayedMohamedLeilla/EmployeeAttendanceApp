namespace Dawem.Models.Response.Employees.Departments
{
    public class GetDepartmentInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public bool IsActive { get; set; }
    }
}
