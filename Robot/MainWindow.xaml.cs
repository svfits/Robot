using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Ports;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Robot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm"); 
        }      

        private void button_Click(object sender, RoutedEventArgs e)
        {           

        }      

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          //  e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }     

        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
