using Microsoft.VisualStudio.TestTools.UnitTesting;
using LocalDataBase.CryptoEncrypter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.CryptoEncrypter.Tests
{
    [TestClass()]
    public class CryptoEncrypterTests
    {
        [TestMethod()]
        public void stringToHExTest()
        {
         string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.stringToHEx("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq");
        }
    }
}