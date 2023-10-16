using Dawem.Domain.Entities.Lookups;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Dawem.Domain.Entities.Provider
{
    [Table(nameof(Branch) + DawemKeys.ES)]
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public string BranchName { get; set; }

        public int? MainBranchId { get; set; }
        [ForeignKey(nameof(MainBranchId))]

        public Branch MainBranch { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsMainBranch { get; set; }
        public string Address { get; set; }


        public string Email { get; set; }



        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]

        public Country Country { get; set; }

     
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }


        public int? AddUserId { get; set; }


        public int? ModifyUserId { get; set; }


        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletionDate { get; set; }
        public int AdminUserId { get; set; }


        public List<UserBranch> UserBranches { get; set; }


        public void Delete()
        {
            IsDeleted = true;
            DeletionDate = DateTime.UtcNow;
        }
    }
}
