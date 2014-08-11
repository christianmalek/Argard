using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;

namespace CMDValidatorTests
{
    [TestClass]
    public class UnitTestSimple
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
        public void TestMethodSimple1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("list");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple2()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install", expectedFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple3()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple4()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("    list     ", expectedFunc);
            validator.AddArgumentSet("  install  ", dummyFunc);

            validator.CheckArgs("list");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple5()
        {
            bool exceptionThrown = false;

            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            try
            {
                validator.AddArgumentSet("list", expectedFunc);
            }
            catch(InvalidArgumentSetException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodSimple6()
        {
            bool exceptionThrown = false;

            InitializeTest();

            Validator validator = new Validator(false, true);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            try
            {
                validator.AddArgumentSet("lIsT", expectedFunc);
            }
            catch (InvalidArgumentSetException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodSimple7()
        {
            bool exceptionThrown = false;

            InitializeTest();

            Validator validator = new Validator(false, false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            try
            {
                validator.AddArgumentSet("lIsT", expectedFunc);
            }
            catch (InvalidArgumentSetException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(false, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodSimple8()
        {
            InitializeTest();

            Validator validator = new Validator(false, true);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("LIST");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple9()
        {
            InitializeTest();

            Validator validator = new Validator(false, false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("LIST");

            Assert.AreEqual(false, GetRaisingResults());
        }
    }
}