using Dawem.Translations;

namespace Dawem.Helpers
{
    public static class StringHelper
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = DawemKeys.ForRandomString;
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumber(int length)
        {
            const string chars = DawemKeys.AllNumbers;
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
