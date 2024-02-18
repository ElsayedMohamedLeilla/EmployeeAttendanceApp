using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Lookups
{
    [Table("Countries")]
    public class Country : BaseEntity
    {
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public string Dial { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }
        public string TimeZoneId { get; set; }
        public int Order { get; set; }
        public string NationalityNameEn { get; set; }
        public string NationalityNameAr { get; set; }
    }
}
