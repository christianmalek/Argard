using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using cmdValidator;
using cmdValidator.Exception;

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
        public void TestMethodFlags1()
        {
            bool exceptionThrown = false;

            InitializeTest();

            Parser validator = new Parser(false);

            try
            {
                validator.AddArgumentSet("list , l , i , s, t", expectedFunc);
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

            Parser validator = new Parser(false);

            try
            {
                validator.AddArgumentSet("l[i]st , l, s, t", expectedFunc);
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

            Parser validator = new Parser(false);

            try
            {
                validator.AddArgumentSet("l[i]st|showall , s, h, o, w, a, l", expectedFunc);
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

            Parser validator = new Parser(false);

            try
            {
                validator.AddArgumentSet("list, x|s, k|l, m|t, o|t", expectedFunc);
            }
            catch (MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }
    }
}
