using System.ComponentModel.DataAnnotations.Schema;
using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Provider;
using Microsoft.AspNetCore.Identity;
using SmartBusinessERP.Domain;
using SmartBusinessERP.Enums;

namespace Dawem.Domain.Entities.UserManagement;

[Table(nameof(User) + "s")]
public class User : IdentityUser<int>, IBaseEntity
{
    // public string verificationCode { get; set; }

    public DateTime AddedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? AddUserId { get; set; }
    public int? ModifyUserId { get; set; }

    public int? MainBranchId { get; set; }

    public bool IsAdmin { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Gender Gender { get; set; }
    public string? MobileNumber { get; set; }

    [NotMapped]
    public List<UserRole> UserRols { get; set; }
    public List<UserBranch?>? UserBranches { get; set; }
    public List<UserGroup?>? UserGroups { get; set; }
}

