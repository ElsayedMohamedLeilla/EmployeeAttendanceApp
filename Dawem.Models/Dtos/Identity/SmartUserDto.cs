using Dawem.Models.Dtos.Core;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Dtos.Identity
{
    public class SmartUserDTO
    {
        public SmartUserDTO()
        {

            UserRols = new List<SmartUserRoleDTO>();

        }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int BranchId { get; set; }
        public virtual string? UserName { get; set; }
        public bool IsActive { get; set; }
        public string? MobileNumber { get; set; }
        public virtual string? Email { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual string? PhoneNumber { get; set; }
        public virtual string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public List<SmartUserRoleDTO>? UserRols { get; set; }

        public List<UserBranchDTO>? UserBranches { get; set; }
        public List<UserGroupDTO?>? UserGroups { get; set; }

    }


}
