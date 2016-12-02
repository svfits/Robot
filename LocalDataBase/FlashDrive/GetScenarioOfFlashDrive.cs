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

        /// <summary>
        /// получить список файлов с флешки
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        ////если файл на флешке
        /// </summary>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public static Boolean checkFilesFromFlash(string nameFile)
        {          
           try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        foreach (string dir in dirs)
                        {
                            if (Path.GetFileName(dir) == nameFile)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// проверка наличия файлов для сценария 5
        /// </summary>
        /// <returns></returns>
        public static Boolean checkFilesFromFlashForInitScenario5()
        {
            List<string> binCheck = new List<string>();
            binCheck.Add("cp1600-bf-v2-5-0.bin");
            binCheck.Add("cp1600-sys-v15-7-16.bin");
            binCheck.Add("ns230.bin");
            binCheck.Add("ss274.bin");
            binCheck.Add("sa232.bin");
            binCheck.Add("sw244.bin");
            binCheck.Add("sb210.bin");
            binCheck.Add("sk212.bin");
            binCheck.Add("san235.bin");

            List<string> files = new List<string>();
            
            try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        foreach (string dir in dirs)
                        {
                            files.Add(Path.GetFileName(dir));                           
                        }
                       
                    }
                }

                List<string> resultList = binCheck.Where(t => files.Contains(t)).ToList<string>();
                if(binCheck.Count == resultList.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
        }

        /// <summary>
        /// проверка наличия файлов для backup
        /// </summary>
        /// <returns></returns>
        public static Boolean checkFilesFromFlashForInitScenarioBackup()
        {
            List<string> binCheck = new List<string>();
            binCheck.Add("cp1600-bf-v2-5-0.bin");
            binCheck.Add("cp1600-sys-v15-7-16.bin");
            binCheck.Add("ns230.bin");
            binCheck.Add("ss274.bin");
            binCheck.Add("sa232.bin");
            binCheck.Add("sw244.bin");
            binCheck.Add("sb210.bin");
            binCheck.Add("sk212.bin");
            binCheck.Add("san235.bin");

            List<string> files = new List<string>();

            try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        foreach (string dir in dirs)
                        {
                            files.Add(Path.GetFileName(dir));
                        }

                    }
                }

                List<string> resultList = binCheck.Where(t => files.Contains(t)).ToList();
                if ( resultList.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
        }


    }
}
