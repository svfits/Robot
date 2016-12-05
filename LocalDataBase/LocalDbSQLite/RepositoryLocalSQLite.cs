using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.LocalDbSQLite
{
    public static class RepositoryLocalSQLite
    {
        public static List<ListCommand> getDataFromListCommand()
        {
            try
            {
                using (HContext db = new HContext())
                {
                    return db.ListCommand
                        .AsEnumerable()
                        .ToList()
                        ;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// поиск команды и описание к ней
        /// </summary>
        /// <param name="textCommand"></param>
        /// <param name="scenarioDiagnosticRobot"></param>
        /// <param name="lastCommand"></param>
        /// <returns></returns>
        public static List<ListCommand> searchCommandFromBD(string textCommand,int scenarioDiagnosticRobot)
        {           
            try
            {
                using (HContext dbL = new HContext())
                {                 
                    if(textCommand.Contains("make modules install") && scenarioDiagnosticRobot == 3)
                    {
                        var makeModulesInstall = dbL.ListCommand
                    .AsEnumerable()
                    .Where(c => c.command.ToLower().Trim() == "make modules install" && c.scenario == scenarioDiagnosticRobot)
                    .ToList()
                    ;
                        return makeModulesInstall;
                    }

                      var helpList = dbL.ListCommand
                      .AsEnumerable()
                      .Where(c => c.command.ToLower().Trim() == textCommand && c.scenario == scenarioDiagnosticRobot)
                      .ToList()
                      ;

                    if(helpList != null && helpList.Count != 0)
                    {
                        return helpList;
                    }

                    helpList = dbL.ListCommand
                        .AsEnumerable()
                        .Where(a => a.command.ToLower().Trim() == textCommand )
                        .ToList()
                        ;

                    if (helpList.Count == 0 || helpList == null)
                    {
                        return null;
                    }
                    else
                    {
                        return helpList;
                    }
                }
            }
            catch
            {             
                return null;
            }
        }

        public static List<ListCommand> serachCOnnecting(int scenarioDiagnosticRobot)
        {
            try
            {
                using (HContext dbL = new HContext())
                {
                    var connection = dbL.ListCommand
                      .AsEnumerable()
                      .Where(c => c.command.ToLower().Trim() == "connecting" && c.scenario == scenarioDiagnosticRobot)
                      .ToList()
                      ;
                    return connection;
                }
               
            }
            catch
            {
                return null;
            }
        }

       
        /// <summary>
        ////вывод в модули тестотовой информации
        /// </summary>
        /// <returns></returns>
        public static string getStringForModules(Random r)
        {            
            int  randomString = r.Next(1, 9);
            string str;

            switch (randomString)
            {
                case 1:
                    str = "Module Size Used by";
                    break;
                case 2:
                    str = "fuse 91981 3";
                    break;
                case 3:
                    str = "w1_therm 4319 0";
                    break;
                case 4:
                    str = "w1_gpio 4295 0" + "wire 31227 2 w1_gpio,w1_therm";
                    break;
                case 5:
                    str = "snd_soc_wm8804 8413 1 snd_soc_wm8804_i2c";
                    break;
                case 6:
                    str = "snd_soc_pcm512x 18073 1 snd_soc_pcm512x_i2c";
                    break;
                case 7:
                    str = "regmap_i2c 3346 3 snd_soc_pcm512x_i2c,snd_soc_wm8804_i2c,snd_soc_tas5713";
                    break;
                case 8:
                    str = "oc_core,snd_pcm_dmaengine";
                    break;
                default:
                    str = "snd_seq_device";
                    break;
            }

            return str;
        }
    }
}
