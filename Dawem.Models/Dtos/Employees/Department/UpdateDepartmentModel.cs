namespace Dawem.Models.Dtos.Employees.Department
{
    public class UpdateDepartmentModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
    }
}
