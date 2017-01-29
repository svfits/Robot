using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalDataBase.RandomFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.RandomFiles.Tests
{
    [TestClass()]
    public class WarningCheckFilesRandomTests
    {
        [TestMethod()]
        public void RandomFilesTest()
        {

        }

        [TestMethod()]
        public void RandomSizeTest()
        {
            WarningCheckFilesRandom.RandomSizeFile();
        }

        [TestMethod()]
        public void RandomMonthTest()
        {
            string[] dd = new string[1000];
            for (int i = 0; i <= 999; i++)
            {
                dd[i] = WarningCheckFilesRandom.RandomMonth();
            }
        }

        [TestMethod()]
        public void RandomTimeTest()
        {
            string[] dd = new string[1000];
            for (int i = 0; i <= 999; i++)
            {
                dd[i] = WarningCheckFilesRandom.RandomTime().ToString();
            }
        }

        [TestMethod()]
        public void RandomFilesTest1()
        {
            string errorFileScenario3 = WarningCheckFilesRandom.RandomFiles();
        }

        
    }
}