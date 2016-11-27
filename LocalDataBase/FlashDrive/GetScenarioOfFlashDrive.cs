using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.FlashDrive
{
    /// <summary>
    ////класс для работы с флешками
    /// </summary>
  public  class GetScenarioOfFlashDrive
    {
      /// <summary>
      ////получение данных по сценарию с флешки
      /// </summary>
      /// <returns></returns>
      public  static int getNameFlashisAlive()
        {
            string fileNameKernel = "RobotKernel.bin";

            try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        foreach (string dir in dirs)
                        {
                            if (Path.GetFileName(dir) == fileNameKernel)
                            {
                                string line;
                                StreamReader file = new StreamReader(dir);

                                while ((line = file.ReadLine()) != null)
                                {
                                    return Convert.ToInt32(line);
                                }
                                file.Close();
                            }
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

      public static string[] getFilesFromFlash()
        {
            string[] files = new string[100];
            int i = 0;
            try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        foreach (string dir in dirs)
                        {
                            files[i] = Path.GetFileName(dir);
                            i++;
                        }
                        return files;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

    }
}
