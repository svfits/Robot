using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase.LocalDbSQLite
{
    public  class ListCommand : INotifyPropertyChanged
    {
        [Key]
        public int id { get; set; }
        private string _command, _helpPrint, _monitorPrint;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ////команда
        /// </summary>
        public string command
        {
            get
            {  return _command; }
            set
            {
                string txt = value;
                txt = txt.Trim().ToLower();
                _command = txt;
                OnPropertyChanged("command");
            }
        }

        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
                _helpPrint = value;
                OnPropertyChanged("helpPrint");
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
                _monitorPrint = value;
                OnPropertyChanged("monitorPrint");
            }
        }

        public int?  scenario { get; set; }
    }
}
