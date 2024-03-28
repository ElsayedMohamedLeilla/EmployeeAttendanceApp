namespace Dawem.Models.Dtos.Dawem.Core.PermissionsTypes
{
    public class UpdatePermissionTypeDTO
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
