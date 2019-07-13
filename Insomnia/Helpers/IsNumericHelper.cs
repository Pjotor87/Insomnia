using System.Globalization;

namespace Insomnia.Helpers
{
    public static class IsNumericHelper
    {
        public static bool IsInt(string str)
        {
            int temp;
            return int.TryParse(str, out temp);
        }

        public static bool IsBigInteger(string str)
        {
            int substringLength = int.MaxValue.ToString().Length - 1;

            for (int i = 0; i < str.Length;)
            {
                int lengthOfNextSubstring = i + substringLength;
                if (lengthOfNextSubstring > str.Length)
                    substringLength = str.Length - i;

                if (!IsInt(str.Substring(i, substringLength)))
                    return false;

                i += lengthOfNextSubstring;
            }

            return true;
        }

        public static bool IsDouble(string str)
        {
            double temp;
            return double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out temp);
        }

        public static bool IsNumeric(string str, bool allowNonIntegers, bool allowBigIntegers = false)
        {
            return
                allowNonIntegers ?
                IsDouble(str) :
                allowBigIntegers ?
                    IsBigInteger(str) :
                    IsInt(str);
        }

        public static bool IsNumeric(string[] strArr, bool allowNonIntegers = true, bool allowBigIntegers = false)
        {
            if (allowNonIntegers)
            {
                for (int i = 0; i < strArr.Length; i++)
                    if (!IsDouble(strArr[i]))
                        return false;
            }
            else if (allowBigIntegers)
            {
                for (int i = 0; i < strArr.Length; i++)
                    if (!IsBigInteger(strArr[i]))
                        return false;
            }
            else
            {
                for (int i = 0; i < strArr.Length; i++)
                    if (!IsInt(strArr[i]))
                        return false;
            }

            return true;
        }
    }
}
