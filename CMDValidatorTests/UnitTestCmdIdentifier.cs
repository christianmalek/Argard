﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Argard.Exception;
using Argard;

namespace ArgardTests
{
    [TestClass]
    public class UnitTestCmdIdentifier
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
        public void TestMethodCmdIdentifiers1()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("inst[all]", expectedFunc);

            validator.Parse("inst");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers2()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", dummyFunc);
            validator.AddParameterSet("inst[all]", expectedFunc);

            validator.Parse("install");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers3()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", expectedFunc);
            validator.AddParameterSet("install", dummyFunc);

            validator.Parse("all");

            Assert.AreEqual(false, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers4()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("list", expectedFunc);

            try
            {
                validator.AddParameterSet("inst [all]", dummyFunc);
            }
            catch (InvalidParameterException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers5()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddParameterSet("(list)", dummyFunc);
            }
            catch (InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            validator.Parse("list");

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers6()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);

            try
            {
                validator.AddParameterSet("(inst[all] |  add )", expectedFunc);
            }
            catch (InvalidParameterSetException)
            {
                exceptionThrown = true;
            }

            validator.Parse("inst");

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers7()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("cmd, (inst[all] |  add )", expectedFunc);
            validator.Parse("cmd");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers8()
        {
            InitializeTest();

            ParameterSetParser validator = new ParameterSetParser(false);
            validator.AddParameterSet("cmd, (inst[all] |  add )", expectedFunc);
            validator.Parse("cmd");

            Assert.AreEqual(true, GetRaisingResults());
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers9()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);
            try
            {
                validator.AddParameterSet("cmd, inst[all], install", expectedFunc);
            }
            catch(Argard.Exception.MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers10()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);
            try
            {
                validator.AddParameterSet("cmd, list, inst, list", expectedFunc);
            }
            catch(MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }

        [TestMethod]
        public void TestMethodCmdIdentifiers11()
        {
            InitializeTest();
            bool exceptionThrown = false;

            ParameterSetParser validator = new ParameterSetParser(false);
            try
            {
                validator.AddParameterSet("cmd, cmd", expectedFunc);
            }
            catch (MultipleUseOfIdentifierNameException)
            {
                exceptionThrown = true;
            }

            Assert.AreEqual(true, exceptionThrown);
        }
    }
}