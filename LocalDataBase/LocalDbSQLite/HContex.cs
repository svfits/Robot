using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.LocalDbSQLite
{
    public class HContext : DbContext
    { 
        public HContext() : base("SQLiteS") { }

        public DbSet<ListCommand> ListCommand { get; set; }      
    }
}
