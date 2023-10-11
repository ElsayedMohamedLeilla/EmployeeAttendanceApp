using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dawem.Domain.Entities.Lookups
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameEn { get; set; }

        public string NameAr { get; set; }
        public string Iso { get; set; }
        public string Iso3 { get; set; }
        public string Dial { get; set; }
        public string Currency { get; set; }
        public string CurrencyName { get; set; }



    }
}
