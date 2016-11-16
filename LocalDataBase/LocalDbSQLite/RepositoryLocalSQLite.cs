using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.LocalDbSQLite
{
  public static  class RepositoryLocalSQLite
    {
        public static List<ListCommand> getDataFromListCommand()
        {
            try
            {
                using (HContext db = new HContext())
                {
                    return  db.ListCommand
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
    }
}
