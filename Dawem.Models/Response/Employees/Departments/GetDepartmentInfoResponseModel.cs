namespace Dawem.Models.Response.Employees.Departments
{
    public class GetDepartmentInfoResponseModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public bool IsActive { get; set; }
        public string Manager { get; set; }
        public List<string> ManagerDelegators { get; set; }
        public List<string> Zones { get; set; }

    }
}
