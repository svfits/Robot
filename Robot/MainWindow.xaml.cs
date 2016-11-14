using LocalDataBase.LocalDbSQLite;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Media;

namespace Robot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int batteryCharge;
        System.Timers.Timer timerBatttery = new System.Timers.Timer();
        System.Timers.Timer timerDateTime = new System.Timers.Timer();

        public MainWindow()
        {
            InitializeComponent();
            dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); 
        }   
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          //  e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {           
            dateTimeUpdate();

            string datadb = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
            AppDomain.CurrentDomain.SetData("DataDirectory", datadb);
        }

        private void dateTimeUpdate()
        {
            timerDateTime.Elapsed += printDateTime;
            timerDateTime.Interval = 1000;
            timerDateTime.Start();
        }

        private void printDateTime(object sender, ElapsedEventArgs e)
        {           
            dateTimeLbl.Dispatcher.Invoke(new Action(delegate { dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); }));
        }     

        /// <summary>
        /// зарядим батарейку
        /// </summary>
        private void randomBatteryCharge()
        {
            Random r = new Random();
            batteryCharge = r.Next(50,90);
            
            timerBatttery.Elapsed += printBatteryCharge;
            timerBatttery.Interval = 600000;
            timerBatttery.Start();
        }

        private void printBatteryCharge(object sender, ElapsedEventArgs e)
        {
            batteryCharge++;
            
            if(batteryCharge >= 100)
            {
                timerBatttery.Stop();
            }            
            statusBataryLbl.Dispatcher.Invoke(new Action(delegate { statusBataryLbl.Content = batteryCharge; }));
        }

        /// <summary>
        /// при подключении USB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToUSB_Click(object sender, RoutedEventArgs e)
        {
            statusConnectionLbl.Content = "ON";
            statusConnectionLbl.Foreground = Brushes.Green;
            modeLbl.Content = "Standby";
            modeLbl.Foreground = Brushes.Gray;
            connectBtn.IsEnabled = true;


        }

        /// <summary>
        ///  при нажатии кнопки connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            randomBatteryCharge();
            statusBataryLbl.Content = batteryCharge;
            modeLbl.Content = "Prog";
            modeLbl.Foreground = Brushes.Green;

            versionProgrammLbl.Foreground = Brushes.Green;
            versionProgrammLbl.Content = "v.15.7.16";

            connectOrDisconnectLbl.Content = "CONNECTED";
            connectOrDisconnectLbl.Foreground = Brushes.Green;

            try
            {
                LocalDataBase.LocalDaBase.Create_Table_Events();

                using (HContext db = new HContext())
                {
                     db.ListCommand.Find();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}
