using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.RandomFiles
{
 public static class WarningCheckFilesRandom
    {
        public static string RandomFiles()
        {
            string str = "";
            Random r = new Random();
            int numFile = r.Next(1, 9);
            
            switch (numFile)
            {
                case 1:
                    str = "vc144.bin";
                    break;
                case 2:
                    str = "ac251.bin";
                    break;
                case 3:
                    str = "ss274.bin";
                    break;
                case 4:
                    str = "sa232.bin";
                    break;
                case 5:
                    str = "sw244.bin";
                    break;
                case 6:
                    str = "sb210.bin";
                    break;
                case 7:
                    str = "sk212.bin";
                    break;
                case 8:
                    str = "san235.bin";
                    break;
            }

            return str;
        }
    }
}
