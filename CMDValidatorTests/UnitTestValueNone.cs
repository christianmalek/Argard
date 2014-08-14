using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;

namespace cmdValidatorTests
{
    [TestClass]
    public class UnitTestValueNone
    {
        #region Test requirements
        bool expectedEventGotRaised;
        bool wrongEventGotRaised;

        private void InitializeTest()
        {
            this.expectedEventGotRaised = false;
            this.wrongEventGotRaised = false;
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
        }
        #endregion

        [TestMethod]
        public void TestMethodValueNone1()
        {
            InitializeTest();

            Parser validator = new Parser(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueNone2()
        {
            InitializeTest();

            Parser validator = new Parser(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs");

            Assert.AreEqual(false, GetRaisingResults());
        }
    }
}
