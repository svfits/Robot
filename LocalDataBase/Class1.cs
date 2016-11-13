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
                        command.CommandText = @"CREATE TABLE [Events] (
                    [ID] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [message] char(1000) NOT NULL,
                    [type_id] int NOT NULL,                   
                    TIMESTAMP not null default CURRENT_TIMESTAMP,
                    [source] char(1000),   
                    FOREIGN KEY(type_id) REFERENCES Type_Exception(type_id)                                    
                    );";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        //таблица типов исключений
                        command.CommandText = @"CREATE TABLE [Type_Exception] (
                    [type_id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [type_events] char(1000) NOT NULL                                                      
                    );";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        ADD_In_Type_Exception();

                        // connection
                        command.CommandText = @"CREATE TABLE [Connections] (
                    [id]  integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [login] char(1000) NOT NULL,
                    [passwords] char(1000) NOT NULL,
                    [address]   char(1000) NOT NULL,
                    [organization_name] char(1000),
                    [id_organization] int,
                    [http_proxy] char(1000),
                    [http_proxy_port] int,
                    [if_proxy] int                                                                         
                    );";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                        command.CommandText = @"CREATE TABLE [Versions] (
                    id_version integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [client] char(1000),
                    [server] char(1000)
                    );";
                        command.CommandType = CommandType.Text;
                        command.ExecuteNonQuery();

                    }
                }
            }
        }

        /// <summary>
        ////добавим перечень исключений котрые могут возникнуть
        /// </summary>
        private static void ADD_In_Type_Exception()
        {
            /*
            List<Type_Exception> type_events = new List<Type_Exception>()
            {
                new Type_Exception { type_events = "На стороне сервера не удалось получить список устройств",type_id = 1},
                new Type_Exception { type_events = "Успешно выполенена отправка результатов на сервер" , type_id =2},
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

            try
            {
                using (HContext db = new HContext())
                {
                    db.Type_Exception.AddRange(type_events);

                    db.SaveChanges();
                }
            }
            catch
            { }
            return;
            */
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


