using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Resources;

namespace Robot.MediaPlayer
{
  public static  class MediaPlayer
    {
        public static void startMediaPlayer(string res)
        {
            try
            {
                Process.Start(@"C:\Program Files (x86)\Windows Media Player\wmplayer.exe",  res);
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("Не получилось плеер " + ex.ToString());
            }
        }
               
    }
}
