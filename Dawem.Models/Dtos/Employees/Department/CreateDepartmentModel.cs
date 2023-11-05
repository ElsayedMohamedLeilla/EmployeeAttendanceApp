namespace Dawem.Models.Dtos.Employees.Department
{
    public class CreateDepartmentModel
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int? ParentId { get; set; }
    }
}
