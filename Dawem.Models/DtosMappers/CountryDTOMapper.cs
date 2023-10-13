using Dawem.Domain.Entities.Lookups;
using Dawem.Models.Context;
using Dawem.Models.Dtos.Lookups;

namespace Dawem.Models.DtosMappers
{
    public class CountryDTOMapper
    {
        private static RequestHeaderContext? userContext;
        public static void InitBranchContext(RequestHeaderContext _userContext)
        {
            userContext = _userContext;
        }
        public static CountryDTO Map(Country country)
        {
            if (country == null) return null;
            var DTO = new CountryDTO()
            {

                Name = country.NameAr,

                Id = country.Id

            };
            return DTO;
        }


        public static List<CountryDTO> Map(List<Country> country)
        {
            if (country == null) return null;
            return country.Select(x => Map(x)).ToList();
        }

        public static List<Country> Map(List<CountryDTO> country)
        {
            if (country == null) return null;
            return country.Select(x => Map(x)).ToList();
        }

        public static Country Map(CountryDTO country)
        {
            if (country == null) return null;
            var DTO = new Country()
            {
                NameAr = country.Name,

                Id = country.Id


            };
            return DTO;
        }

    }
}
