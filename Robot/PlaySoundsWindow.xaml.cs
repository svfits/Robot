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
    /// Interaction logic for PlaySoundsWindow.xaml
    /// </summary>
    public partial class PlaySoundsWindow : Window
    {
        public PlaySoundsWindow(string idSounds)
        {            
            InitializeComponent();
            MessageBox.Show(idSounds);
        }
    }
}
