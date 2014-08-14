using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;
using cmdValidator.Exception;

namespace cmdValidatorTests
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
        public void TestMethodSimple1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("list");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple2()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install", expectedFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple3()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple4()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
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

            ParameterSetParser validator = new ParameterSetParser(false);
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

            ParameterSetParser validator = new ParameterSetParser(false, true);
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

            ParameterSetParser validator = new ParameterSetParser(false, false);
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

            ParameterSetParser validator = new ParameterSetParser(false, true);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("LIST");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple9()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false, false);
            validator.AddArgumentSet("list", expectedFunc);
            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("LIST");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple10()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddArgumentSet("(list)", expectedFunc);
            }
            catch (InvalidArgumentSetException)
            {
                exceptionThrown = true;
            }

            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("list");

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodSimple11()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddArgumentSet("(list), xyz", expectedFunc);
            }
            catch(InvalidArgumentSetException)
            {
                exceptionThrown = true;
            }

            validator.AddArgumentSet("install", dummyFunc);

            validator.CheckArgs("list");

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodSimple12()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddArgumentSet("list, ()", expectedFunc);
            }
            catch (InvalidArgumentSchemeException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodSimple13()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddArgumentSet("list, install, show, install: (^s) ", expectedFunc);
            }
            catch (MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }
    }
}