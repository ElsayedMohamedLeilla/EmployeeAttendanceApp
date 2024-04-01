using System.Globalization;

namespace Dawem.Models.DTOs.Dawem.Generic
{
    public class BasicCountryInfo
    {
        public string TimeZoneId { get; set; }
        public string CultureName { get; set; }
        public int CountryId { get; set; }
        public CultureInfo CurrentCulture { get; set; }
    }
}
