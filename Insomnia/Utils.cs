using System.Collections.Generic;

namespace Insomnia
{
    public static class Utils
    {
        public static bool IsNumeric(string str)
        {
            int temp;
            return int.TryParse(str, out temp);
        }

        public static bool IsNumeric(string[] strArr)
        {
            for (int i = 0; i < strArr.Length; i++)
                if (!IsNumeric(strArr[i]))
                    return false;
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
