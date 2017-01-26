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

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {

        }      

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string cnbSelected = cmb.SelectedValue.ToString();

            switch (cnbSelected)
            {
                case "Hex":
                    RotxTbc.Visibility = Visibility.Collapsed;
                    WithKeyTBC.Visibility = Visibility.Collapsed;
                    break;
                case "Rotx":
                    RotxTbc.Visibility = Visibility.Visible;
                    WithKeyTBC.Visibility = Visibility.Collapsed;
                    break;
                case "With Key":
                    RotxTbc.Visibility = Visibility.Collapsed;
                    WithKeyTBC.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }
    }
}
