using LocalDataBase.FlashDrive;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.RandomFiles
{
    /// <summary>
    //// ошибка в работе файла сценарий 3
    /// </summary>
 public static class WarningCheckFilesRandom
    {
        private static Random r = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);

        /// <summary>
        /// случайный сломанный файл для сценария 3
        /// </summary>
        /// <returns></returns>
        public static string RandomFiles()
        {
            Random r = random();            

            List<string> list = GetSetScenarioOfFlashDrive.binCheck;
            int numFile = r.Next(list.Count);
            return (string)list[numFile];

        }

        public static int RandomSizeFile()
        {
            Random r = random();
            int numBer = r.Next(10000, 99999);

            return numBer;
        }

        public static string RandomMonth()
        {
            string str = "";
            Random r = random();
            int numFile = r.Next(1, 12);

            switch (numFile)
            {
                case 1:
                    str = "January";
                    break;
                case 2:
                    str = "February";
                    break;
                case 3:
                    str = "March";
                    break;
                case 4:
                    str = "April";
                    break;
                case 5:
                    str = "May";
                    break;
                case 6:
                    str = "June";
                    break;
                case 7:
                    str = "July";
                    break;
                case 8:
                    str = "August";
                    break;
                case 9:
                    str = "September";
                    break;
                case 10:
                    str = "October";
                    break;
                case 11:
                    str = "November";
                    break;
                case 12:
                    str = "December";
                    break;
            }

            return str;
        }

        private static Random random()
        {
            RandomClass rnd = new RandomClass();
            return rnd.main();
        }

        public static  string RandomTime()
        {
            Random gen = random();
            DateTime start = new DateTime(2016, 1, 1);
            int range = (DateTime.Today - start).Days;
            CultureInfo heIL = new CultureInfo("en-US");
            return start.AddDays(gen.Next(range)).ToString("m",heIL);
        }

        public static int randomSleep(int first, int end)
        {           
            Random r = random();
            int numBer = r.Next(first, end);

            return numBer;
        }

        public static char[] randomStr(char[] nStr)
        {
            char[] nStr1 = null;

            for (int i = nStr.Length -1; i >=1; i--)
            {
                int j = r.Next(i + 3);
                var temp = nStr[j];
                nStr[j] = nStr[i];
                nStr[i] = temp;
                nStr1 = nStr;
            }

            return nStr1;
        }
    }

    public class RandomClass
    {
        Random r = new Random();

        public Random main()
        {
            return r;
        }

    }
}
