using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Argard;
using Argard.Exception;

namespace ArgardTests
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
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("list");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple2()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install", expectedFunc);

            validator.Parse("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple3()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple4()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("    list     ", expectedFunc);
            validator.AddParameterSet("  install  ", dummyFunc);

            validator.Parse("list");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple5()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            try
            {
                validator.AddParameterSet("list", expectedFunc);
            }
            catch(InvalidParameterSetException)
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
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            try
            {
                validator.AddParameterSet("lIsT", expectedFunc);
            }
            catch (InvalidParameterSetException)
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
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            try
            {
                validator.AddParameterSet("lIsT", expectedFunc);
            }
            catch (InvalidParameterSetException)
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
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("LIST");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodSimple9()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false, false);
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("LIST");

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
                validator.AddParameterSet("(list)", expectedFunc);
            }
            catch (InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("list");

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
                validator.AddParameterSet("(list), xyz", expectedFunc);
            }
            catch(InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("list");

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
                validator.AddParameterSet("list, ()", expectedFunc);
            }
            catch (InvalidParameterException)
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
                validator.AddParameterSet("list, install, show, install: (^s) ", expectedFunc);
            }
            catch (MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }
    }
}