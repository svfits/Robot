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
        private string _command, _helpPrint, _monitorPrint;

        /// <summary>
        ////команда
        /// </summary>
        public string command
        {
            get
            {
                return _command;
            }
            set
            {
                if (_command !=value)
                {
                    string txt = value;
                    txt = txt.Trim().ToLower();
                    _command = txt;
                }
            }
        }

        /// <summary>
        ////справка по команде
        /// </summary>
        public string helpPrint
        {
            get
            {
                return _helpPrint;
            }
            set
            {
                if (_helpPrint != value)
                {
                    string txt = value;
                    txt = txt.Trim();
                    _helpPrint = txt;
                }
            }
        }

        /// <summary>
        /// вывод по команде 
        /// </summary>
        public string monitorPrint
        {
            get
            {
                return _monitorPrint;
            }
            set
            {
                if (_monitorPrint != value)
                {
                    string txt = value;
                    txt = txt.Trim();
                    _monitorPrint = txt;
                }
            }
        }

        public int?  scenario { get; set; }
    }
}
