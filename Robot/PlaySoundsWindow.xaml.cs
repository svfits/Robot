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
        string _muzik;

        public PlaySoundsWindow(string idSounds)
        {            
            InitializeComponent();
            _muzik = idSounds;
         
#if DEBUG
#else
            Topmost = true;
#endif
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            try
            {
                this.Title = "Music Player  " + _muzik;
                my_media.Source = new Uri(_muzik);
                my_media.Play();                           

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmd_play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                my_media.Play();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmd_stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                my_media.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmd_pause_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                my_media.Pause();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmd_resume_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                my_media.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
