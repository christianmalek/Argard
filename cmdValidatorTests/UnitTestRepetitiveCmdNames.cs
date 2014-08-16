using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Argard;
using Argard.Exception;

namespace ArgardTests
{
    [TestClass]
    public class UnitTestRepetitiveCmdNames
    {
        #region Test requirements
        bool expectedEventGotRaised;
        bool wrongEventGotRaised;
        ParameterSetArgs args;

        private void InitializeTest()
        {
            this.expectedEventGotRaised = false;
            this.wrongEventGotRaised = false;
            args = null;
        }

        private bool GetRaisingResults()
        {
            return expectedEventGotRaised && !wrongEventGotRaised;
        }

        void dummyFunc(ParameterSetArgs args)
        {
            this.wrongEventGotRaised = true;
            this.args = args;
        }

        void expectedFunc(ParameterSetArgs args)
        {
            this.expectedEventGotRaised = true;
            this.args = args;
        }
        #endregion

        [TestMethod]
        public void TestMethodRepetitiveCmdNames1()
        {
            InitializeTest();

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install | add,x, y", dummyFunc);
            parser.AddParameterSet("install,y:(aloha)", dummyFunc);

            parser.CheckArgs("install -y");

            bool actual = args.Options["y"].ArgumentValues.AllowedValues[0] == "aloha";

            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void TestMethodRepetitiveCmdNames2()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install | add,y", dummyFunc);
            try
            {
                parser.AddParameterSet("add,y", dummyFunc);
            }
            catch(InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            parser.CheckArgs("install -y");

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodRepetitiveCmdNames3()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install,x,(y)", dummyFunc);
            try
            {
                parser.AddParameterSet("install,x", dummyFunc);
            }
            catch (InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodRepetitiveCmdNames4()
        {
            InitializeTest();

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install,x,(y), z", dummyFunc);
            parser.AddParameterSet("install,x, y", expectedFunc);
            parser.CheckArgs("install -x -y");

            Assert.AreEqual(true, expectedEventGotRaised);
        }

        [TestMethod]
        public void TestMethodRepetitiveCmdNames5()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install,(x)", dummyFunc);
            try
            {
                parser.AddParameterSet("install,x", dummyFunc);
            }
            catch (InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            parser.CheckArgs("install -y");

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodRepetitiveCmdNames6()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install,x", dummyFunc);
            try
            {
                parser.AddParameterSet("install,(x)", dummyFunc);
            }
            catch (InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodRepetitiveCmdNames7()
        {
            InitializeTest();

            ParameterSetParser parser = new ParameterSetParser(false);
            parser.AddParameterSet("install,x,(y), z", dummyFunc);
            parser.AddParameterSet("list", dummyFunc);
            parser.AddParameterSet("install,x, y", expectedFunc);
            parser.CheckArgs("install -x -y");

            Assert.AreEqual(true, expectedEventGotRaised);
        }
    }
}
