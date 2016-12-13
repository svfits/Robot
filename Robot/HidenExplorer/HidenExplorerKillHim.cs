using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.HidenExplorer
{
  public  class HidenExplorerKillHim
    {
        /// <summary>
        ////убить эксплорер
        /// </summary>
        public static void killExplorer()
        {
            try
            {
                Process.Start("taskkill", "/im explorer.exe /f");
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("Не получилось убить explorer " + ex.ToString());
            }
        }

        /// <summary>
        /// запустить эксплорер
        /// </summary>
        public static void startExplorer()
        {
            try
            {
                //Process.Start("explorer.exe");
                var proc = new Process();
                proc.StartInfo.FileName = "C:\\Windows\\explorer.exe";
                proc.StartInfo.UseShellExecute = true;
                proc.Start();
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog("Не получилось запусить explorer " + ex.ToString());
            }
        }
    }
}
