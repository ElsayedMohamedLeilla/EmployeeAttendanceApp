namespace Dawem.Models.Dtos.Dawem.Permissions.Permissions
{
    public class GetPermissionScreensCriteria : BaseGetPermissionScreensCriteria
    {
        public int? ScreenId { get; set; }
        public int? ScreenCode { get; set; }
    }
}
