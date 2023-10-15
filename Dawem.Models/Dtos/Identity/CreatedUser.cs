using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.General;

namespace Dawem.Models.Dtos.Identity
{
    public class CreatedUser
    {
        public int Id { get; set; }
        public virtual string UserName { get; set; }
        public int MainBranchId { get; set; }
        public bool IsActive { get; set; }
        public string MobileNumber { get; set; }
        public virtual string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public List<string> UserRols { get; set; }
        public List<UserBranch> UserBranches { get; set; }
        public List<UserGroup> UserGroups { get; set; }
    }


}
