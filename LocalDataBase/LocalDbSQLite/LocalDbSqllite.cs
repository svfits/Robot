using LocalDataBase.LocalDbSQLite;
using Robot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDataBase
{
    public class LocalDaBase
    {
        public static void Create_Table_Events()
        {
            try
            {
                string baseNamePath = GetConnectionStringByName("SQLiteS");
                string baseName = baseNamePath.Replace("data source=|DataDirectory|", "");

                if (!File.Exists(baseName))
                {
                    SQLiteConnection.CreateFile(baseName);

                    SQLiteFactory factory = (SQLiteFactory)DbProviderFactories.GetFactory("System.Data.SQLite");
                    using (SQLiteConnection connection = (SQLiteConnection)factory.CreateConnection())
                    {
                        connection.ConnectionString = baseNamePath;
                        connection.Open();

                        using (SQLiteCommand command = new SQLiteCommand(connection))
                        {
                            //таблица событий 
                            command.CommandText = @"CREATE TABLE [ListCommands] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [command] char(1000) ,
                    [helpPrint] char(1000) ,                 
                    [monitorPrint] char(1000),
                    [scenario] integer   
                    );";
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();

                            addNewHelp();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("создание фала базы данных  " + ex.ToString());
            }
        }
        /// <summary>
        ////добавим справку
        /// </summary>
        private static void addNewHelp()
        {

            List<ListCommand> listCommand = new List<ListCommand>()
            {         
                new ListCommand { command = "Clear" , helpPrint = " ", monitorPrint = "Очистить экран консоли и буфер" },
                new ListCommand { command = "Diag, Diag ?" , helpPrint =  "Помощь по команде. Список всех доступных опций", monitorPrint = "Помощь по команде. Список всех доступных опций" },
                new ListCommand { command = "?" , helpPrint =  "help []", monitorPrint = "Help список всех доступных команд" },
                new ListCommand { command = "Diag all" , helpPrint =  "Диагностика верхнего уровня. Диагностируются блоки: communication, neuro, system, servo, modules", monitorPrint = "Помощь по команде. Список всех доступных опций" },
                new ListCommand { command = "Diag neuro" , helpPrint =  "Детальная диагностика каждого модуля в блоке", monitorPrint = "" },
                new ListCommand { command = "Diag system" , helpPrint =  "Детальная диагностика каждого модуля в блоке", monitorPrint = "" },
                new ListCommand { command = "Diag servo" , helpPrint =  "Детальная диагностика каждого модуля в блоке", monitorPrint = "" },
                new ListCommand { command = "Diag modules" , helpPrint =  "Детальная диагностика каждого модуля в блоке", monitorPrint = "" },
                new ListCommand { command = "uname -a" , helpPrint =  "Показать версию ядра", monitorPrint = "" },
                new ListCommand { command = "lsmod" , helpPrint =  "Список всех модулей", monitorPrint = "" },
                new ListCommand { command = "top" , helpPrint =  "Полная информация о системе", monitorPrint = "" },
                new ListCommand { command = "ls" , helpPrint =  "Выводит список файлов на внешнем накопителе. Если накопитель не вставлен – соответствующее сообщение.", monitorPrint = "" },
                new ListCommand { command = "make modules" , helpPrint =  "Компиляция модуля (работает только под sudo)", monitorPrint = "" },
                new ListCommand { command = "Make module install []" , helpPrint =  "[cp1600-sys-v15-7-16.bin][ns230.bin][vc144.bin][ac251.bin][ss274.bin][sa232.bin][sw244.bin][sb210.bin][sk212.bin][san235.bin]", monitorPrint = "" },
                new ListCommand { command = "sudo" , helpPrint =  "Права администратора", monitorPrint = "" },
                new ListCommand { command = "cpav" , helpPrint =  "Антивирус (работает только под sudo)", monitorPrint = "" },
                new ListCommand { command = "save" , helpPrint =  "Сохранить изменения", monitorPrint = "" },
                new ListCommand { command = "reboot" , helpPrint =  "перезагрузить систему", monitorPrint = "" },
                new ListCommand { command = "backup" , helpPrint =  "Создание на флешке резервного образа системы.", monitorPrint = "" },
                new ListCommand { command = "init" , helpPrint =  "Полная установка ПО на робота. Робот должен быть «пустой».", monitorPrint = "init robot"                
                }
            };

           
            try
            {
                using (HContext db = new HContext())
                {
                    db.ListCommand.AddRange(listCommand);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                LogInFile.addFileLog("заполнение таблицы  " + ex.ToString());
            }
            return;
            
        }

        public static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
            {
                returnValue = settings.ConnectionString;
            }
            return returnValue;
        }
      
    }
}


