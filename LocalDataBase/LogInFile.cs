﻿using System;
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
            catch(Exception ex)
            {
                return;
            }
        }
    }
}
