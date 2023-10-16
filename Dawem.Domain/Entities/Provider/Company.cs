using Dawem.Domain.Entities.Lookups;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Provider
{
    [Table("Companies")]
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public virtual List<Branch> Branches { get; set; }
        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

    }

}
