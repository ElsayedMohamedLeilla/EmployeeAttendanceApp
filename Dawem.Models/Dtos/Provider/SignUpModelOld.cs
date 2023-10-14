using Dawem.Translations;
using System.ComponentModel.DataAnnotations;

namespace Dawem.Models.Dtos.Provider
{



    public class ChangePasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int UserId { get; set; }

    }
    public class UserBranchModel
    {
        public string UserName { get; set; }
    }


    public class SignUpModelOld
    {
        #region CompanyData
        public string? BusinessName { get; set; }
        public int BusinessCountryId { get; set; }

        #endregion

        #region BranchData

        public string? BusinessAddress { get; set; }
        public int BusinessCurrencyId { get; set; }
        public int? PackageId { get; set; }
        public string? BusinessTaxRegistrationNumber { get; set; }
        public string? BusinessCommercialRecordNumber { get; set; }
        [Required]
        [Display(Name = DawemKeys.Email)]
        public string? BusinessEmail { get; set; }

        #endregion

        #region UserData

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string? UserEmail { get; set; }
        public virtual string? UserMobileNumber { get; set; }
        public virtual string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Agreed { get; set; }
        #endregion
    }

    public class SignUpResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public int BranchId { get; set; }
        public int CompanyId { get; set; }
        public string Token { get; set; }
    }
    public class ForgetPasswordModel
    {
        public string Email { get; set; }
    }
}
