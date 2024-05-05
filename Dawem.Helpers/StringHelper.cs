using Dawem.Translations;
using System.Text;

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
        public static bool IsDigitsOnly(this string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        public static string FixFingerprintBody(string input, int occurrence)
        {
            if (string.IsNullOrEmpty(input) || occurrence <= 0)
                return input;

            StringBuilder result = new StringBuilder();
            int spaceCount = 0;

            foreach (char c in input)
            {
                if (c == ' ')
                {
                    spaceCount++;
                    if (spaceCount == occurrence)
                    {
                        result.Append('\n');
                        spaceCount = 0;
                        continue;
                    }
                    if (spaceCount == 2)
                        result.Append(c);
                    else
                        result.Append('\t');

                }
                else
                    result.Append(c);
            }

            return result.ToString();
        }
    }
}
