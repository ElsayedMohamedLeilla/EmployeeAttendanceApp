using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Employees;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Providers;
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

        public int? EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }

        public int? MobileCountryId { get; set; }
        [ForeignKey(nameof(MobileCountryId))]
        public Country MobileCountry { get; set; }

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
        public List<UserResponsibility> UserResponsibilities { get; set; }
        public List<UserBranch> UserBranches { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public string DisableReason { get; set; }
        public ApplicationType AddedApplicationType { get; set; }
        public ApplicationType? ModifiedApplicationType { get; set; }
        public string VerificationCode { get; set; }
        public DateTime VerificationCodeSendDate { get; set; }
        public AuthenticationType Type { get; set; }
        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
    }
}