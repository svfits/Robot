// <copyright file="WarningCheckFilesRandomTest.cs">Copyright ©  2016</copyright>
using System;
using LocalDataBase.RandomFiles;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalDataBase.RandomFiles.Tests
{
    /// <summary>This class contains parameterized unit tests for WarningCheckFilesRandom</summary>
    [PexClass(typeof(WarningCheckFilesRandom))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class WarningCheckFilesRandomTest
    {
        /// <summary>Test stub for randomSleep(Int32, Int32)</summary>
        [PexMethod]
        public int randomSleepTest(int first, int end)
        {
            int result = WarningCheckFilesRandom.randomSleep(first, end);
            return result;
            // TODO: add assertions to method WarningCheckFilesRandomTest.randomSleepTest(Int32, Int32)
        }
    }
}
