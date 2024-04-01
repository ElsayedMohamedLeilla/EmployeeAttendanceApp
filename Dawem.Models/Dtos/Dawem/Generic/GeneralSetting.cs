namespace Dawem.Models.DTOs.Dawem.Generic
{
    public class GeneralSetting
    {
        public int? CountryId { get; set; }
        public BasicCountryInfo SmartCulture { get; set; }

        public void SetCultureByCountry(int? countryId)
        {
            if (countryId > 0)
            {
                SmartCulture = Globals.CountryCultures.FirstOrDefault(a => a.CountryId == countryId);
            }
        }

    }
}
