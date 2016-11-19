using LocalDataBase.LocalDbSQLite;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Timer timerBatttery = new Timer();
        Timer timerDateTime = new Timer();
        public int scenarioDiagnosticRobot;

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
            //  string datadb = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
            string datadb = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            AppDomain.CurrentDomain.SetData("DataDirectory", datadb);

            LocalDataBase.LocalDaBase.Create_Table_Events();
        }

        private void dateTimeUpdate()
        {
            timerDateTime.Elapsed += printDateTime;
            timerDateTime.Interval = 1000;
            timerDateTime.Start();
        }

        private void printDateTime(object sender, ElapsedEventArgs e)
        {
            try
            {
                dateTimeLbl.Dispatcher.Invoke(new Action(delegate { dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); }));
            }
            catch
            { }
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

        /// <summary>
        /// зарада больше 100 не бывает нарисуем зарядку
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            selectRegimWorkRobot();
        }

        private void selectRegimWorkRobot()
        {
            Random r = new Random();
            scenarioDiagnosticRobot = r.Next(1, 6);
            System.Diagnostics.Debug.WriteLine(scenarioDiagnosticRobot);
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
        }

        /// <summary>
        /// редактирование списка команд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F12)
            {
                AddNewCommandWindow addNewHelp = new AddNewCommandWindow();
                addNewHelp.Show();
            }
        }

        /// <summary>
        /// обработка нажатия enter при вводе команд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commandTXB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
           if(e.Key == System.Windows.Input.Key.Enter)
            {
                //сколько строк 
                int countLineTxb = commandTXB.LineCount;
                string textLineTXB = commandTXB.GetLineText(countLineTxb - 2);

                textLineTXB =  textLineTXB.Trim();
                textLineTXB = textLineTXB.ToLower();
                if(textLineTXB != "")
                {
                    textLineTXB = textLineTXB.Substring(1);
                }

                if(RepositoryLocalSQLite.searchCommandFromBD(textLineTXB) == null)
                {
                    commandTXB.Foreground = Brushes.Blue;
                    commandTXB.Text += "команда не найдена" + Environment.NewLine;
                }
                else
                {
                    commandTXB.Foreground = Brushes.Blue;
                    commandTXB.Text += RepositoryLocalSQLite.searchCommandFromBD(textLineTXB).First().monitorPrint + Environment.NewLine;
                }

                commandTXB.Foreground = Brushes.Green;
                commandTXB.Text += "#";
              
                commandTXB.SelectionStart = commandTXB.Text.Length;

                //commandTXB.Text = searchCommand(textLineTXB);
                //commandTXB.Text = "fffffffffffff";
                //MessageBox.Show(textLineTXB.ToString());
            }
        }

       
    }
}
