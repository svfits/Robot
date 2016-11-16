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
        public AddNewCommandWindow()
        {
            InitializeComponent();
        }

        private void saveNewHelpCommandBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HContext db = new HContext())
                {
                    db.ListCommand.Add(new ListCommand()
                    {
                        command = commandTxb.Text,
                        helpPrint = tooltipHelpTxtb.Text,
                        monitorPrint = listingCommandTxb.Text
                    }
                        );
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listHelpDg.ItemsSource = RepositoryLocalSQLite.getDataFromListCommand();

        }
    }
}
