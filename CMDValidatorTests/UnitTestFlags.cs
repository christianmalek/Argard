using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Argard;
using Argard.Exception;

namespace cmdValidatorTests
{
    [TestClass]
    public class UnitTestFlags
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
        public void TestMethodFlags1()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddParameterSet("list , l , i , s, t", expectedFunc);
            }
            catch (ConflictingFlagNamesException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodFlags2()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddParameterSet("l[i]st , l, s, t", expectedFunc);
            }
            catch (ConflictingFlagNamesException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodFlags3()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddParameterSet("l[i]st|showall , s, h, o, w, a, l", expectedFunc);
            }
            catch (ConflictingFlagNamesException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodFlags4()
        {
            bool exceptionThrown = false;

            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddParameterSet("list, x|s, k|l, m|t, o|t", expectedFunc);
            }
            catch (MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodFlags5()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("l[i]st , l, t", expectedFunc);
            validator.CheckArgs("lst -lt");

            Assert.AreEqual(true, expectedEventGotRaised);
        }

        [TestMethod]
        public void TestMethodFlags6()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("l[i]st , a, b, c, d, e, test1, test2, abk", expectedFunc);
            validator.CheckArgs("lst -abed -test1 -test2 -abk -c");

            Assert.AreEqual(true, expectedEventGotRaised);
        }
    }
}