using LocalDataBase.LocalDbSQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Robot
{
    /// <summary>
    /// Interaction logic for AddNewCommand.xaml
    /// </summary>
    public partial class AddNewCommandWindow : Window
    {
        HContext db = new HContext();
        ListCommand objToAdd;

        public AddNewCommandWindow()
        {
            InitializeComponent();
        }

        private void saveNewHelpCommandBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (objToAdd.id == 0)
                {
                    db.ListCommand.Add(new ListCommand
                    {
                        command = objToAdd.command,
                        monitorPrint = objToAdd.monitorPrint,
                        helpPrint = objToAdd.helpPrint,
                        scenario = objToAdd.scenario
                    });
                }

                db.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка данные могут не сохранится");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listHelpDg.ItemsSource = getDataFromListCommand();
            SizeToContent = SizeToContent.Width;
        }

        private void listHelpDg_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           // MessageBox.Show("ggggg!");
        }

        private void rejectNewHelpCommandBtn_Click(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }            

        private void listHelpDg_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
          
        }

        private void listHelpDg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            objToAdd = listHelpDg.SelectedItem as ListCommand;
        }

        public List<ListCommand> getDataFromListCommand()
        {
           return db.ListCommand.AsEnumerable().ToList();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            db.Dispose();
        }
    }
    
}
