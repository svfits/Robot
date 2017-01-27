using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.CryptoEncrypter
{
  public static  class CryptoEncrypter
    {
        /// <summary>
        ////Строка в HEx
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string stringToHEx(string txt)
        {
            char[] values = txt.ToCharArray();
            string strHex = String.Empty;

            foreach(char letter in values)
            {
                int value = Convert.ToInt32(letter);
                string hexOutput = String.Format("{0:X}", value);
                strHex += hexOutput;
            }

            return strHex;
        }

        /// <summary>
        /// Hex to string
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string hexToString(string txt)
        {
            try
            {
                byte[] raw = new byte[txt.Length / 2];
                for (int i = 0; i < raw.Length; i++)
                {
                    raw[i] = Convert.ToByte(txt.Substring(i * 2, 2), 16);
                }
                return System.Text.Encoding.UTF8.GetString(raw);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// String with Key
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static string stringToWithKey(string ishText, string pass)
        {
            try
            {
                string sol = "doberman";
                string cryptographicAlgorithm = "SHA1";
                int passIter = 2;
                string initVec = "a8doSuDitOz1hZe#";
                int keySize = 256;

                if (string.IsNullOrEmpty(ishText))
                    return "";

                byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
                byte[] solB = Encoding.ASCII.GetBytes(sol);
                byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);

                PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
                byte[] keyBytes = derivPass.GetBytes(keySize / 8);
                RijndaelManaged symmK = new RijndaelManaged();
                symmK.Mode = CipherMode.CBC;

                byte[] cipherTextBytes = null;

                using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memStream.ToArray();
                            memStream.Close();
                            cryptoStream.Close();
                        }
                    }
                }

                symmK.Clear();
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        ////Дешифрование строки
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string withKeytoString(string ciphText, string pass)
        {
            try
            {
                string sol = "doberman";
                string cryptographicAlgorithm = "SHA1";
                int passIter = 2;
                string initVec = "a8doSuDitOz1hZe#";
                int keySize = 256;

                if (string.IsNullOrEmpty(ciphText))
                    return "";

                byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
                byte[] solB = Encoding.ASCII.GetBytes(sol);
                byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

                PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
                byte[] keyBytes = derivPass.GetBytes(keySize / 8);

                RijndaelManaged symmK = new RijndaelManaged();
                symmK.Mode = CipherMode.CBC;

                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int byteCount = 0;

                using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
                {
                    using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                        {
                            byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            mSt.Close();
                            cryptoStream.Close();
                        }
                    }
                }

                symmK.Clear();
                return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
            }
            catch
            {
                return "";
            }
        }
    }
}
