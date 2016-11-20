using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.LocalDbSQLite
{
    public  class ListCommand
    {
        [Key]
        public int id { get; set; }

        public string command { get; set; }

        public string helpPrint { get; set; }

        public string monitorPrint { get; set; }

        public int?  scenario { get; set; }
    }
}
