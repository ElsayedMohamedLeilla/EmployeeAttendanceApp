using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Lookups
{
    [Table("Currencies")]
    public class Currency : BaseEntity
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public int? CountryId { get; set; }

        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

    }
}
