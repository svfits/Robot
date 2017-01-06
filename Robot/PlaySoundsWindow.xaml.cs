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
            //robotImage.Source = new BitmapImage(new Uri("ImageFonts/UncRobot.png", UriKind.Relative));
            //my_media.Source = new Uri("Sounds/trrr.mp3",UriKind.Relative);
            // MessageBox.Show(idSounds);
            //Source="Sounds/trrr.mp3"
         //   Topmost = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Source = "Sounds/trrr.mp3"
            try
            {
                // my_media.Source = new Uri("Sounds/trrr.mp3", UriKind.Relative);
                my_media.Source = new Uri(_muzik);
                my_media.Play();
                            
               //MediaPlayer.MediaPlayer.startMediaPlayer(_muzik);
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
