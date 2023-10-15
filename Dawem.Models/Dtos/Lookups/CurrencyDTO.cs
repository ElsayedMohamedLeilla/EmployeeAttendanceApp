using Dawem.Domain.Entities.Lookups;

namespace Dawem.Models.Dtos.Lookups
{
    public class CurrencyDTO
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
    }
}
