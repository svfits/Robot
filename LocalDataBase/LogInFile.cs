using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
 public class LogInFile
    {
        public LogInFile() 
        {
           
        }

        /// <summary>
        ////запись логов в файл
        /// </summary>
        /// <param name="txt"></param>
        public static void addFileLog(string txt)
        {
            try
            {
               string path = Directory.GetCurrentDirectory();
               string pathFile = Path.Combine(path, "log.txt");

                using (StreamWriter sw = new StreamWriter(pathFile, true, Encoding.Default))
                {
                    sw.WriteLineAsync(txt + "  " + DateTime.Now.ToString());
                }
            }
            catch
            {
                return;
            }
        }
    }
}
