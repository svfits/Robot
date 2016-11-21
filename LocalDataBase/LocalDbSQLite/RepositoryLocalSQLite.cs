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


        public static List<ListCommand> searchCommandFromBD(string textCommand)
        {
            try
            {
                using (HContext dbL = new HContext())
                {
                    var helpList = dbL.ListCommand
                      .AsEnumerable()
                      .Where(c => c.command.ToLower().Trim() == textCommand)
                      // .Select(a => a.monitorPrint)
                      .ToList()
                      //  .Where(a => a.id == 1) 
                      //  .ToString()                                          
                      ;
                    // return helpList; 

                    if (helpList.Count == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return helpList;
                    }
                }
            }
            catch (Exception ex)
            {
              //   MessageBox.Show(ex.ToString());
                return null;
            }
        }
    }
}
