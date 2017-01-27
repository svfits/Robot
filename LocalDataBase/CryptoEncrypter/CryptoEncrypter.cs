using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.CryptoEncrypter
{
  public static  class CryptoEncrypter
    {
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
    }
}
