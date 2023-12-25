using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dawem.Models.Dtos.Providers
{
    public class BranchDTO
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public CompanyDto Company { get; set; }
        public string BranchName { get; set; }
        public string BranchNameCulture { get; set; }
        public string GlobalName { get; set; }
        public bool IsMainBranch { get; set; }
        public string Address { get; set; }

        public string AddressCulture { get; set; }

        public string CityName { get; set; }
        public string PostCode { get; set; }
        public string PhoneNumber { get; set; }

        public string CountryGlobalName { get; set; }
        public string CurrencyGlobalName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }

        public string TaxRegistrationNumber { get; set; }

        public string CommercialRecordNumber { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }


        public int? AddUserId { get; set; }


        public int? ModifyUserId { get; set; }


        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public int AdminUserId { get; set; }

        public int CountryId { get; set; }
        public int CurrencyId { get; set; }

        public List<UserBranchDTO> UserBranches { get; set; }
        public int? PackageId { get; set; }
    }
}
