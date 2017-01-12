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
        public void checkFilesFromFlashForInitScenarioBackupTest()
        {
            if (GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == true)
            {

                return;
            }
            else
            {
                GetSetScenarioOfFlashDrive.greateFileForBackup();
            }
        }

      
    }
}