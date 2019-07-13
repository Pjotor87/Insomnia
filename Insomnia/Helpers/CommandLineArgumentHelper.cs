using System.Collections.Generic;

namespace Insomnia.Helpers
{
    public static class CommandLineArgumentHelper
    {
        public const string DEFAULT_ARG_KEY = "-";

        public static IList<KeyValuePair<string, string>> ParseCommandlineArgs(string[] args, string argKeyStartsWith = DEFAULT_ARG_KEY)
        {
            IList<KeyValuePair<string, string>> parsedCommandlineArgs = new List<KeyValuePair<string, string>>();

            if (args != null && args.Length >= 1)
                for (int i = 0; i < args.Length; i++)
                {
                    string argumentKey = GetArgumentKey(args, argKeyStartsWith, i);

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

        public static IDictionary<string, string> ParseCommandlineArgsToDict(string[] args, string argKeyStartsWith = DEFAULT_ARG_KEY)
        {
            IDictionary<string, string> parsedCommandlineArgs = new Dictionary<string, string>();

            if (args != null && args.Length >= 1)
                for (int i = 0; i < args.Length; i++)
                {
                    string argumentKey = GetArgumentKey(args, argKeyStartsWith, i);

                    if (string.IsNullOrEmpty(argumentKey) ||
                        args.Length == i)
                        continue;
                    else
                    {
                        string argumentValue = GetArgumentValue(args, i);

                        if (!string.IsNullOrEmpty(argumentValue))
                            if (!parsedCommandlineArgs.ContainsKey(argumentKey))
                                parsedCommandlineArgs.Add(new KeyValuePair<string, string>(argumentKey, argumentValue));
                            else
                                parsedCommandlineArgs[argumentKey] = argumentValue;
                    }
                }

            return parsedCommandlineArgs;
        }

        private static string GetArgumentValue(string[] args, int i)
        {
            return 
                args.Length >= (i + 1) ?
                args[i + 1] :
                string.Empty;
        }

        private static string GetArgumentKey(string[] args, string argKeyStartsWith, int i)
        {
            return
                args[i].StartsWith(argKeyStartsWith) ?
                args[i].Remove(0, argKeyStartsWith.Length).ToLower() :
                string.Empty;
        }
    }
}
