using Dawem.Models.Dtos.Core;
using Dawem.Models.Dtos.Provider;

namespace Dawem.Models.Dtos.Identity
{
    public class UserInfo
    {
        public UserInfo()
        {

        }
        public int Id { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int? BranchId { get; set; }
        public string? UserName { get; set; }
        public bool? IsActive { get; set; }
        public string? MobileNumber { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public List<UserBranchDTO?>? UserBranches { get; set; }
    }


}
