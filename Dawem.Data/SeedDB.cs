using Dawem.Domain.Entities.Core;
using Dawem.Domain.Entities.Dawem;
using Dawem.Domain.Entities.Lookups;
using Dawem.Domain.Entities.Subscriptions;
using Dawem.Enums.Generals;
using Dawem.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Dawem.Data
{
    public class timezone
    {
        public string country_code { get; set; }
        public decimal? time_zone { get; set; }
    }
    public class SeedDB
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDBContext>();
            context.Database.EnsureCreated();

            var rolesToSeed = new List<Domain.Entities.UserManagement.Role>
            {
                new() { Name = "FULLACCESS", NormalizedName = "FULLACCESS" , ConcurrencyStamp =null},
                new() { Name = "ADMIN", NormalizedName = "ADMIN", ConcurrencyStamp =null},
                new() { Name = "EMPLOYEE", NormalizedName = "EMPLOYEE", ConcurrencyStamp =null},
                new() { Name = "USER", NormalizedName = "USER", ConcurrencyStamp =null},
                new() { Name = "MANAGER", NormalizedName = "MANAGER", ConcurrencyStamp =null},
                new() { Name = "DEVELOPER", NormalizedName = "DEVELOPER", ConcurrencyStamp =null},
                new() { Name = "SUPPORT", NormalizedName = "SUPPORT", ConcurrencyStamp =null},
                new() { Name = "CUSTOMER", NormalizedName = "CUSTOMER", ConcurrencyStamp =null},
                new() { Name = "VIEWER", NormalizedName = "VIEWER", ConcurrencyStamp =null},
                new() { Name = "DOCTOR", NormalizedName = "DOCTOR", ConcurrencyStamp =null},
                new() { Name = "NURSE", NormalizedName = "NURSE", ConcurrencyStamp =null},
                new() { Name = "MEDICALASSISTANT", NormalizedName = "MEDICALASSISTANT", ConcurrencyStamp =null},
                new() { Name = "PHARMACIST", NormalizedName = "PHARMACIST", ConcurrencyStamp =null},
                new() { Name = "RECEPTIONIST", NormalizedName = "RECEPTIONIST", ConcurrencyStamp =null},
                new() { Name = "SECURITYOFFICER", NormalizedName = "SECURITYOFFICER", ConcurrencyStamp =null},
                new() { Name = "HOUSEKEEPING", NormalizedName = "HOUSEKEEPING", ConcurrencyStamp =null},
                new() { Name = "ITSUPPORT", NormalizedName = "ITSUPPORT", ConcurrencyStamp =null},

            };
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(rolesToSeed);
                context.SaveChanges();
            }

            if (context.Roles.Any() && !context.Responsibilities.Any())
            {
                var roles = context.Roles.ToList();
                var companies = context.Companies.ToList();
                var responsibilities = new List<Responsibility>();
                int getNextCode = 0;

                foreach (var company in companies)
                {
                    #region Set Employee code

                    getNextCode = context.Responsibilities
                        .Where(e => e.CompanyId == company.Id)
                        .Select(e => e.Code)
                        .DefaultIfEmpty()
                        .Max();

                    #endregion

                    responsibilities = new List<Responsibility>();

                    foreach (var role in roles)
                    {
                        getNextCode++;
                        responsibilities.Add(new Responsibility
                        {
                            Name = role.Name,
                            Code = getNextCode,
                            CompanyId = company.Id,
                            Type = AuthenticationType.DawemAdmin
                        });
                    }

                    context.Responsibilities.AddRange(responsibilities);
                    context.SaveChanges();
                }

                responsibilities = new List<Responsibility>();

                #region Set Employee code

                getNextCode = context.Responsibilities
                    .Where(e => e.CompanyId == null)
                    .Select(e => e.Code)
                    .DefaultIfEmpty()
                    .Max();

                #endregion

                foreach (var role in roles)
                {
                    getNextCode++;
                    responsibilities.Add(new Responsibility
                    {
                        Name = role.Name,
                        Code = getNextCode,
                        Type = AuthenticationType.AdminPanel
                    });
                }

                context.Responsibilities.AddRange(responsibilities);
                context.SaveChanges();


            }


            var allCountriesCount = context.Countries.Count();

            if (allCountriesCount <= 0)
            {
                string countriesData = "[{\"NameAr\":\"TempData\",\"NameEn\":\"Afghanistan\",\"iso\":\"AF\",\"iso3\":\"AFG\",\"dial\":93,\"currency\":\"AFN\",\"CurrencyName\":\"Afghani\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Albania\",\"iso\":\"AL\",\"iso3\":\"ALB\",\"dial\":355,\"currency\":\"ALL\",\"CurrencyName\":\"Lek\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Algeria\",\"iso\":\"DZ\",\"iso3\":\"DZA\",\"dial\":213,\"currency\":\"DZD\",\"CurrencyName\":\"AlgerianDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"AmericanSamoa\",\"iso\":\"AS\",\"iso3\":\"ASM\",\"dial\":\"1-684\",\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Andorra\",\"iso\":\"AD\",\"iso3\":\"AND\",\"dial\":376,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Angola\",\"iso\":\"AO\",\"iso3\":\"AGO\",\"dial\":244,\"currency\":\"AOA\",\"CurrencyName\":\"Kwanza\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Anguilla\",\"iso\":\"AI\",\"iso3\":\"AIA\",\"dial\":\"1-264\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Antarctica\",\"iso\":\"AQ\",\"iso3\":\"ATA\",\"dial\":672,\"currency\":\"NULL\",\"CurrencyName\":\"NULL\"},{\"NameAr\":\"TempData\",\"NameEn\":\"AntiguaandBarbuda\",\"iso\":\"AG\",\"iso3\":\"ATG\",\"dial\":\"1-268\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Argentina\",\"iso\":\"AR\",\"iso3\":\"ARG\",\"dial\":54,\"currency\":\"ARS\",\"CurrencyName\":\"ArgentinePeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Armenia\",\"iso\":\"AM\",\"iso3\":\"ARM\",\"dial\":374,\"currency\":\"AMD\",\"CurrencyName\":\"ArmenianDram\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Aruba\",\"iso\":\"AW\",\"iso3\":\"ABW\",\"dial\":297,\"currency\":\"AWG\",\"CurrencyName\":\"ArubanFlorin\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Australia\",\"iso\":\"AU\",\"iso3\":\"AUS\",\"dial\":61,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Austria\",\"iso\":\"AT\",\"iso3\":\"AUT\",\"dial\":43,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Azerbaijan\",\"iso\":\"AZ\",\"iso3\":\"AZE\",\"dial\":994,\"currency\":\"AZN\",\"CurrencyName\":\"AzerbaijanianManat\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bahamas\",\"iso\":\"BS\",\"iso3\":\"BHS\",\"dial\":\"1-242\",\"currency\":\"BSD\",\"CurrencyName\":\"BahamianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bahrain\",\"iso\":\"BH\",\"iso3\":\"BHR\",\"dial\":973,\"currency\":\"BHD\",\"CurrencyName\":\"BahrainiDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bangladesh\",\"iso\":\"BD\",\"iso3\":\"BGD\",\"dial\":880,\"currency\":\"BDT\",\"CurrencyName\":\"Taka\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Barbados\",\"iso\":\"BB\",\"iso3\":\"BRB\",\"dial\":\"1-246\",\"currency\":\"BBD\",\"CurrencyName\":\"BarbadosDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Belarus\",\"iso\":\"BY\",\"iso3\":\"BLR\",\"dial\":375,\"currency\":\"BYR\",\"CurrencyName\":\"BelarussianRuble\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Belgium\",\"iso\":\"BE\",\"iso3\":\"BEL\",\"dial\":32,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Belize\",\"iso\":\"BZ\",\"iso3\":\"BLZ\",\"dial\":501,\"currency\":\"BZD\",\"CurrencyName\":\"BelizeDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Benin\",\"iso\":\"BJ\",\"iso3\":\"BEN\",\"dial\":229,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bermuda\",\"iso\":\"BM\",\"iso3\":\"BMU\",\"dial\":\"1-441\",\"currency\":\"BMD\",\"CurrencyName\":\"BermudianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bhutan\",\"iso\":\"BT\",\"iso3\":\"BTN\",\"dial\":975,\"currency\":\"INR\",\"CurrencyName\":\"IndianRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bolivia,PlurinationalStateof\",\"iso\":\"BO\",\"iso3\":\"BOL\",\"dial\":591,\"currency\":\"BOB\",\"CurrencyName\":\"Boliviano\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bonaire,SintEustatiusandSaba\",\"iso\":\"BQ\",\"iso3\":\"BES\",\"dial\":599,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"BosniaandHerzegovina\",\"iso\":\"BA\",\"iso3\":\"BIH\",\"dial\":387,\"currency\":\"BAM\",\"CurrencyName\":\"ConvertibleMark\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Botswana\",\"iso\":\"BW\",\"iso3\":\"BWA\",\"dial\":267,\"currency\":\"BWP\",\"CurrencyName\":\"Pula\"},{\"NameAr\":\"TempData\",\"NameEn\":\"BouvetIsland\",\"iso\":\"BV\",\"iso3\":\"BVT\",\"dial\":47,\"currency\":\"NOK\",\"CurrencyName\":\"NorwegianKrone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Brazil\",\"iso\":\"BR\",\"iso3\":\"BRA\",\"dial\":55,\"currency\":\"BRL\",\"CurrencyName\":\"BrazilianReal\"},{\"NameAr\":\"TempData\",\"NameEn\":\"BritishIndianOceanTerritory\",\"iso\":\"IO\",\"iso3\":\"IOT\",\"dial\":246,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"BruneiDarussalam\",\"iso\":\"BN\",\"iso3\":\"BRN\",\"dial\":673,\"currency\":\"BND\",\"CurrencyName\":\"BruneiDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Bulgaria\",\"iso\":\"BG\",\"iso3\":\"BGR\",\"dial\":359,\"currency\":\"BGN\",\"CurrencyName\":\"BulgarianLev\"},{\"NameAr\":\"TempData\",\"NameEn\":\"BurkinaFaso\",\"iso\":\"BF\",\"iso3\":\"BFA\",\"dial\":226,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Burundi\",\"iso\":\"BI\",\"iso3\":\"BDI\",\"dial\":257,\"currency\":\"BIF\",\"CurrencyName\":\"BurundiFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Cambodia\",\"iso\":\"KH\",\"iso3\":\"KHM\",\"dial\":855,\"currency\":\"KHR\",\"CurrencyName\":\"Riel\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Cameroon\",\"iso\":\"CM\",\"iso3\":\"CMR\",\"dial\":237,\"currency\":\"XAF\",\"CurrencyName\":\"CFAFrancBEAC\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Canada\",\"iso\":\"CA\",\"iso3\":\"CAN\",\"dial\":1,\"currency\":\"CAD\",\"CurrencyName\":\"CanadianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"CapeVerde\",\"iso\":\"CV\",\"iso3\":\"CPV\",\"dial\":238,\"currency\":\"CVE\",\"CurrencyName\":\"CaboVerdeEscudo\"},{\"NameAr\":\"TempData\",\"NameEn\":\"CaymanIslands\",\"iso\":\"KY\",\"iso3\":\"CYM\",\"dial\":\"1-345\",\"currency\":\"KYD\",\"CurrencyName\":\"CaymanIslandsDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"CentralAfricanRepublic\",\"iso\":\"CF\",\"iso3\":\"CAF\",\"dial\":236,\"currency\":\"XAF\",\"CurrencyName\":\"CFAFrancBEAC\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Chad\",\"iso\":\"TD\",\"iso3\":\"TCD\",\"dial\":235,\"currency\":\"XAF\",\"CurrencyName\":\"CFAFrancBEAC\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Chile\",\"iso\":\"CL\",\"iso3\":\"CHL\",\"dial\":56,\"currency\":\"CLP\",\"CurrencyName\":\"ChileanPeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"China\",\"iso\":\"CN\",\"iso3\":\"CHN\",\"dial\":86,\"currency\":\"CNY\",\"CurrencyName\":\"YuanRenminbi\"},{\"NameAr\":\"TempData\",\"NameEn\":\"ChristmasIsland\",\"iso\":\"CX\",\"iso3\":\"CXR\",\"dial\":61,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Cocos(Keeling)Islands\",\"iso\":\"CC\",\"iso3\":\"CCK\",\"dial\":61,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Colombia\",\"iso\":\"CO\",\"iso3\":\"COL\",\"dial\":57,\"currency\":\"COP\",\"CurrencyName\":\"ColombianPeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Comoros\",\"iso\":\"KM\",\"iso3\":\"COM\",\"dial\":269,\"currency\":\"KMF\",\"CurrencyName\":\"ComoroFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Congo\",\"iso\":\"CG\",\"iso3\":\"COG\",\"dial\":242,\"currency\":\"XAF\",\"CurrencyName\":\"CFAFrancBEAC\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Congo,theDemocraticRepublicofthe\",\"iso\":\"CD\",\"iso3\":\"COD\",\"dial\":243,\"currency\":\"NULL\",\"CurrencyName\":\"NULL\"},{\"NameAr\":\"TempData\",\"NameEn\":\"CookIslands\",\"iso\":\"CK\",\"iso3\":\"COK\",\"dial\":682,\"currency\":\"NZD\",\"CurrencyName\":\"NewZealandDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"CostaRica\",\"iso\":\"CR\",\"iso3\":\"CRI\",\"dial\":506,\"currency\":\"CRC\",\"CurrencyName\":\"CostaRicanColon\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Croatia\",\"iso\":\"HR\",\"iso3\":\"HRV\",\"dial\":385,\"currency\":\"HRK\",\"CurrencyName\":\"CroatianKuna\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Cuba\",\"iso\":\"CU\",\"iso3\":\"CUB\",\"dial\":53,\"currency\":\"CUP\",\"CurrencyName\":\"CubanPeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Curaçao\",\"iso\":\"CW\",\"iso3\":\"CUW\",\"dial\":599,\"currency\":\"ANG\",\"CurrencyName\":\"NetherlandsAntilleanGuilder\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Cyprus\",\"iso\":\"CY\",\"iso3\":\"CYP\",\"dial\":357,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"CzechRepublic\",\"iso\":\"CZ\",\"iso3\":\"CZE\",\"dial\":420,\"currency\":\"CZK\",\"CurrencyName\":\"CzechKoruna\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Côted''Ivoire\",\"iso\":\"CI\",\"iso3\":\"CIV\",\"dial\":225,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Denmark\",\"iso\":\"DK\",\"iso3\":\"DNK\",\"dial\":45,\"currency\":\"DKK\",\"CurrencyName\":\"DanishKrone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Djibouti\",\"iso\":\"DJ\",\"iso3\":\"DJI\",\"dial\":253,\"currency\":\"DJF\",\"CurrencyName\":\"DjiboutiFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Dominica\",\"iso\":\"DM\",\"iso3\":\"DMA\",\"dial\":\"1-767\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"DominicanRepublic\",\"iso\":\"DO\",\"iso3\":\"DOM\",\"dial\":\"1-809\",\"currency\":\"DOP\",\"CurrencyName\":\"DominicanPeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Ecuador\",\"iso\":\"EC\",\"iso3\":\"ECU\",\"dial\":593,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Egypt\",\"iso\":\"EG\",\"iso3\":\"EGY\",\"dial\":20,\"currency\":\"EGP\",\"CurrencyName\":\"EgyptianPound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"ElSalvador\",\"iso\":\"SV\",\"iso3\":\"SLV\",\"dial\":503,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"EquatorialGuinea\",\"iso\":\"GQ\",\"iso3\":\"GNQ\",\"dial\":240,\"currency\":\"XAF\",\"CurrencyName\":\"CFAFrancBEAC\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Eritrea\",\"iso\":\"ER\",\"iso3\":\"ERI\",\"dial\":291,\"currency\":\"ERN\",\"CurrencyName\":\"Nakfa\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Estonia\",\"iso\":\"EE\",\"iso3\":\"EST\",\"dial\":372,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Ethiopia\",\"iso\":\"ET\",\"iso3\":\"ETH\",\"dial\":251,\"currency\":\"ETB\",\"CurrencyName\":\"EthiopianBirr\"},{\"NameAr\":\"TempData\",\"NameEn\":\"FalklandIslands(Malvinas)\",\"iso\":\"FK\",\"iso3\":\"FLK\",\"dial\":500,\"currency\":\"FKP\",\"CurrencyName\":\"FalklandIslandsPound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"FaroeIslands\",\"iso\":\"FO\",\"iso3\":\"FRO\",\"dial\":298,\"currency\":\"DKK\",\"CurrencyName\":\"DanishKrone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Fiji\",\"iso\":\"FJ\",\"iso3\":\"FJI\",\"dial\":679,\"currency\":\"FJD\",\"CurrencyName\":\"FijiDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Finland\",\"iso\":\"FI\",\"iso3\":\"FIN\",\"dial\":358,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"France\",\"iso\":\"FR\",\"iso3\":\"FRA\",\"dial\":33,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"FrenchGuiana\",\"iso\":\"GF\",\"iso3\":\"GUF\",\"dial\":594,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"FrenchPolynesia\",\"iso\":\"PF\",\"iso3\":\"PYF\",\"dial\":689,\"currency\":\"XPF\",\"CurrencyName\":\"CFPFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"FrenchSouthernTerritories\",\"iso\":\"TF\",\"iso3\":\"ATF\",\"dial\":262,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Gabon\",\"iso\":\"GA\",\"iso3\":\"GAB\",\"dial\":241,\"currency\":\"XAF\",\"CurrencyName\":\"CFAFrancBEAC\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Gambia\",\"iso\":\"GM\",\"iso3\":\"GMB\",\"dial\":220,\"currency\":\"GMD\",\"CurrencyName\":\"Dalasi\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Georgia\",\"iso\":\"GE\",\"iso3\":\"GEO\",\"dial\":995,\"currency\":\"GEL\",\"CurrencyName\":\"Lari\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Germany\",\"iso\":\"DE\",\"iso3\":\"DEU\",\"dial\":49,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Ghana\",\"iso\":\"GH\",\"iso3\":\"GHA\",\"dial\":233,\"currency\":\"GHS\",\"CurrencyName\":\"GhanaCedi\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Gibraltar\",\"iso\":\"GI\",\"iso3\":\"GIB\",\"dial\":350,\"currency\":\"GIP\",\"CurrencyName\":\"GibraltarPound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Greece\",\"iso\":\"GR\",\"iso3\":\"GRC\",\"dial\":30,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Greenland\",\"iso\":\"GL\",\"iso3\":\"GRL\",\"dial\":299,\"currency\":\"DKK\",\"CurrencyName\":\"DanishKrone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Grenada\",\"iso\":\"GD\",\"iso3\":\"GRD\",\"dial\":\"1-473\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guadeloupe\",\"iso\":\"GP\",\"iso3\":\"GLP\",\"dial\":590,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guam\",\"iso\":\"GU\",\"iso3\":\"GUM\",\"dial\":\"1-671\",\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guatemala\",\"iso\":\"GT\",\"iso3\":\"GTM\",\"dial\":502,\"currency\":\"GTQ\",\"CurrencyName\":\"Quetzal\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guernsey\",\"iso\":\"GG\",\"iso3\":\"GGY\",\"dial\":44,\"currency\":\"GBP\",\"CurrencyName\":\"PoundSterling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guinea\",\"iso\":\"GN\",\"iso3\":\"GIN\",\"dial\":224,\"currency\":\"GNF\",\"CurrencyName\":\"GuineaFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guinea-Bissau\",\"iso\":\"GW\",\"iso3\":\"GNB\",\"dial\":245,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Guyana\",\"iso\":\"GY\",\"iso3\":\"GUY\",\"dial\":592,\"currency\":\"GYD\",\"CurrencyName\":\"GuyanaDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Haiti\",\"iso\":\"HT\",\"iso3\":\"HTI\",\"dial\":509,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"HeardIslandandMcDonaldIslands\",\"iso\":\"HM\",\"iso3\":\"HMD\",\"dial\":672,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"HolySee(VaticanCityState)\",\"iso\":\"VA\",\"iso3\":\"VAT\",\"dial\":\"39-06\",\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Honduras\",\"iso\":\"HN\",\"iso3\":\"HND\",\"dial\":504,\"currency\":\"HNL\",\"CurrencyName\":\"Lempira\"},{\"NameAr\":\"TempData\",\"NameEn\":\"HongKong\",\"iso\":\"HK\",\"iso3\":\"HKG\",\"dial\":852,\"currency\":\"HKD\",\"CurrencyName\":\"HongKongDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Hungary\",\"iso\":\"HU\",\"iso3\":\"HUN\",\"dial\":36,\"currency\":\"HUF\",\"CurrencyName\":\"Forint\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Iceland\",\"iso\":\"IS\",\"iso3\":\"ISL\",\"dial\":354,\"currency\":\"ISK\",\"CurrencyName\":\"IcelandKrona\"},{\"NameAr\":\"TempData\",\"NameEn\":\"India\",\"iso\":\"IN\",\"iso3\":\"IND\",\"dial\":91,\"currency\":\"INR\",\"CurrencyName\":\"IndianRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Indonesia\",\"iso\":\"ID\",\"iso3\":\"IDN\",\"dial\":62,\"currency\":\"IDR\",\"CurrencyName\":\"Rupiah\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Iran,IslamicRepublicof\",\"iso\":\"IR\",\"iso3\":\"IRN\",\"dial\":98,\"currency\":\"IRR\",\"CurrencyName\":\"IranianRial\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Iraq\",\"iso\":\"IQ\",\"iso3\":\"IRQ\",\"dial\":964,\"currency\":\"IQD\",\"CurrencyName\":\"IraqiDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Ireland\",\"iso\":\"IE\",\"iso3\":\"IRL\",\"dial\":353,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"IsleofMan\",\"iso\":\"IM\",\"iso3\":\"IMN\",\"dial\":44,\"currency\":\"GBP\",\"CurrencyName\":\"PoundSterling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Israel\",\"iso\":\"IL\",\"iso3\":\"ISR\",\"dial\":972,\"currency\":\"ILS\",\"CurrencyName\":\"NewIsraeliSheqel\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Italy\",\"iso\":\"IT\",\"iso3\":\"ITA\",\"dial\":39,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Jamaica\",\"iso\":\"JM\",\"iso3\":\"JAM\",\"dial\":\"1-876\",\"currency\":\"JMD\",\"CurrencyName\":\"JamaicanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Japan\",\"iso\":\"JP\",\"iso3\":\"JPN\",\"dial\":81,\"currency\":\"JPY\",\"CurrencyName\":\"Yen\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Jersey\",\"iso\":\"JE\",\"iso3\":\"JEY\",\"dial\":44,\"currency\":\"GBP\",\"CurrencyName\":\"PoundSterling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Jordan\",\"iso\":\"JO\",\"iso3\":\"JOR\",\"dial\":962,\"currency\":\"JOD\",\"CurrencyName\":\"JordanianDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Kazakhstan\",\"iso\":\"KZ\",\"iso3\":\"KAZ\",\"dial\":7,\"currency\":\"KZT\",\"CurrencyName\":\"Tenge\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Kenya\",\"iso\":\"KE\",\"iso3\":\"KEN\",\"dial\":254,\"currency\":\"KES\",\"CurrencyName\":\"KenyanShilling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Kiribati\",\"iso\":\"KI\",\"iso3\":\"KIR\",\"dial\":686,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Korea,DemocraticPeople''sRepublicof\",\"iso\":\"KP\",\"iso3\":\"PRK\",\"dial\":850,\"currency\":\"KPW\",\"CurrencyName\":\"NorthKoreanWon\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Korea,Republicof\",\"iso\":\"KR\",\"iso3\":\"KOR\",\"dial\":82,\"currency\":\"KRW\",\"CurrencyName\":\"Won\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Kuwait\",\"iso\":\"KW\",\"iso3\":\"KWT\",\"dial\":965,\"currency\":\"KWD\",\"CurrencyName\":\"KuwaitiDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Kyrgyzstan\",\"iso\":\"KG\",\"iso3\":\"KGZ\",\"dial\":996,\"currency\":\"KGS\",\"CurrencyName\":\"Som\"},{\"NameAr\":\"TempData\",\"NameEn\":\"LaoPeople''sDemocraticRepublic\",\"iso\":\"LA\",\"iso3\":\"LAO\",\"dial\":856,\"currency\":\"LAK\",\"CurrencyName\":\"Kip\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Latvia\",\"iso\":\"LV\",\"iso3\":\"LVA\",\"dial\":371,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Lebanon\",\"iso\":\"LB\",\"iso3\":\"LBN\",\"dial\":961,\"currency\":\"LBP\",\"CurrencyName\":\"LebanesePound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Lesotho\",\"iso\":\"LS\",\"iso3\":\"LSO\",\"dial\":266,\"currency\":\"ZAR\",\"CurrencyName\":\"Rand\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Liberia\",\"iso\":\"LR\",\"iso3\":\"LBR\",\"dial\":231,\"currency\":\"LRD\",\"CurrencyName\":\"LiberianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Libya\",\"iso\":\"LY\",\"iso3\":\"LBY\",\"dial\":218,\"currency\":\"LYD\",\"CurrencyName\":\"LibyanDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Liechtenstein\",\"iso\":\"LI\",\"iso3\":\"LIE\",\"dial\":423,\"currency\":\"CHF\",\"CurrencyName\":\"SwissFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Lithuania\",\"iso\":\"LT\",\"iso3\":\"LTU\",\"dial\":370,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Luxembourg\",\"iso\":\"LU\",\"iso3\":\"LUX\",\"dial\":352,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Macao\",\"iso\":\"MO\",\"iso3\":\"MAC\",\"dial\":853,\"currency\":\"MOP\",\"CurrencyName\":\"Pataca\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Macedonia,theFormerYugoslavRepublicof\",\"iso\":\"MK\",\"iso3\":\"MKD\",\"dial\":389,\"currency\":\"MKD\",\"CurrencyName\":\"Denar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Madagascar\",\"iso\":\"MG\",\"iso3\":\"MDG\",\"dial\":261,\"currency\":\"MGA\",\"CurrencyName\":\"MalagasyAriary\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Malawi\",\"iso\":\"MW\",\"iso3\":\"MWI\",\"dial\":265,\"currency\":\"MWK\",\"CurrencyName\":\"Kwacha\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Malaysia\",\"iso\":\"MY\",\"iso3\":\"MYS\",\"dial\":60,\"currency\":\"MYR\",\"CurrencyName\":\"MalaysianRinggit\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Maldives\",\"iso\":\"MV\",\"iso3\":\"MDV\",\"dial\":960,\"currency\":\"MVR\",\"CurrencyName\":\"Rufiyaa\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mali\",\"iso\":\"ML\",\"iso3\":\"MLI\",\"dial\":223,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Malta\",\"iso\":\"MT\",\"iso3\":\"MLT\",\"dial\":356,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"MarshallIslands\",\"iso\":\"MH\",\"iso3\":\"MHL\",\"dial\":692,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Martinique\",\"iso\":\"MQ\",\"iso3\":\"MTQ\",\"dial\":596,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mauritania\",\"iso\":\"MR\",\"iso3\":\"MRT\",\"dial\":222,\"currency\":\"MRO\",\"CurrencyName\":\"Ouguiya\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mauritius\",\"iso\":\"MU\",\"iso3\":\"MUS\",\"dial\":230,\"currency\":\"MUR\",\"CurrencyName\":\"MauritiusRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mayotte\",\"iso\":\"YT\",\"iso3\":\"MYT\",\"dial\":262,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mexico\",\"iso\":\"MX\",\"iso3\":\"MEX\",\"dial\":52,\"currency\":\"MXN\",\"CurrencyName\":\"MexicanPeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Micronesia,FederatedStatesof\",\"iso\":\"FM\",\"iso3\":\"FSM\",\"dial\":691,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Moldova,Republicof\",\"iso\":\"MD\",\"iso3\":\"MDA\",\"dial\":373,\"currency\":\"MDL\",\"CurrencyName\":\"MoldovanLeu\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Monaco\",\"iso\":\"MC\",\"iso3\":\"MCO\",\"dial\":377,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mongolia\",\"iso\":\"MN\",\"iso3\":\"MNG\",\"dial\":976,\"currency\":\"MNT\",\"CurrencyName\":\"Tugrik\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Montenegro\",\"iso\":\"ME\",\"iso3\":\"MNE\",\"dial\":382,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Montserrat\",\"iso\":\"MS\",\"iso3\":\"MSR\",\"dial\":\"1-664\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Morocco\",\"iso\":\"MA\",\"iso3\":\"MAR\",\"dial\":212,\"currency\":\"MAD\",\"CurrencyName\":\"MoroccanDirham\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Mozambique\",\"iso\":\"MZ\",\"iso3\":\"MOZ\",\"dial\":258,\"currency\":\"MZN\",\"CurrencyName\":\"MozambiqueMetical\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Myanmar\",\"iso\":\"MM\",\"iso3\":\"MMR\",\"dial\":95,\"currency\":\"MMK\",\"CurrencyName\":\"Kyat\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Namibia\",\"iso\":\"NA\",\"iso3\":\"NAM\",\"dial\":264,\"currency\":\"ZAR\",\"CurrencyName\":\"Rand\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Nauru\",\"iso\":\"NR\",\"iso3\":\"NRU\",\"dial\":674,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Nepal\",\"iso\":\"NP\",\"iso3\":\"NPL\",\"dial\":977,\"currency\":\"NPR\",\"CurrencyName\":\"NepaleseRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Netherlands\",\"iso\":\"NL\",\"iso3\":\"NLD\",\"dial\":31,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"NewCaledonia\",\"iso\":\"NC\",\"iso3\":\"NCL\",\"dial\":687,\"currency\":\"XPF\",\"CurrencyName\":\"CFPFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"NewZealand\",\"iso\":\"NZ\",\"iso3\":\"NZL\",\"dial\":64,\"currency\":\"NZD\",\"CurrencyName\":\"NewZealandDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Nicaragua\",\"iso\":\"NI\",\"iso3\":\"NIC\",\"dial\":505,\"currency\":\"NIO\",\"CurrencyName\":\"CordobaOro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Niger\",\"iso\":\"NE\",\"iso3\":\"NER\",\"dial\":227,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Nigeria\",\"iso\":\"NG\",\"iso3\":\"NGA\",\"dial\":234,\"currency\":\"NGN\",\"CurrencyName\":\"Naira\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Niue\",\"iso\":\"NU\",\"iso3\":\"NIU\",\"dial\":683,\"currency\":\"NZD\",\"CurrencyName\":\"NewZealandDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"NorfolkIsland\",\"iso\":\"NF\",\"iso3\":\"NFK\",\"dial\":672,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"NorthernMarianaIslands\",\"iso\":\"MP\",\"iso3\":\"MNP\",\"dial\":\"1-670\",\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Norway\",\"iso\":\"NO\",\"iso3\":\"NOR\",\"dial\":47,\"currency\":\"NOK\",\"CurrencyName\":\"NorwegianKrone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Oman\",\"iso\":\"OM\",\"iso3\":\"OMN\",\"dial\":968,\"currency\":\"OMR\",\"CurrencyName\":\"RialOmani\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Pakistan\",\"iso\":\"PK\",\"iso3\":\"PAK\",\"dial\":92,\"currency\":\"PKR\",\"CurrencyName\":\"PakistanRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Palau\",\"iso\":\"PW\",\"iso3\":\"PLW\",\"dial\":680,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Palestine,Stateof\",\"iso\":\"PS\",\"iso3\":\"PSE\",\"dial\":970,\"currency\":\"NULL\",\"CurrencyName\":\"NULL\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Panama\",\"iso\":\"PA\",\"iso3\":\"PAN\",\"dial\":507,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"PapuaNewGuinea\",\"iso\":\"PG\",\"iso3\":\"PNG\",\"dial\":675,\"currency\":\"PGK\",\"CurrencyName\":\"Kina\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Paraguay\",\"iso\":\"PY\",\"iso3\":\"PRY\",\"dial\":595,\"currency\":\"PYG\",\"CurrencyName\":\"Guarani\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Peru\",\"iso\":\"PE\",\"iso3\":\"PER\",\"dial\":51,\"currency\":\"PEN\",\"CurrencyName\":\"NuevoSol\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Philippines\",\"iso\":\"PH\",\"iso3\":\"PHL\",\"dial\":63,\"currency\":\"PHP\",\"CurrencyName\":\"PhilippinePeso\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Pitcairn\",\"iso\":\"PN\",\"iso3\":\"PCN\",\"dial\":870,\"currency\":\"NZD\",\"CurrencyName\":\"NewZealandDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Poland\",\"iso\":\"PL\",\"iso3\":\"POL\",\"dial\":48,\"currency\":\"PLN\",\"CurrencyName\":\"Zloty\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Portugal\",\"iso\":\"PT\",\"iso3\":\"PRT\",\"dial\":351,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"PuertoRico\",\"iso\":\"PR\",\"iso3\":\"PRI\",\"dial\":1,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Qatar\",\"iso\":\"QA\",\"iso3\":\"QAT\",\"dial\":974,\"currency\":\"QAR\",\"CurrencyName\":\"QatariRial\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Romania\",\"iso\":\"RO\",\"iso3\":\"ROU\",\"dial\":40,\"currency\":\"RON\",\"CurrencyName\":\"NewRomanianLeu\"},{\"NameAr\":\"TempData\",\"NameEn\":\"RussianFederation\",\"iso\":\"RU\",\"iso3\":\"RUS\",\"dial\":7,\"currency\":\"RUB\",\"CurrencyName\":\"RussianRuble\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Rwanda\",\"iso\":\"RW\",\"iso3\":\"RWA\",\"dial\":250,\"currency\":\"RWF\",\"CurrencyName\":\"RwandaFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Réunion\",\"iso\":\"RE\",\"iso3\":\"REU\",\"dial\":262,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintBarthélemy\",\"iso\":\"BL\",\"iso3\":\"BLM\",\"dial\":590,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintHelena,AscensionandTristandaCunha\",\"iso\":\"SH\",\"iso3\":\"SHN\",\"dial\":290,\"currency\":\"SHP\",\"CurrencyName\":\"SaintHelenaPound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintKittsandNevis\",\"iso\":\"KN\",\"iso3\":\"KNA\",\"dial\":\"1-869\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintLucia\",\"iso\":\"LC\",\"iso3\":\"LCA\",\"dial\":\"1-758\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintMartin(Frenchpart)\",\"iso\":\"MF\",\"iso3\":\"MAF\",\"dial\":590,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintPierreandMiquelon\",\"iso\":\"PM\",\"iso3\":\"SPM\",\"dial\":508,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaintVincentandtheGrenadines\",\"iso\":\"VC\",\"iso3\":\"VCT\",\"dial\":\"1-784\",\"currency\":\"XCD\",\"CurrencyName\":\"EastCaribbeanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Samoa\",\"iso\":\"WS\",\"iso3\":\"WSM\",\"dial\":685,\"currency\":\"WST\",\"CurrencyName\":\"Tala\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SanMarino\",\"iso\":\"SM\",\"iso3\":\"SMR\",\"dial\":378,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaoTomeandPrincipe\",\"iso\":\"ST\",\"iso3\":\"STP\",\"dial\":239,\"currency\":\"STD\",\"CurrencyName\":\"Dobra\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SaudiArabia\",\"iso\":\"SA\",\"iso3\":\"SAU\",\"dial\":966,\"currency\":\"SAR\",\"CurrencyName\":\"SaudiRiyal\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Senegal\",\"iso\":\"SN\",\"iso3\":\"SEN\",\"dial\":221,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Serbia\",\"iso\":\"RS\",\"iso3\":\"SRB\",\"dial\":381,\"currency\":\"RSD\",\"CurrencyName\":\"SerbianDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Seychelles\",\"iso\":\"SC\",\"iso3\":\"SYC\",\"dial\":248,\"currency\":\"SCR\",\"CurrencyName\":\"SeychellesRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SierraLeone\",\"iso\":\"SL\",\"iso3\":\"SLE\",\"dial\":232,\"currency\":\"SLL\",\"CurrencyName\":\"Leone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Singapore\",\"iso\":\"SG\",\"iso3\":\"SGP\",\"dial\":65,\"currency\":\"SGD\",\"CurrencyName\":\"SingaporeDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SintMaarten(Dutchpart)\",\"iso\":\"SX\",\"iso3\":\"SXM\",\"dial\":\"1-721\",\"currency\":\"ANG\",\"CurrencyName\":\"NetherlandsAntilleanGuilder\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Slovakia\",\"iso\":\"SK\",\"iso3\":\"SVK\",\"dial\":421,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Slovenia\",\"iso\":\"SI\",\"iso3\":\"SVN\",\"dial\":386,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SolomonIslands\",\"iso\":\"SB\",\"iso3\":\"SLB\",\"dial\":677,\"currency\":\"SBD\",\"CurrencyName\":\"SolomonIslandsDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Somalia\",\"iso\":\"SO\",\"iso3\":\"SOM\",\"dial\":252,\"currency\":\"SOS\",\"CurrencyName\":\"SomaliShilling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SouthAfrica\",\"iso\":\"ZA\",\"iso3\":\"ZAF\",\"dial\":27,\"currency\":\"ZAR\",\"CurrencyName\":\"Rand\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SouthGeorgiaandtheSouthSandwichIslands\",\"iso\":\"GS\",\"iso3\":\"SGS\",\"dial\":500,\"currency\":\"NULL\",\"CurrencyName\":\"NULL\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SouthSudan\",\"iso\":\"SS\",\"iso3\":\"SSD\",\"dial\":211,\"currency\":\"SSP\",\"CurrencyName\":\"SouthSudanesePound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Spain\",\"iso\":\"ES\",\"iso3\":\"ESP\",\"dial\":34,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SriLanka\",\"iso\":\"LK\",\"iso3\":\"LKA\",\"dial\":94,\"currency\":\"LKR\",\"CurrencyName\":\"SriLankaRupee\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Sudan\",\"iso\":\"SD\",\"iso3\":\"SDN\",\"dial\":249,\"currency\":\"SDG\",\"CurrencyName\":\"SudanesePound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SuriNameEn\",\"iso\":\"SR\",\"iso3\":\"SUR\",\"dial\":597,\"currency\":\"SRD\",\"CurrencyName\":\"SurinamDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SvalbardandJanMayen\",\"iso\":\"SJ\",\"iso3\":\"SJM\",\"dial\":47,\"currency\":\"NOK\",\"CurrencyName\":\"NorwegianKrone\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Swaziland\",\"iso\":\"SZ\",\"iso3\":\"SWZ\",\"dial\":268,\"currency\":\"SZL\",\"CurrencyName\":\"Lilangeni\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Sweden\",\"iso\":\"SE\",\"iso3\":\"SWE\",\"dial\":46,\"currency\":\"SEK\",\"CurrencyName\":\"SwedishKrona\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Switzerland\",\"iso\":\"CH\",\"iso3\":\"CHE\",\"dial\":41,\"currency\":\"CHF\",\"CurrencyName\":\"SwissFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"SyrianArabRepublic\",\"iso\":\"SY\",\"iso3\":\"SYR\",\"dial\":963,\"currency\":\"SYP\",\"CurrencyName\":\"SyrianPound\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Taiwan,ProvinceofChina\",\"iso\":\"TW\",\"iso3\":\"TWN\",\"dial\":886,\"currency\":\"TWD\",\"CurrencyName\":\"NewTaiwanDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Tajikistan\",\"iso\":\"TJ\",\"iso3\":\"TJK\",\"dial\":992,\"currency\":\"TJS\",\"CurrencyName\":\"Somoni\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Tanzania,UnitedRepublicof\",\"iso\":\"TZ\",\"iso3\":\"TZA\",\"dial\":255,\"currency\":\"TZS\",\"CurrencyName\":\"TanzanianShilling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Thailand\",\"iso\":\"TH\",\"iso3\":\"THA\",\"dial\":66,\"currency\":\"THB\",\"CurrencyName\":\"Baht\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Timor-Leste\",\"iso\":\"TL\",\"iso3\":\"TLS\",\"dial\":670,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Togo\",\"iso\":\"TG\",\"iso3\":\"TGO\",\"dial\":228,\"currency\":\"XOF\",\"CurrencyName\":\"CFAFrancBCEAO\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Tokelau\",\"iso\":\"TK\",\"iso3\":\"TKL\",\"dial\":690,\"currency\":\"NZD\",\"CurrencyName\":\"NewZealandDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Tonga\",\"iso\":\"TO\",\"iso3\":\"TON\",\"dial\":676,\"currency\":\"TOP\",\"CurrencyName\":\"Pa’anga\"},{\"NameAr\":\"TempData\",\"NameEn\":\"TrinidadandTobago\",\"iso\":\"TT\",\"iso3\":\"TTO\",\"dial\":\"1-868\",\"currency\":\"TTD\",\"CurrencyName\":\"TrinidadandTobagoDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Tunisia\",\"iso\":\"TN\",\"iso3\":\"TUN\",\"dial\":216,\"currency\":\"TND\",\"CurrencyName\":\"TunisianDinar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Turkey\",\"iso\":\"TR\",\"iso3\":\"TUR\",\"dial\":90,\"currency\":\"TRY\",\"CurrencyName\":\"TurkishLira\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Turkmenistan\",\"iso\":\"TM\",\"iso3\":\"TKM\",\"dial\":993,\"currency\":\"TMT\",\"CurrencyName\":\"TurkmenistanNewManat\"},{\"NameAr\":\"TempData\",\"NameEn\":\"TurksandCaicosIslands\",\"iso\":\"TC\",\"iso3\":\"TCA\",\"dial\":\"1-649\",\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Tuvalu\",\"iso\":\"TV\",\"iso3\":\"TUV\",\"dial\":688,\"currency\":\"AUD\",\"CurrencyName\":\"AustralianDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Uganda\",\"iso\":\"UG\",\"iso3\":\"UGA\",\"dial\":256,\"currency\":\"UGX\",\"CurrencyName\":\"UgandaShilling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Ukraine\",\"iso\":\"UA\",\"iso3\":\"UKR\",\"dial\":380,\"currency\":\"UAH\",\"CurrencyName\":\"Hryvnia\"},{\"NameAr\":\"TempData\",\"NameEn\":\"UnitedArabEmirates\",\"iso\":\"AE\",\"iso3\":\"ARE\",\"dial\":971,\"currency\":\"AED\",\"CurrencyName\":\"UAEDirham\"},{\"NameAr\":\"TempData\",\"NameEn\":\"UnitedKingdom\",\"iso\":\"GB\",\"iso3\":\"GBR\",\"dial\":44,\"currency\":\"GBP\",\"CurrencyName\":\"PoundSterling\"},{\"NameAr\":\"TempData\",\"NameEn\":\"UnitedStates\",\"iso\":\"US\",\"iso3\":\"USA\",\"dial\":1,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"UnitedStatesMinorOutlyingIslands\",\"iso\":\"UM\",\"iso3\":\"UMI\",\"dial\":1,\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Uruguay\",\"iso\":\"UY\",\"iso3\":\"URY\",\"dial\":598,\"currency\":\"UYU\",\"CurrencyName\":\"PesoUruguayo\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Uzbekistan\",\"iso\":\"UZ\",\"iso3\":\"UZB\",\"dial\":998,\"currency\":\"UZS\",\"CurrencyName\":\"UzbekistanSum\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Vanuatu\",\"iso\":\"VU\",\"iso3\":\"VUT\",\"dial\":678,\"currency\":\"VUV\",\"CurrencyName\":\"Vatu\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Venezuela,BolivarianRepublicof\",\"iso\":\"VE\",\"iso3\":\"VEN\",\"dial\":58,\"currency\":\"VEF\",\"CurrencyName\":\"Bolivar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"VietNam\",\"iso\":\"VN\",\"iso3\":\"VNM\",\"dial\":84,\"currency\":\"VND\",\"CurrencyName\":\"Dong\"},{\"NameAr\":\"TempData\",\"NameEn\":\"VirginIslands,British\",\"iso\":\"VG\",\"iso3\":\"VGB\",\"dial\":\"1-284\",\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"VirginIslands,U.S.\",\"iso\":\"VI\",\"iso3\":\"VIR\",\"dial\":\"1-340\",\"currency\":\"USD\",\"CurrencyName\":\"USDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"WallisandFutuna\",\"iso\":\"WF\",\"iso3\":\"WLF\",\"dial\":681,\"currency\":\"XPF\",\"CurrencyName\":\"CFPFranc\"},{\"NameAr\":\"TempData\",\"NameEn\":\"WesternSahara\",\"iso\":\"EH\",\"iso3\":\"ESH\",\"dial\":212,\"currency\":\"MAD\",\"CurrencyName\":\"MoroccanDirham\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Yemen\",\"iso\":\"YE\",\"iso3\":\"YEM\",\"dial\":967,\"currency\":\"YER\",\"CurrencyName\":\"YemeniRial\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Zambia\",\"iso\":\"ZM\",\"iso3\":\"ZMB\",\"dial\":260,\"currency\":\"ZMW\",\"CurrencyName\":\"ZambianKwacha\"},{\"NameAr\":\"TempData\",\"NameEn\":\"Zimbabwe\",\"iso\":\"ZW\",\"iso3\":\"ZWE\",\"dial\":263,\"currency\":\"ZWL\",\"CurrencyName\":\"ZimbabweDollar\"},{\"NameAr\":\"TempData\",\"NameEn\":\"ÅlandIslands\",\"iso\":\"AX\",\"iso3\":\"ALA\",\"dial\":358,\"currency\":\"EUR\",\"CurrencyName\":\"Euro\"}]";

                var countries = JsonConvert.DeserializeObject<List<Country>>(countriesData);

                context.Countries.AddRange(countries);
                context.SaveChanges();
            }

            #region Handle Languages

            var languages = new List<Language>();

            if (!context.Languages.Any())
            {
                languages.AddRange(new List<Language>()
                {   new () { Name = "Arabic", NativeName = "العربية", ISO2 = "ar" , ISO3="ara" , IsActive = true, Order = 1},
                    new () { Name = "English", NativeName = "English", ISO2 = "en" , ISO3="eng" , IsActive = true, Order = 2},
                    new () { Name = "Chinese", NativeName = "汉语", ISO2 = "zh" , ISO3="zho" , IsActive = false, Order = 6},
                    new () { Name = "Spanish", NativeName = "Español", ISO2 = "es" , ISO3="spa" , IsActive = false, Order = 7},
                    new () { Name = "Russian", NativeName = "Русский", ISO2 = "ru" , ISO3="rus", IsActive = false, Order = 8 },
                    new () { Name = "Hindi", NativeName = "हिन्दी", ISO2 = "hi" , ISO3="hin", IsActive = false , Order = 3},
                    new () { Name = "Japanese", NativeName = "日本語", ISO2 = "ja" , ISO3="jpn", IsActive = false , Order = 9},
                    new () { Name = "German", NativeName = "Deutsch", ISO2 = "de" , ISO3="due" , IsActive = false, Order = 10},
                    new () { Name = "French", NativeName = "Français", ISO2 = "fr" , ISO3="fra" , IsActive = false, Order = 11},
                    new () { Name = "Portuguese", NativeName = "Português", ISO2 = "pt" , ISO3="por" , IsActive = false, Order = 12},
                    new () { Name = "Korean", NativeName = "한국어", ISO2 = "ko" , ISO3="kor" , IsActive = false, Order = 13},
                    new () { Name = "Turkish", NativeName = "Türkçe", ISO2 = "tr" , ISO3="tur", IsActive = false , Order = 4},
                    new () { Name = "Italian", NativeName = "Italiano", ISO2 = "it" , ISO3="ita" , IsActive = false, Order = 14},
                    new () { Name = "Indonesian", NativeName = "Bahasa Indonesia", ISO2 = "id" , ISO3="ind" , IsActive = false, Order = 5},
                    new () { Name = "Bengali", NativeName = "বাংলা", ISO2 = "bn" , ISO3="ben" , IsActive = false, Order = 15},
                    new () { Name = "Punjabi", NativeName = "ਪੰਜਾਬੀ", ISO2 = "pa" , ISO3="pan" , IsActive = false, Order = 16}
                });

                context.Languages.AddRange(languages);
                context.SaveChanges();
            }

            #endregion

            #region Handle Plans

            var allPlansCount = context.Plans.Count();

            if (allPlansCount <= 0)
            {
                var plans = new List<Plan>();
                var code = 0;

                plans.Add(new()
                {
                    Code = code++,
                    EmployeeCost = 0,
                    IsTrial = true,
                    MinNumberOfEmployees = 1,
                    MaxNumberOfEmployees = 2,
                    PlanNameTranslations = new()
                    {
                        new ()
                        {
                            LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "ar").Id,
                            Name ="التجريبية"
                        },
                         new ()
                        {
                             LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "en").Id,
                             Name ="Trial"
                        }
                    }
                });

                plans.Add(new()
                {
                    Code = code++,
                    EmployeeCost = 5,
                    MinNumberOfEmployees = 1,
                    MaxNumberOfEmployees = 100,
                    PlanNameTranslations = new()
                    {
                        new ()
                        {
                            LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "ar").Id,
                            Name ="الاساسية"
                        },
                         new ()
                        {
                             LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "en").Id,
                             Name ="Basic"
                        }
                    }
                });

                plans.Add(new()
                {
                    Code = code++,
                    EmployeeCost = 4,
                    MinNumberOfEmployees = 101,
                    MaxNumberOfEmployees = 500,
                    PlanNameTranslations = new()
                    {
                        new ()
                        {
                            LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "ar").Id,
                            Name ="المتوسطة"
                        },
                         new ()
                        {
                             LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "en").Id,
                             Name ="Medium"
                        }
                    }
                });

                plans.Add(new()
                {
                    Code = code++,
                    EmployeeCost = 3,
                    MinNumberOfEmployees = 501,
                    MaxNumberOfEmployees = 1000,
                    PlanNameTranslations = new()
                    {
                        new ()
                        {
                            LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "ar").Id,
                            Name ="المتقدمة"
                        },
                         new ()
                        {
                             LanguageId = context.Languages.FirstOrDefault(l=>l.ISO2 == "en").Id,
                             Name ="Advanced"
                        }
                    }

                });

                context.Plans.AddRange(plans);
                context.SaveChanges();
            }

            #endregion

            #region Handle Subscriptions

            var allSubscriptionsCount = context.Subscriptions.Count();

            if (allSubscriptionsCount <= 0)
            {
                var subscriptions = new List<Subscription>();

                var getAllCompanies = context.Companies.ToList();
                var code = 0;
                var getBasicPlan = context.Plans
                    .FirstOrDefault(p => p.PlanNameTranslations.Any(pt => pt.Name == "Medium"));

                foreach (var company in getAllCompanies)
                {
                    code++;
                    subscriptions.Add(new()
                    {
                        CompanyId = company.Id,
                        PlanId = getBasicPlan?.Id ?? 0,
                        Code = code,
                        DurationInDays = 6 * 30,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(6 * 30),
                        Status = SubscriptionStatus.Active,
                        RenewalCount = 1,
                        EmployeeCost = getBasicPlan.EmployeeCost,
                        NumberOfEmployees = company.NumberOfEmployees,
                        TotalAmount = getBasicPlan.EmployeeCost * company.NumberOfEmployees,
                        FollowUpEmail = company.Email
                    });
                }

                context.Subscriptions.AddRange(subscriptions);
                context.SaveChanges();
            }

            #endregion

            #region Handle Dawem Setting

            var dawemSettings = new List<Setting>();

            if (context.DawemSettings.FirstOrDefault(d => d.SettingType == (int)AdminPanelSettingType.PlanGracePeriodPercentage) == null)
            {
                dawemSettings.Add(new()
                {
                    Type = AuthenticationType.AdminPanel,
                    SettingType = (int)AdminPanelSettingType.PlanGracePeriodPercentage,
                    TypeName = nameof(AdminPanelSettingType.PlanGracePeriodPercentage),
                    GroupType = (int)AdminPanelSettingGroupType.Plans,
                    GroupTypeName = nameof(AdminPanelSettingGroupType.Plans),
                    ValueType = SettingValueType.Integer,
                    ValueTypeName = nameof(SettingValueType.Integer),
                    Integer = 5
                });
            }
            if (context.DawemSettings.FirstOrDefault(d => d.SettingType == (int)AdminPanelSettingType.PlanTrialDurationInDays) == null)
            {
                dawemSettings.Add(new()
                {
                    Type = AuthenticationType.AdminPanel,
                    SettingType = (int)AdminPanelSettingType.PlanTrialDurationInDays,
                    TypeName = nameof(AdminPanelSettingType.PlanTrialDurationInDays),
                    GroupType = (int)AdminPanelSettingGroupType.Plans,
                    GroupTypeName = nameof(AdminPanelSettingGroupType.Plans),
                    ValueType = SettingValueType.Integer,
                    ValueTypeName = nameof(SettingValueType.Integer),
                    Integer = 3
                });
            }
            if (context.DawemSettings.FirstOrDefault(d => d.SettingType == (int)AdminPanelSettingType.PlanTrialEmployeesCount) == null)
            {
                dawemSettings.Add(new()
                {
                    Type = AuthenticationType.AdminPanel,
                    SettingType = (int)AdminPanelSettingType.PlanTrialEmployeesCount,
                    TypeName = nameof(AdminPanelSettingType.PlanTrialEmployeesCount),
                    GroupType = (int)AdminPanelSettingGroupType.Plans,
                    GroupTypeName = nameof(AdminPanelSettingGroupType.Plans),
                    ValueType = SettingValueType.Integer,
                    ValueTypeName = nameof(SettingValueType.Integer),
                    Integer = 2
                });
            }
            if (dawemSettings.Count > 0)
            {
                context.DawemSettings.AddRange(dawemSettings);
                context.SaveChanges();
            }
            #endregion

            #region Handle country phone numbers length

            /*var fullPath3 = "C:\\Users\\Leilla\\Downloads\\countries_phone_number_length.json";

            var jsonData3 = System.IO.File.ReadAllText(fullPath3);

            if (!string.IsNullOrWhiteSpace(jsonData3))
            {
                try
                {
                    var phones = JsonConvert.DeserializeObject<List<PhoneLengthDTO>>(jsonData3);

                    #region Seed phones

                    var getCountries = context.Countries.ToList();

                    if (phones?.Count() > 0)
                    {
                        foreach (var item in getCountries)
                        {
                            var getCountry = phones.FirstOrDefault(p => p.code == item.Iso);
                            if (getCountry != null)
                            {
                                item.Dial = getCountry.phone;
                                item.PhoneLength = getCountry.phoneLength;
                                context.SaveChanges();
                            }
                        }

                    }

                    #endregion
                }
                catch (Exception ex)
                {

                    throw;
                }
            }*/


            #endregion

            #region Handle time zones

            /* var fullPath3 = "C:\\Users\\Leilla\\Downloads\\\\Documents\\time-zone-country-code.json";

             var jsonData3 = System.IO.File.ReadAllText(fullPath3);

             if (!string.IsNullOrWhiteSpace(jsonData3) )
             {
                 var timezones = JsonConvert.DeserializeObject<List<timezone>>(jsonData3);
                 timezones = timezones.ToList();

                 var getAllCountries = context.Countries.ToList();

                 #region Seed timezones

                 for (int i = 0; i < timezones.Count; i++)
                 {
                     timezone timezone = timezones[i];

                     var getCurrentCountry = getAllCountries
                         .FirstOrDefault(c => c.Iso == timezone.country_code);

                     if (getCurrentCountry != null)
                     {
                         getCurrentCountry.TimeZoneToUTC = timezone.time_zone;
                     }



                 }
                 context.SaveChanges();
                 #endregion

             }
            */

            #endregion

            #region Handle Is Two Days Shift

            /*var shiftWorkingTimes = context.ShiftWorkingTimes.ToList();
       
            if (shiftWorkingTimes != null && shiftWorkingTimes.Count > 0)
            {
                foreach (var shiftWorkingTime in shiftWorkingTimes)
                {
                    shiftWorkingTime.IsTwoDaysShift = TimeHelper.
                        IsTwoDaysShift(shiftWorkingTime.CheckInTime, shiftWorkingTime.CheckOutTime);
                }
                context.SaveChanges();
            }*/

            #endregion

        }
    }
}