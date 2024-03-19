namespace Dawem.Models.Dtos.Lookups
{
    public class CountryLiteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryISOCode { get; set; }
        public string Dial { get; set; }
        public bool? IsCurrentCountry { get; set; }
        public int PhoneLength { get; set; }
        public string FlagPath { get; set; }
    }
}
