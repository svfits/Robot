using LocalDataBase.LocalDbSQLite;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace Robot
{
    /// <summary>
    /// Interaction logic for AddNewCommand.xaml
    /// </summary>
    public partial class AddNewCommandWindow : Window
    {
        HContext db = new HContext();       
       
        public AddNewCommandWindow()
        {          
            InitializeComponent();
        }

        private void saveNewHelpCommandBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {           
                db.SaveChanges();
                listHelpDg.Items.Refresh();                           
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
                MessageBox.Show("Произошла ошибка данные могут не сохранится, текст ошибки  " + ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {        
            db.ListCommand.Load();
            listHelpDg.ItemsSource = db.ListCommand.Local;
     
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
        }       
        
        public List<ListCommand> getDataFromListCommand()
        {
            try
            {
                return db.ListCommand.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
                MessageBox.Show("база данных не доступна или сломалась  " + ex.ToString());
                return null;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {            
            this.db.Dispose();
        }

    
    }
    
}
