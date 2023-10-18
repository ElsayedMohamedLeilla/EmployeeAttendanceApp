using Dawem.Domain.Entities.Lookups;
using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Provider
{
    [Table(nameof(Branch) + DawemKeys.ES)]
    public class Branch : BaseEntity
    {
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public string Name { get; set; }
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
        public int AdminUserId { get; set; }
        public List<UserBranch> UserBranches { get; set; }
    }
}
