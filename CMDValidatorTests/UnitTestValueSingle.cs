using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;

namespace CMDValidatorTests
{
    [TestClass]
    public class UnitTestValueSingle
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
        public void TestMethodValueSingle1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^s", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle2()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^s", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle3()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^s", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install --angularjs");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle4()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^s", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs jquery");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle5()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^s)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle6()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^s)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle7()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^s)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install jquery lo-dash");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle1_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^single", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle2_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^single", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle3_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^single", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install --angularjs");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle4_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^single", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs jquery");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle5_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^single)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle6_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^single)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle7_1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^single)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install jquery lo-dash");

            Assert.AreEqual(false, GetRaisingResults());
        }
    }
}
