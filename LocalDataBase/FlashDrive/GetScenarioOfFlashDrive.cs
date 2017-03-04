using Robot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LocalDataBase.FlashDrive
{
    /// <summary>
    ////класс для работы с флешками
    /// </summary>
    public  class GetSetScenarioOfFlashDrive
    {
         /// <summary>
         /// файлы на флешке
         /// </summary>
     public static List<string> binCheck = new List<string>()
        {                   
            "ss274.bin",
            "sa232.bin",
            "sw244.bin",
            "sb210.bin",
            "sk212.bin",
            "san235.bin",
            "vc144.bin",
            "ac251.bin"
        };
            

      /// <summary>
      ////получение данных по сценарию с флешки
      /// </summary>
      /// <returns></returns>
      public  static int getNameFlashisAlive()
        {            
            string fileNameKernel = "RobotKernel.bin";
         //   return 1;
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
            string[] files = new string[1000];
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
                string[] dirs = Directory.GetFiles(getPathToFlashAliens());

                foreach(var rr in dirs)
                {
                   string ff = Path.GetFileName(rr);
                    if (ff == nameFile)
                    {
                        return true;
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
        ///  Удаление файлов с флешки
        /// </summary>
        public static void deleteFilesFromFlash()
        {
            string[] dirs = Directory.GetFiles(getPathToFlashAliens());

            foreach (string dir in dirs)
            {
                File.Delete(dir);
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
                        string[] dirs = Directory.GetFiles(getPathToFlashAliens());

                        foreach (string dir in dirs)
                        {
                            files.Add(Path.GetFileName(dir));                           
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
                        string[] dirs = Directory.GetFiles(getPathToFlashAliens());

                        foreach (string dir in dirs)
                        {
                            files.Add(Path.GetFileName(dir));
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
            string path = getPathToFlashAliens();

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
            string path = Path.GetDirectoryName(@"C:\robot_comand\");
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
        public static string getPathToFlashRobotKernel()
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
        ////получить путь до флешки та которая не ядро
        /// </summary>
        /// <returns></returns>
        public static string getPathToFlashAliens()
        {
            string fileNameKernel = "RobotKernel.bin";
            Boolean whetherThereisaFile = false;

            try
            {
                foreach (var dinfo in DriveInfo.GetDrives())
                {
                    if (dinfo.DriveType == DriveType.Removable && dinfo.IsReady == true)
                    {
                        string[] dirs = Directory.GetFiles(dinfo.Name);

                        if(dirs == null || dirs.Length == 0)
                        {
                            return dinfo.ToString();
                        }

                        foreach (string dir in dirs)
                        {
                            if (Path.GetFileName(dir) == fileNameKernel)
                            {
                                whetherThereisaFile = true;
                                break;
                            }
                            else
                            {
                                whetherThereisaFile = false;
                            }
                        }

                        if (!whetherThereisaFile)
                        {
                            return Path.GetDirectoryName(dirs[0]);
                        }                      
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("ошибка при  получении пути к другой флешке " + ex.ToString());
                return String.Empty;
            }
        }

        /// <summary>
        /// получение списка файлов не из ядра (без файла kernelRobot)
        /// </summary>
        /// <returns></returns>
        public static List<string> getFilesFromFlashAliens()
        {
            List<string> lstFiles = new List<string>();
            try
            {
                var getFileName = Directory.GetFiles(getPathToFlashAliens());
                //return getFileName;
                foreach(string file in getFileName )
                {
                    lstFiles.Add( Path.GetFileName(file));                    
                }

                return lstFiles;
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("не удалось получить список файлов с флешки без ядра" + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// получить сценарий из Статус Робота
        /// </summary>
        /// <returns></returns>
        public static int getScenarioApplyNotapplyscenario()
        {
            try
            {     
                string path = Path.GetDirectoryName(@"C:\robot_comand\");
                string pathFile = Path.Combine(path, "StatusRobot.bin");

                if (!File.Exists(pathFile))
                {
                    File.Create(pathFile);
                    return 0;
                }

                StreamReader file = new StreamReader(pathFile);
                
                string value = file.ReadLine();
                file.Close();
                return Convert.ToInt32(value);
            }
            catch(Exception ex )
            {
                LogInFile.addFileLog(DateTime.Now + " ошибка при  получении данных во временном файле выполнения задания " + ex.ToString());
                return 0;
            }
           
        }

        /// <summary>
        /// Получить из файла
        /// </summary>
        /// <returns></returns>
        public static string getPassword()
        {
            try
            {
                string path = Path.GetDirectoryName(@"C:\robot_comand\");
                string pathFile = Path.Combine(path, "password");

                if (!File.Exists(pathFile))
                {
                   // File.Create(pathFile);

                    using (StreamWriter sw = new StreamWriter(pathFile, false, Encoding.Default))
                    {
                        sw.WriteLine("111777");
                        return "111777";
                    }
                }

                StreamReader file = new StreamReader(pathFile);

                string value = file.ReadLine().Trim();
                file.Close();
                return value;
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog(DateTime.Now + " ошибка при  получении данных во временном файле выполнения задания " + ex.ToString());
                return null;
            }

        }

        /// <summary>
        /// Get password for crypto
        /// </summary>
        /// <returns></returns>
        public static string getPasswordForCrypto()
        {
            try
            {
                string path = Path.GetDirectoryName(@"C:\robot_comand\");
                string pathFile = Path.Combine(path, "unblock_pass");

                if (!File.Exists(pathFile))
                {
                    // File.Create(pathFile);

                    using (StreamWriter sw = new StreamWriter(pathFile, false, Encoding.Default))
                    {
                        sw.WriteLine("111777");
                        return "111777";
                    }
                }

                StreamReader file = new StreamReader(pathFile);

                string value = file.ReadLine().Trim();
                file.Close();
                return value;
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog(DateTime.Now + " get password for unblock_pass crypto " + ex.ToString());
                return null;
            }

        }


        /// <summary>
        ////получить содержимое файла по его имени на флешке 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string[] getFileContents(string fileName)
        {
            string pathFile = Path.Combine(getPathToFlashAliens(),fileName);

            string[] readText = File.ReadAllLines(pathFile, Encoding.GetEncoding("windows-1251"));

            //foreach(var ss in readText)
            //{
            //   if(ss.Contains("#hex"))
            //    {
            //        ss.Replace("#hex","");
            //    }
            //} 

            //readText = readText.Select( a => a.Replace("#hex","")).ToArray();

            return readText;
        }
    }
}
