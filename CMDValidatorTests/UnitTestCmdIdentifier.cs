using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator.Exception;
using cmdValidator;

namespace cmdValidatorTests
{
    [TestClass]
    public class UnitTestCmdIdentifier
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

        void dummyFunc(ParameterSetArgs args)
        {
            this.wrongEventGotRaised = true;
        }

        void expectedFunc(ParameterSetArgs args)
        {
            this.expectedEventGotRaised = true;
        }
        #endregion

        [TestMethod]
        public void TestMethodCmdIdentifiers1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("inst[all]", expectedFunc);

            validator.CheckArgs("inst");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers2()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("inst[all]", expectedFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers3()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("all");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers4()
        {
            InitializeTest();

            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", expectedFunc);

            try
            {
                validator.AddArgumentSet("inst [all]", dummyFunc);
            }
            catch (InvalidArgumentSchemeException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }
    }
}
