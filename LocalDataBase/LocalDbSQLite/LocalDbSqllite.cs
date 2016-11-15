using LocalDataBase.LocalDbSQLite;
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
                    [monitorPrint] char(1000)   
                    );";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        addNewHelp();
                    }
                }
            }
        }
        /// <summary>
        ////добавим справку
        /// </summary>
        private static void addNewHelp()
        {

            List<ListCommand> listCommand = new List<ListCommand>()
            {
                new ListCommand { command = "?", helpPrint = "Help список всех доступных команд",
                    monitorPrint = " CP Interactive Operating System Software (CPIOSS)" +
                    "PIOSS (tm) 1600 ANDROID Software (C), Version 15.7.16," +
                    "Copyright (c) 2065-2073 by CP Systems, Corp." +
                    "These shell commands are defined internally.  Type `help' to see this list." +
                    "Type `help name' or ‘name ?’ to find out more about the function `name'." +
                    "                                                                        " +
                    "clear                       - clear the terminal screen."   +
                    "cpav scan                   - scan, find and cure viruses." +
                    "diag [node]                 - check all nodes in the system how it’s work." +
                    "ls                      - list directory contents on flash drive" +
                    "lsmod                       - list all modules in the system." +
                    "make modules install [module_name]      - replaces the selected module in the system. A new" +
                    "                         module is taken from flash drive." +
                   "top                     - print system information."
                } ,

                new ListCommand { command = "Clear" , helpPrint = " ", monitorPrint = "Очистить экран консоли и буфер" },
                new ListCommand { command = "Diag, Diag ?" ,
                    helpPrint =  "Помощь по команде. Список всех доступных опций",
                    monitorPrint = "Помощь по команде. Список всех доступных опций"
                },
                /*
                new Type_Exception { type_events = "Возникла ошибка  при отправке результатов на сервер" , type_id =3},
                new Type_Exception { type_events = "Возникла ошибка при выполнении POST запроса при отправке результатов на сервер", type_id =4},
                new Type_Exception { type_events = "Удачно отправляет результаты поиска устройств на сервер" , type_id =5},
                new Type_Exception { type_events = "Возникла ошибка при отправке результатов поиска устройств на сервер", type_id =6},
                new Type_Exception { type_events = "Ошибка при POST запросе при отправке результатов поиска устройств на сервер" , type_id =7},
                new Type_Exception { type_events = "Ошибка при вызове сервером сканирования устройств" , type_id = 8},
                new Type_Exception { type_events = "Удачно подключился к серверу" , type_id = 9},
                new Type_Exception { type_events = "При старте сервиса не удалось подключится" , type_id = 10},
                new Type_Exception { type_events = "Произошла ошибка определения параметров подключения" , type_id = 11 },
                new Type_Exception { type_events = "Ошибка при сканировании устройств" , type_id = 12 },
                new Type_Exception { type_events = "Удаленный сервер возвратил ошибку: (404) Не найден.", type_id = 13 },
                new Type_Exception { type_events = "Продолжаем реконтектится" , type_id = 14 },
                new Type_Exception { type_events = "Подключение пропало", type_id = 15 },
                new Type_Exception { type_events = "Ошибка обновления, при получении номера версии", type_id = 16 },
                new Type_Exception { type_events = "Произошла ошибка определения параметров подключения, нет параметров ", type_id = 17 },
                new Type_Exception { type_events = "Ошибка при остановке сервиса BPSCCollector", type_id = 18 },
                new Type_Exception { type_events = "Ошибка при запуске сервиса BPSCCollector", type_id = 19 },
                new Type_Exception { type_events = "Обновление прошло удачно", type_id = 20 },
                new Type_Exception { type_events = "Обновление прошло с ошибками сервис не смог запустится, откатим изменения", type_id = 21 },
                new Type_Exception { type_events = "Обновление сервиса начато на версию", type_id = 22 },
                new Type_Exception { type_events = "Не удалось получить файл обновления с сервера обновлений", type_id = 23 },
            };
*/
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


