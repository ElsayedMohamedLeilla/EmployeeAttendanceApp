using Dawem.Models.Response.Dawem.Permissions.Permissions;
using Dawem.Models.Response.Dawem.Screens;

namespace Dawem.Models.Dtos.Dawem.Identities
{
    public class TokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public List<PermissionScreenResponseWithNamesModel> AvailablePermissions { get; set; }
    }
}
