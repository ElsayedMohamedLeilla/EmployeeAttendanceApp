namespace Dawem.Models.Response.Employees.Departments
{
    public class GetDepartmentByIdResponseModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
    }
}
