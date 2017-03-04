using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalDataBase.FlashDrive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.FlashDrive.Tests
{
    /// <summary>
    /// для тестирования флешек
    /// </summary>
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
            string ttt = GetSetScenarioOfFlashDrive.getPathToFlashAliens();
            string ttt1 = GetSetScenarioOfFlashDrive.getPathToFlashAliens();
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

        [TestMethod()]
        public void checkFilesFromFlashForInitScenario5Test()
        {
            GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenario5();
        }

        [TestMethod()]
        public void greateFileForBackupTest()
        {
            GetSetScenarioOfFlashDrive.greateFileForBackup();
        }

        [TestMethod()]
        public void getScenarioApplyNotapplyscenarioTest()
        {
            if ((GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario() == null) || (GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario() != 199))
            {
                var scenarioDiagnosticRobot = GetSetScenarioOfFlashDrive.getNameFlashisAlive();
            }
        }

        [TestMethod()]
        public void checkFilesFromFlashTest()
        {
            if (GetSetScenarioOfFlashDrive.checkFilesFromFlash("ns230.bin"))
            {
                string fff = "ghjghj";
            }
            else
            {
                string fff = "ghjghjjjjjj";
            }
        }

        [TestMethod()]
        public void deleteFilesFromFlashTest()
        {
            GetSetScenarioOfFlashDrive.deleteFilesFromFlash();
        }

        [TestMethod()]
        public void saveScenariyTest()
        {
            GetSetScenarioOfFlashDrive.saveScenariy("199");
        }

        [TestMethod()]
        public void getPasswordTest()
        {
            GetSetScenarioOfFlashDrive.getPassword();
        }

        [TestMethod()]
        public void getPasswordForCryptoTest()
        {
            GetSetScenarioOfFlashDrive.getPasswordForCrypto();
        }
    }
}