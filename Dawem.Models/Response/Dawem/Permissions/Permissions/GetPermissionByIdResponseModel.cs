using Dawem.Enums.Permissions;

namespace Dawem.Models.Response.Dawem.Permissions.Permissions
{
    public class GetPermissionByIdResponseModel
    {
        public int Id { get; set; }
        public ForResponsibilityOrUser ForType { get; set; }
        public int? ResponsibilityId { get; set; }
        public int? UserId { get; set; }
        public int Code { get; set; }
        public List<PermissionScreenResponseModel> Screens { get; set; }
        public bool IsActive { get; set; }
    }
}
