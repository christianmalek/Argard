using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;

namespace CMDValidatorTests
{
    [TestClass]
    public class UnitTestAdvanced
    {
        #region Test requirements
        bool expectedEventGotRaised;
        bool wrongEventGotRaised;
        ArgumentSetArgs args;

        private void InitializeTest()
        {
            this.expectedEventGotRaised = false;
            this.wrongEventGotRaised = false;
            this.args = null;
        }

        private bool GetRaisingResults()
        {
            return expectedEventGotRaised && !wrongEventGotRaised;
        }

        void dummyFunc(ArgumentSetArgs args)
        {
            this.wrongEventGotRaised = true;
        }

        void expectedFunc(ArgumentSetArgs args)
        {
            this.expectedEventGotRaised = true;
            this.args = args;
        }
        #endregion

        [TestMethod]
        public void TestMethodAdvanced1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list  : x", expectedFunc);

            validator.CheckArgs("list x");

            bool actual = args.CMD["list"].FirstValue == "x";

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void TestMethodAdvanced2()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list  : x |   y |   z", expectedFunc);

            validator.CheckArgs("list y");

            bool actual = args.CMD["list"].ParsedValues[0] == "y";

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void TestMethodAdvanced3()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("l[i]st  :( x |   y | z  | n     )", expectedFunc);

            validator.CheckArgs("lst z");

            bool actual = args.CMD["list"].ParsedValues[0] == "z";

            Assert.AreEqual(true, actual);
        }
    }
}
