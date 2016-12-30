using LocalDataBase.FlashDrive;
using LocalDataBase.LocalDbSQLite;
using LocalDataBase.RandomFiles;
using Robot.HidenExplorer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Robot
{    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {   
        /// <summary>
        /// заряд батареи
        /// </summary>
        public int batteryCharge;
        Timer timerBatttery = new Timer();
        Timer timerDateTime = new Timer();
        Timer timerGetDataFlashDrive = new Timer();
        Timer timerRobotWorkPrintModules = new Timer();

        /// <summary>
        /// ошибка в файле для сценария 3
        /// </summary>
        public  string errorFileScenario3;

        /// <summary>
        /// номер сценария
        /// </summary>
        public int scenarioDiagnosticRobot;

        /// <summary>
        /// нажата кнопка коннект
        /// </summary>
        private Boolean connectNotConnect;

        /// <summary>
        /// был ли получен root
        /// </summary>
        private Boolean sudoNotsudo = false;
        /// <summary>
        ////закрыть или не закрыть окно
        /// </summary>
        private bool closeNotCloseWindowd;

        /// <summary>
        /// требует ли команда повтора ввода
        /// </summary>
        private Boolean x2command = false;

        FlowDocument objDoc = new FlowDocument();
        Paragraph objParag1 = new Paragraph();

        public MainWindow()
        {

#if DEBUG
            Topmost = false;
#else
            Topmost = true;
#endif

            InitializeComponent();

            animaionLoad();

            dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); 
        }   
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(closeNotCloseWindowd)
            {
#if DEBUG
#else
                          HidenExplorerKillHim.startExplorer();
#endif
                return;
            }            
            e.Cancel = true;                       
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //  richTextBox.MaxHeight =  System.Windows.SystemParameters.PrimaryScreenHeight - 60;
            richTextBox.MaxHeight = SystemParameters.WorkArea.Height - 10;
                    
            dateTimeUpdate();
            //  string datadb = System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory));
            string datadb = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
            AppDomain.CurrentDomain.SetData("DataDirectory", datadb);

            LocalDataBase.LocalDaBase.Create_Table_Events();           

            searchFlashDriveandScenarioGet();

#if DEBUG
             
#else
            HidenExplorerKillHim.killExplorer();
#endif

        }

        /// <summary>
        ////показать время 
        /// </summary>
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
            catch (Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
            }
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
        ///  при нажатии кнопки connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Connect_Click(object sender, RoutedEventArgs e)
        {
            richTextBox.Document.Blocks.Clear();
            addTextToRich("", Brushes.Green, false);
            animationLblConnect();
            objDoc = new FlowDocument();
            objParag1 = new Paragraph();

            try
            {
                if (scenarioDiagnosticRobot == 4)
                {
                    //addTextToRich("Connected not known robot can not recognize", Brushes.Red, false);
                    //printHelpCommand("Connected not known robot can not recognize",Brushes.Red);
                    printTextToHelp();
                    connectOrDisconnectLbl.Content = "CONNECTED";
                    connectOrDisconnectLbl.Foreground = Brushes.Green;
                    connectBtn.IsEnabled = false;
                    connectNotConnect = true;
                    modeLbl.Content = "Prog";
                    modeLbl.Foreground = Brushes.Green;
                    if (RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                    {
                        addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                    }
                    return;
                }

                if (scenarioDiagnosticRobot == 5)
                {                 
                    printHelpCommand("Not detect software of the Robot", Brushes.Red);

                    connectOrDisconnectLbl.Content = "CONNECTED";
                    connectOrDisconnectLbl.Foreground = Brushes.Green;
                                    
                    textBoxCommands.Focus();

                    randomBatteryCharge();
                    statusBataryLbl.Content = batteryCharge;
                    modeLbl.Content = "Prog";
                    modeLbl.Foreground = Brushes.Green;

                    versionProgrammLbl.Foreground = Brushes.Green;                  
                                      
                    if (RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                    {
                        addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                    }

                    connectNotConnect = true;

                    connectBtn.IsEnabled = false;
                    printInModulesDateTimer();
                    return;

                }

                if (scenarioDiagnosticRobot > 0)
                {
                    randomBatteryCharge();
                    statusBataryLbl.Content = batteryCharge;
                    modeLbl.Content = "Prog";
                    modeLbl.Foreground = Brushes.Green;

                    versionProgrammLbl.Foreground = Brushes.Green;
                    versionProgrammLbl.Content = "v.15.7.16";

                    connectOrDisconnectLbl.Content = "CONNECTED";
                    connectOrDisconnectLbl.Foreground = Brushes.Green;

                    if(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                    {
                        addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                    }
                   
                    connectNotConnect = true;

                    connectBtn.IsEnabled = false;
                    printInModulesDateTimer();
                                     
                }

                // сценарий ошибка в одном из файлов
                if (scenarioDiagnosticRobot == 3)
                {
                    errorFileScenario3 = WarningCheckFilesRandom.RandomFiles();                  
                }

                textBoxCommands.Focus();             
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("при нажатии кнопки конект   " + ex.ToString());
                return;
            }
        }

        /// <summary>
        /// ввод команды в консоль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      

        private void printTextToHelp()
        {
            List<ListCommand> dataUncRobot = RepositoryLocalSQLite.searchCommandFromBD("datauncrobot", scenarioDiagnosticRobot);
            if (dataUncRobot == null)
            {
                return;
            }

          //  addTextToRich(dataUncRobot.FirstOrDefault().monitorPrint, Brushes.Red, false);
            printHelpCommand(dataUncRobot.FirstOrDefault().helpPrint, Brushes.Red);

            textBoxCommands.Clear();
        }

        private void textBoxSuffixAddText(string v)
        {
            if (v.Length > 5)
            {
                textBoxSuffix.Text = v;
                return;
            }

            if(sudoNotsudo)
            {
                textBoxSuffix.Text = "root" + v;
            }
            else
            {
                textBoxSuffix.Text = v;
            }
        }

        /// <summary>
        /// проверить есть ли такая строка в команде
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Boolean parsingCompareString(string v)
        {
            try
            {
                string str = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                string lineend = "";

                str = str.ToLower().Trim();
                if (str == "")
                {
                    return false;
                }
                str = str.Substring(1);

                foreach (string line in str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
                {
                    if (line.Length >= 0)
                    {
                        lineend = line;
                    }
                }

                if (lineend.Contains(v) == true)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("поиск конектинг  " + ex.ToString());

                return false;
            }
        }      

        /// <summary>
        /// вывод сообщения об ошибке в подсказки
        /// </summary>
        /// <param name="nameCommand"></param>
        /// <param name="color"></param>
        private void printHelpCommand(List<ListCommand> nameCommand, SolidColorBrush color)
        {
            string str = nameCommand.FirstOrDefault().helpPrint;

            if(str == null || nameCommand == null)
            {
                return;
            }

            if (str.Contains("#RED"))
            {
                string txt = str.Substring(5);
                logTXB.Text = txt;
                logTXB.Foreground = Brushes.Red;
            }
            else if(str.Contains("#GREEN"))
            {
                string txt = str.Substring(7);
                logTXB.Text = txt;
                logTXB.Foreground = Brushes.LightGreen;
            }
            else if(str.Contains("#ORANGE"))
            {
                string txt = str.Substring(7);
                logTXB.Text = txt;
                logTXB.Foreground = Brushes.Orange;
            }
            else if(str.Contains("#YELLOW"))
            {
                string txt = str.Substring(8);
                logTXB.Text = txt;
                logTXB.Foreground = Brushes.Yellow;
            }
            else
            {
                logTXB.Text = str;
                logTXB.Foreground = color;
            }         
        }

        /// <summary>
        ////вывод сообщения об ошибке в подсказки
        /// </summary>
        /// <param name="v"></param>
        /// <param name="color"></param>
        private void printHelpCommand(string v, SolidColorBrush color)
        {
            logTXB.Text = v;
            logTXB.Foreground = color;
        }

        /// <summary>
        /// последняя команда
        /// </summary>
        /// <returns></returns>
        public string searchLastCommand()
        {
            string str = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

            string lineend = "";
            str = str.ToLower().Trim();
           // str = str.Substring(1);

            foreach (string line in str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (line.Length > 1)
                {                  
                    string value = line;
                    Char delimiter = '#';
                    String[] substring = value.Split(delimiter);
                    if(substring.Length > 1 && substring[1] != String.Empty)
                    {
                        lineend = substring[1];
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
        private async void addTextToRich(List<ListCommand> nameCommand, SolidColorBrush color)
        {
            if (nameCommand.Count == 0 || nameCommand.FirstOrDefault().monitorPrint == null)
            {
                addTextToRich("", Brushes.White, false);
                return;
            }

            // nameCommand.FirstOrDefault().monitorPrint.Length
            foreach (string line in nameCommand.FirstOrDefault().monitorPrint.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (line.Length > 0)
                {
                    if (line.Contains("#RED"))
                    {
                        string txt = line.Replace("#RED", "").Trim();
                        addTextToRich(txt, Brushes.Red, false);
                    }
                    else if (line.Contains("#GREEN"))
                    {
                        string txt = line.Replace("#GREEN", "").Trim();
                        addTextToRich(txt, Brushes.LightGreen, false);
                    }
                    else if (line.Contains("#ORANGE"))
                    {
                        string txt = line.Replace("#ORANGE", "").Trim();
                        addTextToRich(txt, Brushes.Orange, false);
                    }
                    else if (line.Contains("#YELLOW"))
                    {
                        string txt = line.Replace("#YELLOW", "").Trim();
                        addTextToRich(txt, Brushes.Yellow, false);
                    }
                    else
                    {
                        addTextToRich(line, color, false);
                    }

                }
                await Task.Delay(300);
            }

            Task.WaitAll();
            //вывод текста команды и справки
            printHelpCommand(nameCommand, Brushes.LightGreen);
            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            richTextBox.ScrollToEnd();
        }



        /// <summary>
        /// вывод в консоль данных
        /// </summary>
        /// <param name="v">строка которую надо вывести</param>
        /// <param name="color">цвет строки</param>
        /// <param name="printLattice">надо ли в конце вывести знак решетки</param>
        private void addTextToRich(string v, SolidColorBrush color,Boolean printLattice)
        {
            if (sudoNotsudo && printLattice)
            {
                v = "root" + v;
            }

            if (v != String.Empty)
            {
                TextRange range = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
                objParag1.Inlines.Add(new Run(v + Environment.NewLine) {Foreground = color } );
                objDoc.Blocks.Add(objParag1);             
            }

            try
            {                
                richTextBox.Dispatcher.Invoke(new Action(delegate { richTextBox.Document = objDoc; }));
                richTextBox.Dispatcher.Invoke(new Action(delegate { richTextBox.CaretPosition = richTextBox.Document.ContentEnd; }));
                richTextBox.Dispatcher.Invoke(new Action(delegate { richTextBox.ScrollToEnd(); }));
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("ошибка при добавлении консоль  " + ex.ToString());
            }
        }       
        
         
        private  void beeper()
        {
         //   SystemSounds.Beep.Play();
              SystemSounds.Asterisk.Play();
          //  SystemSounds.Exclamation.Play();
         //   SystemSounds.Question.Play();
          //  SystemSounds.Hand.Play();
        }

#region работа модулей

        /// <summary>
        /// раскрасим модули bkb yfxytv dsdjlbnm byajhvfwb. d yb[
        /// </summary>
        /// <param name="scenarioDiagnosticRobot">номер сценария</param>
        /// <param name="color">цвет</param>
        private void colorizeModule(int scenarioDiagnosticRobot, SolidColorBrush color)
        {
            switch (scenarioDiagnosticRobot)
            {
                case 1:
                    SystemTXB.Background = color;
                    break;
                case 2:
                    NeuroTXB.Background = color;
                    break;
                case 3:
                    ServoTXB.Background = color;
                    break;
                case 4:
                    ServoTXB.Background = color;
                    CommunicationTXB.Background = color;
                    NeuroTXB.Background = color;
                    SystemTXB.Background = color;
                    ModulesTXB.Background = color;
                    break;
                case 5:

                    break;
                case 199:
                    ServoTXB.Background = color;
                    CommunicationTXB.Background = color;
                    NeuroTXB.Background = color;
                    SystemTXB.Background = color;
                    ModulesTXB.Background = color;
                    break;
            }
        }

        private void printInModulesDateTimer()
        {
            timerRobotWorkPrintModules = new Timer();

            timerRobotWorkPrintModules.Elapsed += addTexttoModules;
            timerRobotWorkPrintModules.Interval = 1000;
            timerRobotWorkPrintModules.Start();
        }

        /// <summary>
        ////почистить модули вывода сделать пустыми
        /// </summary>
        private void emptyModules()
        {
            ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.Clear(); }));
            ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.Background = Brushes.Black; }));
            CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.Clear(); }));
            CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.Background = Brushes.Black; }));
            NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.Clear(); }));
            NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.Background = Brushes.Black; }));
            SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.Clear(); }));
            SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.Background = Brushes.Black; }));
            ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.Clear(); }));
            ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.Background = Brushes.Black; }));
        }

        /// <summary>
        /// получим случайную строку и выведем ее в модули 
        /// </summary>
        private void addTexttoModules(object sender, ElapsedEventArgs e)
        {
            timerRobotWorkPrintModules.Stop();

            Random r = new Random();
            int randomString = r.Next(1, 6);

            int randomTime = r.Next(200, 1500);
            timerRobotWorkPrintModules.Interval = randomTime;

            switch (randomString)
            {
                case 1:
                    string str0 = RepositoryLocalSQLite.getStringForModules(r);
                    conclusionInModulesCommunicationTXB(str0);
                    break;
                case 2:
                    string str1 = RepositoryLocalSQLite.getStringForModules(r);
                    conclusionInModulesModulesTXB(str1);
                    break;
                case 3:
                    string str2 = RepositoryLocalSQLite.getStringForModules(r);
                    conclusionInModulesNeuroTXB(str2);
                    break;
                case 4:
                    string str3 = RepositoryLocalSQLite.getStringForModules(r);
                    conclusionInModulesServoTXB(str3);
                    break;
                case 5:
                    string str4 = RepositoryLocalSQLite.getStringForModules(r);
                    conclusionInModulesSystemTXB(str4);
                    break;
            }

            timerRobotWorkPrintModules.Start();
        }

        private void conclusionInModulesCommunicationTXB(string str)
        {          
            try
            {             
                CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.Text += str + Environment.NewLine; } ) );
                CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.SelectionStart = CommunicationTXB.Text.Length; }));
                CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.ScrollToEnd(); }));
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
            }
        }

        private void conclusionInModulesNeuroTXB(string str)
        {         
            try
            {
                NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.Text += str + Environment.NewLine; }));
                NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.SelectionStart = NeuroTXB.Text.Length; }));
                NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.ScrollToEnd(); }));
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
            }
        }

        private void conclusionInModulesModulesTXB(string str)
        {
            //ModulesTXB.Text += str + Environment.NewLine;
            //ModulesTXB.SelectionStart = ModulesTXB.Text.Length;
            //ModulesTXB.ScrollToEnd();
            try
            {
                ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.Text += str + Environment.NewLine; }));
                ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.SelectionStart = ModulesTXB.Text.Length; }));
                ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.ScrollToEnd(); }));
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
            }
        }

        private void conclusionInModulesSystemTXB(string str)
        {
            //SystemTXB.Text += str + Environment.NewLine;
            //SystemTXB.SelectionStart = SystemTXB.Text.Length;
            //SystemTXB.ScrollToEnd();
            try
            {
                SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.Text += str + Environment.NewLine; }));
                SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.SelectionStart = SystemTXB.Text.Length; }));
                SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.ScrollToEnd(); }));
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
            }
        }

        private void conclusionInModulesServoTXB(string str)
        {
            //ServoTXB.Text += str + Environment.NewLine;
            //ServoTXB.SelectionStart = ServoTXB.Text.Length;
            //ServoTXB.ScrollToEnd();
            try
            {
                ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.Text += str + Environment.NewLine; }));
                ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.SelectionStart = ServoTXB.Text.Length; }));
                ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.ScrollToEnd(); }));
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
            }
        }

#endregion работа модулей

#region сочетание клавиш разное
        private void Executed_New(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
           
            Application.Current.Dispatcher.Invoke( () =>
            {
               MainWindow window3 = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (window3 != null)
                {
                    closeNotCloseWindowd = true;
                    window3.Close();
                }
            } );
        }

        private void CanExecute_New(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }       

        private void CanExecute_New_F12(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
      
        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            AddNewCommandWindow addNewHelp = new AddNewCommandWindow();
            addNewHelp.ShowDialog();
        }

#endregion сочетание клавиш на закрытие окна

        private void richTextBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBoxCommands.Focus();
        }

        private void textBoxSuffix_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBoxCommands.Focus();
        }

        private void CommunicationTXB_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            textBoxCommands.Focus();
        }
    }
}
