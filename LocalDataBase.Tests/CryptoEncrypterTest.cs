using System;
using LocalDataBase.CryptoEncrypter;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LocalDataBase.CryptoEncrypter.Tests
{
    /// <summary>This class contains parameterized unit tests for CryptoEncrypter</summary>
    [TestClass]
    [PexClass(typeof(global::LocalDataBase.CryptoEncrypter.CryptoEncrypter))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class CryptoEncrypterTest
    {

        /// <summary>Test stub for hexToString(String)</summary>
        [PexMethod]
        public string hexToStringTest(string txt)
        {
            string result = global::LocalDataBase.CryptoEncrypter.CryptoEncrypter.hexToString(txt);
            return result;
            // TODO: add assertions to method CryptoEncrypterTest.hexToStringTest(String)
        }
    }
}
