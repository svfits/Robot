using LocalDataBase.FlashDrive;
using LocalDataBase.LocalDbSQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        Timer timerGetDataFlashDrive = new Timer();
        public int scenarioDiagnosticRobot;
        private Boolean connectNotConnect;
        
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

            searchFlashDriveandScenarioGet();
        }

        private void searchFlashDriveandScenarioGet()
        {
            timerGetDataFlashDrive.Elapsed += setScenarioDiagnosticRobot;
            timerGetDataFlashDrive.Interval = 5000;
            timerGetDataFlashDrive.Start();
        }

        private void setScenarioDiagnosticRobot(object sender, ElapsedEventArgs e)
        {
            scenarioDiagnosticRobot = GetScenarioOfFlashDrive.getNameFlashisAlive();

            // не известный робот поставим картинку
            if (scenarioDiagnosticRobot == 4)
            {
                try
                {
                    robotImage.Source = new BitmapImage(new Uri("ImageFonts/UncRobot.png", UriKind.Relative));
                }
                catch
                { }
            }

            // usb не подключено
            if(scenarioDiagnosticRobot == 0)
            {
                try
                {
                    //LINK
                    //dateTimeLbl.Dispatcher.Invoke(new Action(delegate { dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); }));
                    statusConnectionLbl.Dispatcher.Invoke(new Action(delegate { statusConnectionLbl.Content = "OFF"; }));
                    statusConnectionLbl.Dispatcher.Invoke(new Action(delegate { statusConnectionLbl.Foreground = Brushes.Black; }) );
                    //statusConnectionLbl.Content = "OFF";
                    //statusConnectionLbl.Foreground = Brushes.Gray;

                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Content = "N/A"; }));
                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Foreground = Brushes.Black; }) );
                    //modeLbl.Content = "N/A";
                    //modeLbl.Foreground = Brushes.Gray;

                    connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = false; }));
                    //connectBtn.IsEnabled = false;
                    connectNotConnect = false;

                    timerBatttery.Stop();                  
                    statusBataryLbl.Dispatcher.Invoke(new Action(delegate { statusBataryLbl.Content = "N/A"; }));
                    statusBataryLbl.Dispatcher.Invoke(new Action(delegate { statusBataryLbl.Foreground = Brushes.Black; }));

                    //connectOrDisconnectLbl.Content = "CONNECTED";
                    //connectOrDisconnectLbl.Foreground = Brushes.Green;
                    connectOrDisconnectLbl.Dispatcher.Invoke(new Action(delegate { connectOrDisconnectLbl.Content = "DISCONNECT"; }));
                    connectOrDisconnectLbl.Dispatcher.Invoke(new Action(delegate { connectOrDisconnectLbl.Foreground = Brushes.Black ; }));

                    //versionProgrammLbl.Foreground = Brushes.Green;
                    //versionProgrammLbl.Content = "v.15.7.16";
                    versionProgrammLbl.Dispatcher.Invoke(new Action(delegate { versionProgrammLbl.Content = "N/A"; }));
                    versionProgrammLbl.Dispatcher.Invoke(new Action(delegate { versionProgrammLbl.Foreground = Brushes.Black; }));

                    richTextBox.Dispatcher.Invoke(new Action(delegate { richTextBox.Document.Blocks.Clear(); }));

                }
                catch
                { }
            }

            // usb подключено 
            if(scenarioDiagnosticRobot > 0)
            {
                try
                {
                    //LINK
                    //  statusConnectionLbl.Content = "ON";
                    //  statusConnectionLbl.Foreground = Brushes.Green;
                    statusConnectionLbl.Dispatcher.Invoke(new Action(delegate { statusConnectionLbl.Content = "ON"; }));
                    statusConnectionLbl.Dispatcher.Invoke(new Action(delegate { statusConnectionLbl.Foreground = Brushes.Green; }));

                    // modeLbl.Content = "Standby";
                    // modeLbl.Foreground = Brushes.Gray;
                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Content = "Standby"; }));
                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Foreground = Brushes.Green; }));

                    // connectBtn.IsEnabled = true;
                    connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = true; }));
                }
                catch
                { }
            }
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

            addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
            connectNotConnect = true;
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

        private void richTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //press key enter
            if (e.Key == System.Windows.Input.Key.Enter)
            { 
                if(scenarioDiagnosticRobot == 0)
                {
                    addTextToRich("Робот не найден, подключите его к USB", Brushes.Red,true);
                    printHelpCommand("Робот не найден, подключите его к USB");
                    return;
                }

                if(connectNotConnect == false)
                {
                    addTextToRich("Инициализация робота не выполнена", Brushes.Red, true);
                    printHelpCommand("Инициализация робота не выполнена");
                    return;
                }
                 
                // строка которую получили из консоли
                string str = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;   
                // команда уже в правильном виде             
                string command =  getEndLine(str);

                if(command == "")
                {
                    addTextToRich("", Brushes.Red, true);
                    return;
                }

                List<ListCommand> nameCommand = RepositoryLocalSQLite.searchCommandFromBD(command, scenarioDiagnosticRobot);
                
                if(nameCommand == null)
                {
                    //command not found
              
                   // Brushes color = Brushes.Red;
                    addTextToRich("команда не найдена", Brushes.Red,true);

                    printHelpCommand("команда не найдена");
                    return;
                }
 
                 printHelpCommand(nameCommand);
                 addTextToRich(nameCommand,Brushes.White);
               
            }

            //press key up
            if(e.Key == System.Windows.Input.Key.Up)
            {
                richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
                string lastCommand = searchLastCommand();
                addTextToRich(lastCommand,Brushes.LightGreen,false);
            }

        }

        private void printHelpCommand(string v)
        {
            logTXB.Text = v;
        }

        private void printHelpCommand(List<ListCommand> nameCommand)
        {
            string str = nameCommand.FirstOrDefault().helpPrint;
            logTXB.Text = str;
        }

        private string searchLastCommand()
        {
            string str = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

            string lineend = "";
            str = str.ToLower().Trim();
           // str = str.Substring(1);

            foreach (string line in str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (line.Length > 1)
                {                    
                    if( line.IndexOf("#") != -1)
                    {

                        lineend = line.Substring(1);
                    }
                }
            }
                       
            return lineend;
        }   

        /// <summary>
        /// разберем обьект на строки и выведем их в консоль
        /// </summary>
        /// <param name="nameCommand">обьект</param>
        /// <param name="color">цвет строки</param>
        private void addTextToRich(List<ListCommand> nameCommand, SolidColorBrush color)
        {
            if(nameCommand.Count == 0 || nameCommand.FirstOrDefault().monitorPrint == null)
            {
                addTextToRich("", Brushes.White, true);
                return;
            }

            // nameCommand.FirstOrDefault().monitorPrint.Length
            foreach (string line in nameCommand.FirstOrDefault().monitorPrint.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (line.Length > 0)
                {
                    if (line.IndexOf("#RED") != -1)
                    {
                      string txt = line.Substring(4);
                      addTextToRich(txt, Brushes.Red, false);
                        addTexttoModules();
                    }
                    else
                    {
                      addTextToRich(line, color, false);
                        addTexttoModules();
                    }
                  
                }
            }

            addTextToRich("", Brushes.White,true);
        }

        /// <summary>
        /// получим случайную строку и выведем ее в модули 
        /// </summary>
        private void addTexttoModules()
        {
            Random r = new Random();            
            string str = RepositoryLocalSQLite.getStringForModules(r);
            string str1 = RepositoryLocalSQLite.getStringForModules(r);
            string str2 = RepositoryLocalSQLite.getStringForModules(r);
            string str3 = RepositoryLocalSQLite.getStringForModules(r);
            string str4 = RepositoryLocalSQLite.getStringForModules(r);
              
            conclusionInModulesCommunicationTXB(str);
            conclusionInModulesModulesTXB(str1);
            conclusionInModulesNeuroTXB(str2);
            conclusionInModulesServoTXB(str3);
            conclusionInModulesSystemTXB(str4);
        }

        /// <summary>
        /// вывод в консоль данных
        /// </summary>
        /// <param name="v">строка которую надо вывести</param>
        /// <param name="color">цвет строки</param>
        /// <param name="printLattice">надо ли в конце вывести знак решетки</param>
        private void addTextToRich(string v, SolidColorBrush color,Boolean printLattice)
        {

            TextRange range = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            //range.Text = v + Environment.NewLine;
            range.Text = v + Environment.NewLine;

            range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
         //   range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);

            if (printLattice)
            {
                setLigthGreenR();
            }
        }

        /// <summary>
        /// вывести в консоль # и покрасить ее в светло зеленый
        /// </summary>
        private void setLigthGreenR()
        {
            //Paragraph myParagraph = new Paragraph();
            //myParagraph.Inlines.Add(new Run("Some paragraph text."));

            //// Create a FlowDocument and add the paragraph to it.
            //FlowDocument myFlowDocument = new FlowDocument();
            //myFlowDocument.Blocks.Add(myParagraph);

            //richTextBox.DataContext = myFlowDocument;

            TextRange rangeR = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            rangeR.Text = "#";

            rangeR.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.LightGreen);            
            //caret to end pozion
            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
        }

        /// <summary>
        ////поиск последней команды
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string getEndLine(string str)
        {
            string lineend = "";
            str = str.ToLower().Trim();
            str = str.Substring(1);
                     
            foreach (string line in str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (line.Length > 0)
                {
                    lineend = line.Substring(1);                    
                }
            }

            return lineend;
        }
             
        private  void beeper()
        {
            SystemSounds.Beep.Play();
        }

        #region работа модулей

        private void conclusionInModulesCommunicationTXB(string str)
        {          
            CommunicationTXB.Text += str + Environment.NewLine;
            CommunicationTXB.SelectionStart = CommunicationTXB.Text.Length;
            CommunicationTXB.ScrollToEnd();
        }

        private void conclusionInModulesNeuroTXB(string str)
        {            
            NeuroTXB.Text += str + Environment.NewLine;
            NeuroTXB.SelectionStart = NeuroTXB.Text.Length;
            NeuroTXB.ScrollToEnd();
        }

        private void conclusionInModulesModulesTXB(string str)
        {           
            ModulesTXB.Text += str + Environment.NewLine;
            ModulesTXB.SelectionStart = ModulesTXB.Text.Length;
            ModulesTXB.ScrollToEnd();
        }

        private void conclusionInModulesSystemTXB(string str)
        {           
            SystemTXB.Text += str + Environment.NewLine;
            SystemTXB.SelectionStart = SystemTXB.Text.Length;
            SystemTXB.ScrollToEnd();
        }

        private void conclusionInModulesServoTXB(string str)
        {           
            ServoTXB.Text += str + Environment.NewLine;
            ServoTXB.SelectionStart = ServoTXB.Text.Length;
            ServoTXB.ScrollToEnd();
        }            

        #endregion работа модулей
    }
}
