using Dawem.Translations;
using System.ComponentModel.DataAnnotations;

namespace Dawem.Models.Dtos.Providers
{
    public class SignUpModelOld
    {
        #region CompanyData
        public string BusinessName { get; set; }
        public int BusinessCountryId { get; set; }

        #endregion

        #region BranchData

        public string BusinessAddress { get; set; }
        public int BusinessCurrencyId { get; set; }
        public int? PackageId { get; set; }
        public string BusinessTaxRegistrationNumber { get; set; }
        public string BusinessCommercialRecordNumber { get; set; }
        [Required]
        [Display(Name = LeillaKeys.Email)]
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
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string UserEmail { get; set; }
        public virtual string UserMobileNumber { get; set; }
        public virtual string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Agreed { get; set; }
        #endregion
    }
}
