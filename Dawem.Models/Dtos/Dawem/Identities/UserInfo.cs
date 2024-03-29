using Dawem.Models.Dtos.Dawem.Providers;

namespace Dawem.Models.Dtos.Dawem.Identities
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
        public string UserName { get; set; }
        public bool? IsActive { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<UserBranchDTO> UserBranches { get; set; }
    }


}
