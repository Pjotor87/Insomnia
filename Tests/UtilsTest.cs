using System.Collections.Generic;
using System.Text;
using Insomnia;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            Assert.IsFalse(Utils.IsNumeric(notNumeric, true));
            Assert.IsTrue(Utils.IsNumeric(numeric1, true));
            Assert.IsTrue(Utils.IsNumeric(numeric2, true));
            Assert.IsTrue(Utils.IsNumeric(numeric3, true));
            Assert.IsTrue(Utils.IsNumeric(numeric4, true));
            Assert.IsTrue(Utils.IsNumeric(numeric5, true));
            Assert.IsTrue(Utils.IsNumeric(numeric6, true));
            Assert.IsTrue(Utils.IsNumeric(numeric7, true));
            Assert.IsTrue(Utils.IsNumeric(numeric8, true));
            Assert.IsTrue(Utils.IsNumeric(numericArr, true));
            Assert.IsFalse(Utils.IsNumeric(nonNumericArr, true));
            Assert.IsTrue(Utils.IsNumeric(numericIntOnlyArr, true));

            Assert.IsFalse(Utils.IsNumeric(notNumeric, false));
            Assert.IsTrue(Utils.IsNumeric(numeric1, false));
            Assert.IsTrue(Utils.IsNumeric(numeric2, false));
            Assert.IsFalse(Utils.IsNumeric(numeric3, false));
            Assert.IsFalse(Utils.IsNumeric(numeric4, false));
            Assert.IsFalse(Utils.IsNumeric(numeric5, false));
            Assert.IsFalse(Utils.IsNumeric(numeric6, false));
            Assert.IsFalse(Utils.IsNumeric(numeric7, false));
            Assert.IsFalse(Utils.IsNumeric(numeric8, false)); // false
            Assert.IsFalse(Utils.IsNumeric(numericArr, false));
            Assert.IsFalse(Utils.IsNumeric(nonNumericArr, false));
            Assert.IsTrue(Utils.IsNumeric(numericIntOnlyArr, false));

            Assert.IsFalse(Utils.IsNumeric(notNumeric, false, true));
            Assert.IsTrue(Utils.IsNumeric(numeric1, false, true));
            Assert.IsTrue(Utils.IsNumeric(numeric2, false, true));
            Assert.IsFalse(Utils.IsNumeric(numeric3, false, true));
            Assert.IsFalse(Utils.IsNumeric(numeric4, false, true));
            Assert.IsFalse(Utils.IsNumeric(numeric5, false, true));
            Assert.IsFalse(Utils.IsNumeric(numeric6, false, true));
            Assert.IsFalse(Utils.IsNumeric(numeric7, false, true));
            Assert.IsTrue(Utils.IsNumeric(numeric8, false, true));
            StringBuilder numeric8Builder = new StringBuilder();
            numeric8Builder.Append(numeric8);
            for (int i = 0; i < int.MaxValue.ToString().Length + 1; i++)
            {
                numeric8Builder.Append("1");
                Assert.IsTrue(Utils.IsNumeric(numeric8Builder.ToString(), false, true));
            }
            Assert.IsFalse(Utils.IsNumeric(numericArr, false, true));
            Assert.IsFalse(Utils.IsNumeric(nonNumericArr, false, true));
            Assert.IsTrue(Utils.IsNumeric(numericIntOnlyArr, false, true));
        }

        [TestMethod]
        public void ParseCommandlineArgs()
        {
            string argKeyIdentifier1 = "-";
            string argKeyIdentifier2 = "--";
            string argKeyIdentifier3 = "ThisIsMyArgumentIdentifier";

            string argValue1 = "value1";
            string argValue2 = "value2";

            string args1 = string.Format("{0}a value1 {0}b value2", argKeyIdentifier1);
            string args2 = string.Format("{0}a value1 {0}a value2", argKeyIdentifier1);

            string args3 = string.Format("{0}a value1 {0}b value2", argKeyIdentifier2);
            string args4 = string.Format("{0}a value1 {0}a value2", argKeyIdentifier2);

            string args5 = string.Format("{0}a value1 {0}b value2", argKeyIdentifier3);
            string args6 = string.Format("{0}a value1 {0}a value2", argKeyIdentifier3);

            string args7 = string.Format("{0}a value1 {0}a value1", argKeyIdentifier1);

            IList<KeyValuePair<string, string>> parsedArgs1a = Utils.ParseCommandlineArgs(args1.Split(' '));
            IList<KeyValuePair<string, string>> parsedArgs2a = Utils.ParseCommandlineArgs(args2.Split(' '));
            IList<KeyValuePair<string, string>> parsedArgs1b = Utils.ParseCommandlineArgs(args1.Split(' '), argKeyIdentifier1);
            IList<KeyValuePair<string, string>> parsedArgs2b = Utils.ParseCommandlineArgs(args2.Split(' '), argKeyIdentifier1);
            IList<KeyValuePair<string, string>> parsedArgs3 = Utils.ParseCommandlineArgs(args3.Split(' '), argKeyIdentifier2);
            IList<KeyValuePair<string, string>> parsedArgs4 = Utils.ParseCommandlineArgs(args4.Split(' '), argKeyIdentifier2);
            IList<KeyValuePair<string, string>> parsedArgs5 = Utils.ParseCommandlineArgs(args5.Split(' '), argKeyIdentifier3);
            IList<KeyValuePair<string, string>> parsedArgs6 = Utils.ParseCommandlineArgs(args6.Split(' '), argKeyIdentifier3);
            IList<KeyValuePair<string, string>> parsedArgs7 = Utils.ParseCommandlineArgs(args7.Split(' '), argKeyIdentifier1);

            Assert.IsTrue(parsedArgs1a.Count == 2);
            Assert.IsTrue(parsedArgs2a.Count == 2);
            Assert.IsTrue(parsedArgs1b.Count == 2);
            Assert.IsTrue(parsedArgs2b.Count == 2);
            Assert.IsTrue(parsedArgs3.Count == 2);
            Assert.IsTrue(parsedArgs4.Count == 2);
            Assert.IsTrue(parsedArgs5.Count == 2);
            Assert.IsTrue(parsedArgs6.Count == 2);
            Assert.IsTrue(parsedArgs7.Count == 2);
        }
    }
}
