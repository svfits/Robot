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

        [TestMethod()]
        public void hexToStringTest()
        {
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.hexToString("48656C6C6F20576F726C6421");
        }

        [TestMethod()]
        public void stringToWithKeyTest()
        {
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.stringToWithKey("Hellow Wordld", "Key");
        }

        [TestMethod()]
        public void withKeytoStringTest()
        {
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.withKeytoString("TBEn0vR643OcIy8nSjSCaQ==", "Key");
        }
    }
}