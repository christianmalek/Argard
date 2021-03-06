﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Argard;

namespace ArgardTests
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
        public void TestMethodValueSingle1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^s", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle2()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^s", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle3()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^s", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install --angularjs");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle4()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^s", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install angularjs jquery");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle5()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^s)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle6()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^s)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle7()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^s)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install jquery lo-dash");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle1_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^single", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle2_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^single", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle3_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^single", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install --angularjs");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle4_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^single", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install angularjs jquery");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle5_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^single)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install angularjs");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle6_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^single)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueSingle7_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^single)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.Parse("install jquery lo-dash");

            Assert.AreEqual(false, GetRaisingResults());
        }
    }
}
