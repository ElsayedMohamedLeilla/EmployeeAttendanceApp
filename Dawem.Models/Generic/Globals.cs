using Dawem.Translations;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using TimeZoneConverter;

namespace Dawem.Models.Generic
{
    public static class Globals
    {
        public static string ToRfc3339String(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
        }

        public static string GetUrlString(string baseUri, string relativeUri)
        {
            if (baseUri == null || relativeUri == null) return null;
            var returnedUrl = !baseUri.EndsWith('/') ? baseUri + "/" : baseUri;
            returnedUrl += relativeUri.StartsWith('/') ? relativeUri.Substring(1, relativeUri.Length) : relativeUri;
            return returnedUrl;
        }
        public static DateTime ToLocal(this DateTime currDate, GeneralSetting generalSetting)
        {
            if (generalSetting != null)
            {
                BasicCountryInfo countryInfo = generalSetting.SmartCulture;
                if (countryInfo == null && generalSetting.CountryId > 0)
                {
                    countryInfo = CountryCultures.FirstOrDefault(a => a.CountryId == generalSetting.CountryId);
                }
                if (countryInfo == null)
                {
                    countryInfo = CountryCultures.FirstOrDefault(a => a.CountryId == 1938);
                }
                var tz = TZConvert.GetTimeZoneInfo(countryInfo.TimeZoneId);
                try
                {
                    DateTime tstTime = TimeZoneInfo.ConvertTimeFromUtc(currDate, tz);
                    return tstTime;
                }
                catch (Exception ex)
                {
                    return currDate;

                    throw ex;


                }
            }
            else
            {
                return currDate;
            }

        }
        public static float? Round(bool roundTheAllFraction, float? price)
        {
            return price == null ? null : Round(roundTheAllFraction, price.Value);
        }
        public static float Round(bool roundTheAllFraction, float price)
        {
            return roundTheAllFraction ? MathF.Round(price, MidpointRounding.AwayFromZero) : MathF.Round(price, 2);
        }
        public static float CalculateTaxes(bool allowTax, float? price, float? rate)
        {
            price = price ?? 0;
            rate = rate ?? 0;
            return allowTax ? (float)(price * (1 + rate * .01)) : price.GetValueOrDefault();
        }
        public static float CalculateTaxValue(bool allowTax, float? price, float? rate)
        {
            price = price ?? 0;
            rate = rate ?? 0;
            return allowTax ? (float)(price * (rate * .01)) : 0;
        }

        public const string FeaturesFolder = DawemKeys.AccountFeatures;
        public static IEnumerable<BasicCountryInfo> CountryCultures = new List<BasicCountryInfo>() {
            new BasicCountryInfo(){ TimeZoneId =DawemKeys.ArabStandardTime, CountryId=1939, CultureName = DawemKeys.ArSA, CurrentCulture = CultureInfo.GetCultureInfo(DawemKeys.ArSA)} ,
            new BasicCountryInfo(){ TimeZoneId =DawemKeys.EgyptStandardTime, CountryId=1938, CultureName = DawemKeys.ArEG, CurrentCulture = CultureInfo.GetCultureInfo(DawemKeys.ArEG) },
            new BasicCountryInfo(){ TimeZoneId =DawemKeys.EgyptStandardTime, CountryId=1938, CultureName =DawemKeys.En, CurrentCulture = CultureInfo.GetCultureInfo(DawemKeys.ArEG) }
        };

        public static string? DefualtCountryCode { get; set; }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }



        public static bool IsDigits(this string numbersString)
        {
            if (string.IsNullOrWhiteSpace(numbersString)) return true;
            Regex regex = new Regex(@"^\d$");
            if (Regex.IsMatch(numbersString, @"^\d+$"))
            {
                return true;
            }
            return false;
        }



    }
}
