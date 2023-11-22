namespace Dawem.Models.Dtos.Employees.Employees.GroupEmployees
{
    public class GroupEmployeeForGridDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeCount { get; set; }
        public GroupManagarForGridDTO Manager { get; set; } = null;

    }
}
