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
    }
}
