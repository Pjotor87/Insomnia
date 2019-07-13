using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Insomnia.Helpers;

namespace Tests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void IsNumeric()
        {
            string notNumeric = "asdjiofj";
            string numeric1 = "0";
            string numeric2 = "-1";
            string numeric3 = "-5,5";
            string numeric4 = "-5.5";
            string numeric5 = "1,35893346756348756347856347865347865783465783465783467856347856347856347865783465783465786347856347857534865783465347853478";
            string numeric6 = "-0,564";
            string numeric7 = "-0,564347892347589347589347589347589347895348957348972897238974238974892374923748923789423747238947238947238974239";
            string numeric8 = "123456789123456789123456789123456789123456789123456789123456789123456789123456789";
            string[] numericArr = new string[] { numeric1, numeric2, numeric3, numeric4, numeric5, numeric6, numeric7, numeric8 };
            string[] nonNumericArr = new string[] { numeric1, numeric2, numeric3, numeric4, numeric5, numeric6, numeric7, numeric8, notNumeric };
            string[] numericIntOnlyArr = new string[] { numeric1, numeric2 };

            Assert.IsFalse(IsNumericHelper.IsNumeric(notNumeric, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric1, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric2, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric3, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric4, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric5, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric6, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric7, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric8, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numericArr, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(nonNumericArr, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numericIntOnlyArr, true));

            Assert.IsFalse(IsNumericHelper.IsNumeric(notNumeric, false));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric1, false));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric2, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric3, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric4, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric5, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric6, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric7, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric8, false)); // false
            Assert.IsFalse(IsNumericHelper.IsNumeric(numericArr, false));
            Assert.IsFalse(IsNumericHelper.IsNumeric(nonNumericArr, false));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numericIntOnlyArr, false));

            Assert.IsFalse(IsNumericHelper.IsNumeric(notNumeric, false, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric1, false, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric2, false, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric3, false, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric4, false, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric5, false, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric6, false, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(numeric7, false, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numeric8, false, true));
            StringBuilder numeric8Builder = new StringBuilder();
            numeric8Builder.Append(numeric8);
            for (int i = 0; i < int.MaxValue.ToString().Length + 1; i++)
            {
                numeric8Builder.Append("1");
                Assert.IsTrue(IsNumericHelper.IsNumeric(numeric8Builder.ToString(), false, true));
            }
            Assert.IsFalse(IsNumericHelper.IsNumeric(numericArr, false, true));
            Assert.IsFalse(IsNumericHelper.IsNumeric(nonNumericArr, false, true));
            Assert.IsTrue(IsNumericHelper.IsNumeric(numericIntOnlyArr, false, true));
        }

        [TestMethod]
        public void ParseCommandlineArgs()
        {
            string argKeyIdentifier1 = "-";
            string argKeyIdentifier2 = "--";
            string argKeyIdentifier3 = "ThisIsMyArgumentIdentifier";
            string argKey1 = "a";
            string argKey2 = "b";
            string argValue1 = "value1";
            string argValue2 = "value2";

            string args1 = $"{argKeyIdentifier1}{argKey1} {argValue1} {argKeyIdentifier1}{argKey2} {argValue2}";
            string args2 = $"{argKeyIdentifier1}{argKey1} {argValue1} {argKeyIdentifier1}{argKey1} {argValue2}";
            string args3 = $"{argKeyIdentifier2}{argKey1} {argValue1} {argKeyIdentifier2}{argKey2} {argValue2}";
            string args4 = $"{argKeyIdentifier2}{argKey1} {argValue1} {argKeyIdentifier2}{argKey1} {argValue2}";
            string args5 = $"{argKeyIdentifier3}{argKey1} {argValue1} {argKeyIdentifier3}{argKey2} {argValue2}";
            string args6 = $"{argKeyIdentifier3}{argKey1} {argValue1} {argKeyIdentifier3}{argKey1} {argValue2}";
            string args7 = $"{argKeyIdentifier1}{argKey1} {argValue1} {argKeyIdentifier1}{argKey1} {argValue1}";

            IList<KeyValuePair<string, string>> parsedArgs1a = CommandLineArgumentHelper.ParseCommandlineArgs(args1.Split(' '));
            IList<KeyValuePair<string, string>> parsedArgs2a = CommandLineArgumentHelper.ParseCommandlineArgs(args2.Split(' '));
            IList<KeyValuePair<string, string>> parsedArgs1b = CommandLineArgumentHelper.ParseCommandlineArgs(args1.Split(' '), argKeyIdentifier1);
            IList<KeyValuePair<string, string>> parsedArgs2b = CommandLineArgumentHelper.ParseCommandlineArgs(args2.Split(' '), argKeyIdentifier1);
            IList<KeyValuePair<string, string>> parsedArgs3 = CommandLineArgumentHelper.ParseCommandlineArgs(args3.Split(' '), argKeyIdentifier2);
            IList<KeyValuePair<string, string>> parsedArgs4 = CommandLineArgumentHelper.ParseCommandlineArgs(args4.Split(' '), argKeyIdentifier2);
            IList<KeyValuePair<string, string>> parsedArgs5 = CommandLineArgumentHelper.ParseCommandlineArgs(args5.Split(' '), argKeyIdentifier3);
            IList<KeyValuePair<string, string>> parsedArgs6 = CommandLineArgumentHelper.ParseCommandlineArgs(args6.Split(' '), argKeyIdentifier3);
            IList<KeyValuePair<string, string>> parsedArgs7 = CommandLineArgumentHelper.ParseCommandlineArgs(args7.Split(' '), argKeyIdentifier1);

            IDictionary<string, string> parsedArgsToDict1a = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args1.Split(' '));
            IDictionary<string, string> parsedArgsToDict2a = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args2.Split(' '));
            IDictionary<string, string> parsedArgsToDict1b = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args1.Split(' '), argKeyIdentifier1);
            IDictionary<string, string> parsedArgsToDict2b = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args2.Split(' '), argKeyIdentifier1);
            IDictionary<string, string> parsedArgsToDict3 = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args3.Split(' '), argKeyIdentifier2);
            IDictionary<string, string> parsedArgsToDict4 = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args4.Split(' '), argKeyIdentifier2);
            IDictionary<string, string> parsedArgsToDict5 = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args5.Split(' '), argKeyIdentifier3);
            IDictionary<string, string> parsedArgsToDict6 = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args6.Split(' '), argKeyIdentifier3);
            IDictionary<string, string> parsedArgsToDict7 = CommandLineArgumentHelper.ParseCommandlineArgsToDict(args7.Split(' '), argKeyIdentifier1);

            Assert.IsTrue(parsedArgs1a.Count == 2);
            Assert.IsTrue(parsedArgs2a.Count == 2);
            Assert.IsTrue(parsedArgs1b.Count == 2);
            Assert.IsTrue(parsedArgs2b.Count == 2);
            Assert.IsTrue(parsedArgs3.Count == 2);
            Assert.IsTrue(parsedArgs4.Count == 2);
            Assert.IsTrue(parsedArgs5.Count == 2);
            Assert.IsTrue(parsedArgs6.Count == 2);
            Assert.IsTrue(parsedArgs7.Count == 2);

            Assert.IsTrue(parsedArgsToDict1a.Count == 2);
            Assert.IsTrue(parsedArgsToDict2a.Count == 1);
            Assert.IsTrue(parsedArgsToDict2a[argKey1] == argValue2);
            Assert.IsTrue(parsedArgsToDict1b.Count == 2);
            Assert.IsTrue(parsedArgsToDict2b.Count == 1);
            Assert.IsTrue(parsedArgsToDict2b[argKey1] == argValue2);
            Assert.IsTrue(parsedArgsToDict3.Count == 2);
            Assert.IsTrue(parsedArgsToDict4.Count == 1);
            Assert.IsTrue(parsedArgsToDict4[argKey1] == argValue2);
            Assert.IsTrue(parsedArgsToDict5.Count == 2);
            Assert.IsTrue(parsedArgsToDict6.Count == 1);
            Assert.IsTrue(parsedArgsToDict6[argKey1] == argValue2);
            Assert.IsTrue(parsedArgsToDict7.Count == 1);
            Assert.IsTrue(parsedArgsToDict7[argKey1] == argValue1);
        }
    }
}