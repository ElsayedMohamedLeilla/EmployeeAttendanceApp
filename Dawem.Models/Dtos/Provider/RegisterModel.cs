using Dawem.Models.Response;
using System.ComponentModel.DataAnnotations;

namespace Dawem.Models.Dtos.Provider
{



    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        //ErrorMessageResourceType = typeof(ar), ErrorMessageResourceName  = "PasswordAndConfirmMatch")
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "PasswordAndConfirmMatch")]
        public string ConfirmPassword { get; set; }
        public int UserId { get; set; }

    }
    public class UserBranchModel
    {
        public string UserName { get; set; }
    }


    public class RegisterModel
    {
        #region CompanyData
        public string BusinessName { get; set; }
        //  public string BusinessNameCulture { get; set; }
        public int BusinessCountryId { get; set; }

        #endregion

        #region BranchData

        public string BusinessAddress { get; set; }


        public int BusinessCurrencyId { get; set; }

        public int? PackageId { get; set; }

        public string BusinessTaxRegistrationNumber { get; set; }

        public string BusinessCommercialRecordNumber { get; set; }


        [Required]
        [Display(Name = "Email")]
        public string BusinessEmail { get; set; }



        #endregion

        #region UserData


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        public virtual string? UserMobileNumber { get; set; }
        public virtual string? FirstName { get; set; }
        public string? LastName { get; set; }

        public bool Agreed { get; set; }
        #endregion



    }

    public class RegisterResponseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int branchId { get; set; }
        public int CompanyId { get; set; }
        public string Token { get; set; }

    }


    public class ForgetPasswordBindingModel
    {

        [Display(Name = "Email")]
        public string Email { get; set; }



    }


}
