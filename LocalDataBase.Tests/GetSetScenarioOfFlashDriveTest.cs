// <copyright file="GetSetScenarioOfFlashDriveTest.cs">Copyright ©  2016</copyright>
using System;
using LocalDataBase.FlashDrive;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalDataBase.FlashDrive.Tests
{
    /// <summary>This class contains parameterized unit tests for GetSetScenarioOfFlashDrive</summary>
    [PexClass(typeof(GetSetScenarioOfFlashDrive))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class GetSetScenarioOfFlashDriveTest
    {
        /// <summary>Test stub for getFilesFromFlash()</summary>
        [PexMethod]
        public string[] getFilesFromFlashTest()
        {
            string[] result = GetSetScenarioOfFlashDrive.getFilesFromFlash();
            return result;
            // TODO: add assertions to method GetSetScenarioOfFlashDriveTest.getFilesFromFlashTest()
        }

        /// <summary>Test stub for greateFileForBackup()</summary>
        [PexMethod]
        public void greateFileForBackupTest()
        {
            GetSetScenarioOfFlashDrive.greateFileForBackup();
            // TODO: add assertions to method GetSetScenarioOfFlashDriveTest.greateFileForBackupTest()
        }

        /// <summary>Test stub for checkFilesFromFlashForInitScenario5()</summary>
        [PexMethod]
        public bool checkFilesFromFlashForInitScenario5Test()
        {
            bool result = GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenario5();
            return result;
            // TODO: add assertions to method GetSetScenarioOfFlashDriveTest.checkFilesFromFlashForInitScenario5Test()
        }
    }
}
