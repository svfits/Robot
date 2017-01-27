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
    /// Interaction logic for CryptoWindow.xaml
    /// </summary>
    public partial class CryptoWindow : Window
    {
        public CryptoWindow()
        {
            InitializeComponent();
        }

        public CryptoWindow(string fileName)
        {
            InitializeComponent();
            richForCrypto.IsReadOnly = true;
        }

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {

        }      

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var cnbSelected = cmb.SelectedValue.ToString();
                //MessageBox.Show(cmb.SelectedValue);
                System.Diagnostics.Debug.WriteLine(cnbSelected);

                switch (cnbSelected)
                {
                    case "Hex":
                        System.Diagnostics.Debug.WriteLine("HEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEX");
                        Key.Visibility = Visibility.Hidden;
                        break;
                    case "Rotx":
                        System.Diagnostics.Debug.WriteLine("ROoooooooooooooooooooooooooooooooooooooooooooootx");

                      //  Key.Visibility = Visibility.Visible;
                        break;
                    case "With Key":
                        System.Diagnostics.Debug.WriteLine("Wiiiiiiiiiiiiiiiiiiiiiith Keeeeeeeeeeeeeeeeeeeeey");
                      //  Key.Visibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          
        }
            
    }
}
