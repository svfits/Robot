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
                new ListCommand { command = "Clear" , helpPrint = " ", monitorPrint = "Очистить экран консоли и буфер" },
                new ListCommand { command = "Diag, Diag ?" , helpPrint =  "Помощь по команде. Список всех доступных опций", monitorPrint = "Помощь по команде. Список всех доступных опций"
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


