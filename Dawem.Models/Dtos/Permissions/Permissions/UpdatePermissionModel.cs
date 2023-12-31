namespace Dawem.Models.Dtos.Employees.AssignmentTypes
{
    public class UpdatePermissionModel
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public List<PermissionScreenModel> PermissionScreens { get; set; }
        public bool IsActive { get; set; }
    }
}
