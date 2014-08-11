using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;

namespace CMDValidatorTests
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
        public void TestMethodValueList1()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^l", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList2()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^l", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList3()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:^l", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install jquery lo-dash \"yet another option\"");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList4()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^l)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList5()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("install:(^l)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList6()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("inst[all]:(^l)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("inst angularjs jquery");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodValueList7()
        {
            InitializeTest();

            Validator validator = new Validator(false);
            validator.AddArgumentSet("list", dummyFunc);
            validator.AddArgumentSet("inst[all]:(^l)", expectedFunc);
            validator.AddArgumentSet("show", dummyFunc);

            validator.CheckArgs("inst, lol dsadasdsa as,das, \"fds  gf gffds\", gfsdfs fdf");

            Assert.AreEqual(true, GetRaisingResults());
        }
    }
}
