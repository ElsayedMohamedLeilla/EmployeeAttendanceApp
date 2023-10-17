using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.General;
using Dawem.Translations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.UserManagement
{
    [Table(nameof(MyUser) + DawemKeys.S)]
    public class MyUser : IdentityUser<int>, IBaseEntity
    {
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public int? MainBranchId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string MobileNumber { get; set; }
        [NotMapped]
        public List<UserRole> UserRols { get; set; }
        public List<UserBranch> UserBranches { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public int? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }
        public bool Status { get; set; }
        public DateTime? DeletionDate { get; set; }
    }
}