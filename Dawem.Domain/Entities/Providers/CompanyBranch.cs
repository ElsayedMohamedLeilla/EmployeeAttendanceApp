using Dawem.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Providers
{
    [Table(nameof(CompanyBranch) + LeillaKeys.ES)]
    public class CompanyBranch : BaseEntity
    {
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string PostalCode { get; set; }
    }
}
