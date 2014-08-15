using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Argard;

namespace cmdValidatorTests
{
    [TestClass]
    public class UnitTestValueList
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
        public void TestMethodValueList1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^l", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList2()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^l", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList3()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^l", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install jquery lo-dash \"yet another option\"");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList4()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^l)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList5()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^l)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList6()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("inst[all]:(^l)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("inst angularjs jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList7()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("inst[all]:(^l)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("inst, lol dsadasdsa as,das, \"fds  gf gffds\", gfsdfs fdf");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList1_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^list", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList2_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^list", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList3_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:^list", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install jquery lo-dash \"yet another option\"");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList4_1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("install:(^list)", expectedFunc);
            validator.AddParameterSet("show", dummyFunc);

            validator.CheckArgs("install jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }
    }
}
