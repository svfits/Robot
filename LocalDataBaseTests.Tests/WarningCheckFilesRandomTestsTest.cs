// <copyright file="WarningCheckFilesRandomTestsTest.cs">Copyright ©  2017</copyright>
using System;
using LocalDataBase.RandomFiles.Tests;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalDataBase.RandomFiles.Tests.Tests
{
    /// <summary>This class contains parameterized unit tests for WarningCheckFilesRandomTests</summary>
    [PexClass(typeof(WarningCheckFilesRandomTests))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class WarningCheckFilesRandomTestsTest
    {
        /// <summary>Test stub for RandomFilesTest1()</summary>
        [PexMethod]
        public void RandomFilesTest1Test([PexAssumeUnderTest]WarningCheckFilesRandomTests target)
        {
            target.RandomFilesTest1();
            // TODO: add assertions to method WarningCheckFilesRandomTestsTest.RandomFilesTest1Test(WarningCheckFilesRandomTests)
        }
             
    }
}
