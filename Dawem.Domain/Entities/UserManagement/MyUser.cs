using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Provider;
using Dawem.Enums.Generals;
using Dawem.Translations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.UserManagement
{
    [Table(nameof(MyUser) + LeillaKeys.S)]
    public class MyUser : IdentityUser<int>, IBaseEntity
    {
        #region Forign Keys

        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }
        public int? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        #endregion
        public int Code { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AddUserId { get; set; }
        public int? ModifyUserId { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public string MobileNumber { get; set; }
        public string ProfileImageName { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<UserBranch> UserBranches { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public string DisableReason { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
    }
}