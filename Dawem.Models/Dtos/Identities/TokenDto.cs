using Dawem.Models.Response.Permissions.Permissions;

namespace Dawem.Models.Dtos.Identities
{
    public class TokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public List<PermissionScreenResponseWithNamesModel> AvailablePermissions { get; set; }
    }
}
