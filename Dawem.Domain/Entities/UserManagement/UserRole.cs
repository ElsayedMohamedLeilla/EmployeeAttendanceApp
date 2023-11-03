using Microsoft.AspNetCore.Identity;

namespace Dawem.Domain.Entities.UserManagement
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual MyUser User { get; set; }
        public virtual Role Role { get; set; }
    }
}