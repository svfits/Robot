using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalDataBase.FlashDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.FlashDrive.Tests
{
    [TestClass()]
    public class GetSetScenarioOfFlashDriveTests
    {
        [TestMethod()]
        public void getFilesFromFlashTest()
        {
            var ttt = GetSetScenarioOfFlashDrive.getPathToFlashRobotKernel();
        }

        [TestMethod()]
        public void getPathToFlashAliensTest()
        {
            var ttt = GetSetScenarioOfFlashDrive.getPathToFlashAliens();
        }

        [TestMethod()]
        public void getFilesFromFlashAliensTest()
        {
            var ttt = GetSetScenarioOfFlashDrive.getFilesFromFlashAliens();
        }

        [TestMethod()]
        public void getFileContentsTest()
        {
            string[] contentFile = GetSetScenarioOfFlashDrive.getFileContents("doc.txt");

        }
    }
}