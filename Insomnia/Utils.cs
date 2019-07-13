using System;
using System.Collections.Generic;
using System.Globalization;

namespace Insomnia
{
    public static class Utils
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
                {
                    substringLength = str.Length - i;
                }

                string subString = str.Substring(i, substringLength);

                if (!IsInt(subString))
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

        public static IList<KeyValuePair<string, string>> ParseCommandlineArgs(string[] args)
        {
            return ParseCommandlineArgs(args, "-");
        }

        public static IList<KeyValuePair<string, string>> ParseCommandlineArgs(string[] args, string argKeyStartsWith)
        {
            IList<KeyValuePair<string, string>> parsedCommandlineArgs = new List<KeyValuePair<string, string>>();

            if (args != null && args.Length >= 1)
                for (int i = 0; i < args.Length; i++)
                {
                    string argumentKey = 
                        args[i].StartsWith(argKeyStartsWith) ? 
                        args[i].Remove(0, argKeyStartsWith.Length).ToLower() : 
                        string.Empty;

                    if (string.IsNullOrEmpty(argumentKey) ||
                        args.Length == i)
                        continue;
                    else
                    {
                        string argumentValue = 
                            (args.Length >= (i + 1)) ? 
                            args[i + 1] : 
                            string.Empty;

                        if (!string.IsNullOrEmpty(argumentValue))
                            parsedCommandlineArgs.Add(new KeyValuePair<string, string>(argumentKey, argumentValue));
                    }
                }

            return parsedCommandlineArgs;
        }
    }
}
