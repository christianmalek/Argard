using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;

namespace CMDValidatorTests
{
    [TestClass]
    public class UnitTestMultipleIdentifiers
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
        public void TestMethodMultipleIdentifiers1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install,src", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install --src");

            Assert.AreEqual(true, GetRaisingResults());
        }
    }
}
