using Dawem.Translations;

namespace Dawem.Helpers
{
    public static class StringHelper
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = LeillaKeys.ForRandomStringAndNumber;
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomNumber(int length)
        {
            const string chars = LeillaKeys.AllNumbers;
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static DateTime GetLocalDateTime(string timeAoneId)
        {
            var localDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, timeAoneId).DateTime;
            return localDateTime;
        }
        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
    }
}
