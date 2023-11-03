using Microsoft.AspNetCore.Identity;

namespace Dawem.Domain.Entities.UserManagement
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}