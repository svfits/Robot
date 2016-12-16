using Robot;
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
  public  class GetSetScenarioOfFlashDrive
    {
     public static List<string> binCheck = new List<string>()
        {
            "cp1600-bf-v2-5-0.bin",
            "cp1600-sys-v15-7-16.bin",
            "ns230.bin",
            "ss274.bin",
            "sa232.bin",
            "sw244.bin",
            "sb210.bin",
            "sk212.bin",
            "san235.bin"
        };
            

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
            catch(Exception ex)
            {
                LogInFile.addFileLog("ошибка при чтении конфига с флешки" + ex.ToString());
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
            catch(Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
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
            catch(Exception ex)
            {
                LogInFile.addFileLog("ошибка при проверке наличия файла на флешке  " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// проверка наличия файлов для сценария 5
        /// </summary>
        /// <returns></returns>
        public static Boolean checkFilesFromFlashForInitScenario5()
        {        

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
            catch(Exception ex)
            {
                LogInFile.addFileLog("проверка файлов на флешке для сценария 5 " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// проверка наличия файлов для backup
        /// </summary>
        /// <returns></returns>
        public static Boolean checkFilesFromFlashForInitScenarioBackup()
        {         
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
            catch(Exception ex)
            {
                LogInFile.addFileLog("ошибка при проверки файлов на флешке для БЭКАПА" + ex.ToString());
                return false;
            }
        }

        /// <summary>
        ///  создадим файлы для backup
        /// </summary>
        public static void greateFileForBackup()
        {
            string path = getPathToFlash();

            try
            {
                foreach (var ss in binCheck)
                {
                    File.Create(Path.Combine(path, ss));
                }
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog("ошибка при проверки создании файлов на флешке для БЭКАПА" + ex.ToString());
            }
        }

        /// <summary>
        /// сохранить сценарий на флешки когда выполнен
        /// </summary>
        /// <param name="text"></param>
        public static void saveScenariy(string text)
        {
            string path = getPathToFlash();
            string fileName = Path.Combine(path, "StatusRobot.bin");

            try
            {       
                if(!File.Exists(fileName))
                {
                    File.Create(fileName);
                }        
                
                using (StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("ошибка при сохранении статуса " + ex.ToString());
            }
        }

        /// <summary>
        ////получить путь до флешки
        /// </summary>
        /// <returns></returns>
        public static string getPathToFlash()
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
                                return Path.GetDirectoryName(dir);
                            }
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("ошибка при  получении списка файлов " + ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// получить сценарий с флешки
        /// </summary>
        /// <returns></returns>
        public static int? getScenarioApplyNotapplyscenario()
        {
            try
            {
                //string path = getPathToFlash();
                //string fileName = Path.Combine(path, "StatusRobot.bin");

                string path = getPathToFlash();
                string fileName = Path.Combine(path, "StatusRobot.bin");
                StreamReader file = new StreamReader(fileName);

                string value = file.ReadLine();
                file.Close();
                return Convert.ToInt32(value);
            }
            catch(Exception ex )
            {
                LogInFile.addFileLog(DateTime.Now + " ошибка при  получении данных во временном файле выполнения задания " + ex.ToString());
                return null;
            }
           
        }
    }
}
