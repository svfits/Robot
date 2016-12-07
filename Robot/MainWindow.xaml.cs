using LocalDataBase.FlashDrive;
using LocalDataBase.LocalDbSQLite;
using LocalDataBase.RandomFiles;
using System;
using System.Collections.Generic;
using System.IO;
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
        string errorFileScenario3;

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

        public MainWindow()
        {
            InitializeComponent();
            dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); 
        }   
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(closeNotCloseWindowd)
            {
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
            // проверим вылечен ли робот статус флешки 
            if(GetScenarioOfFlashDrive.getNameFlashisAlive() == 0 || scenarioDiagnosticRobot != 199)
            {    
              scenarioDiagnosticRobot = GetScenarioOfFlashDrive.getNameFlashisAlive();
            }            

            // не известный робот поставим картинку
             if (scenarioDiagnosticRobot == 4)
            {
                try
                {
                    // robotImage.Source = new BitmapImage(new Uri("ImageFonts/UncRobot.png", UriKind.Relative));
                    robotImage.Dispatcher.Invoke(new Action(
                        delegate
                        {
                            robotImage.Source = new BitmapImage(new Uri("ImageFonts/UncRobot.png", UriKind.Relative));
                        }
                    ));
                }
                catch(Exception ex)
                {
                    LogInFile.addFileLog(ex.ToString());
                }
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

                    // richTextBox.Dispatcher.Invoke(new Action(delegate { richTextBox.Document.Blocks.Clear(); }));

                    connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = false; }));

                    // остоновим модули вывод
                    timerRobotWorkPrintModules.Stop();

                    // очистим модули
                    emptyModules();

                    //очистим richtext
                    //richTextBox.Document.Blocks.Clear();
                    richTextBox.Dispatcher.Invoke(new Action(delegate { richTextBox.Document.Blocks.Clear(); }));

                    // очистить вывода подсказки об ишибках
                    logTXB.Dispatcher.Invoke(new Action ( delegate { logTXB.Clear(); }));

                    //
                    //textBoxCommands.Clear
                    textBoxCommands.Dispatcher.Invoke(new Action(delegate { textBoxCommands.Clear(); }));

                }
                catch(Exception ex)
                {
                    LogInFile.addFileLog(ex.ToString());
                }
            }

            // usb подключено 
            if(scenarioDiagnosticRobot > 0 && scenarioDiagnosticRobot != 4 )
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
                    if (!connectNotConnect)
                    {
                        connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = true; }));
                    }             
                }
                catch(Exception ex)
                {
                    LogInFile.addFileLog(ex.ToString());
                }
            
            }

            // неизвестный робот
           if(scenarioDiagnosticRobot == 4)
            {
                connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = true; }));
            }
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
            LogInFile.addFileLog("Нажатие кнопки");
            string path = Directory.GetCurrentDirectory();
            string pathFile = Path.Combine(path, "log.txt");
            MessageBox.Show(pathFile);
            try
            {
                if (scenarioDiagnosticRobot != 4 && scenarioDiagnosticRobot != 5)
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

                    connectBtn.IsEnabled = false;
                }

                if (scenarioDiagnosticRobot == 4)
                {
                    addTextToRich("Connected not known robot can not recognize", Brushes.Red, false);
                    printHelpCommand("Connected not known robot can not recognize");
                    return;
                }

                if (scenarioDiagnosticRobot == 5)
                {
                    connectNotConnect = true;
                    addTextToRich("Connected robot without software", Brushes.Red, false);
                    printHelpCommand("Connected not known robot can not recognize");
                    return;
                }

                // сценарий ошибка в одном из файлов
                if (scenarioDiagnosticRobot == 3)
                {
                    errorFileScenario3 = WarningCheckFilesRandom.RandomFiles();
                }
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog(ex.ToString());
                return;
            }
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
        /// ввод команды в консоль
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // строка которую получили из консоли
            string command = textBoxCommands.Text.Trim();
                  
            //press key enter
            if (e.Key == System.Windows.Input.Key.Enter)
            { 
                if(scenarioDiagnosticRobot == 0)
                {
                    addTextToRich("Робот не найден, подключите его к USB", Brushes.Red,false);
                    printHelpCommand("Робот не найден, подключите его к USB");
                    beeper();
                    return;
                }

                if(connectNotConnect == false)
                {
                    addTextToRich("Инициализация робота не выполнена", Brushes.Red, false);
                    printHelpCommand("Инициализация робота не выполнена");
                    beeper();
                    return;
                }

                if(scenarioDiagnosticRobot == 4)
                {
                    addTextToRich("Connected not known robot can not recognize", Brushes.Red, false);                   
                    printHelpCommand("Connected not known robot can not recognize");
                    beeper();
                    return;
                } 

                if(command.Length == 0)
                {
                    textBoxCommands.Clear();
                    textBoxSuffix.Text = "#";
                    addTextToRich("#", Brushes.LightGreen, false);
                    return;
                }

                List<ListCommand> nameCommand = null;

                // подверждение команды
                if (textBoxSuffix.Text.Length > 1)               
                {
                    //backup 2 проверка на замену файлов
                    if (textBoxSuffix.Text == "Flash drive is not empty, all data will delete?" && command == "yes")
                    {
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("backup", scenarioDiagnosticRobot);
                        textBoxSuffix.Text = "#";

                    }
                    else if (textBoxSuffix.Text == "Proceed with save?" && command == "yes")
                    {
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("save", scenarioDiagnosticRobot);
                        textBoxSuffix.Text = "#";
                    }
                    else if(textBoxSuffix.Text.Trim() == "Password:")
                    {
                        if (command == RepositoryLocalSQLite.searchCommandFromBD("password111q!!!", scenarioDiagnosticRobot).FirstOrDefault().helpPrint)
                        {
                            addTextToRich("Root rights successfully", Brushes.LightGreen, false);
                            printHelpCommand("Root rights successfully");
                            textBoxCommands.Clear();
                            textBoxSuffix.Text = "#";
                            sudoNotsudo = true;
                            return;
                        }
                        else
                        {
                            addTextToRich("Authentication failure. Sorry, try again", Brushes.Red, false);
                            printHelpCommand("Authentication failure. Sorry, try again");
                            textBoxCommands.Clear();
                            textBoxSuffix.Text = "#";
                            sudoNotsudo = false;
                            beeper();
                            return;
                        }
                    }
                    else if (textBoxSuffix.Text.Trim() == "Proceed with reboot?" && command == "yes")
                        {
                        // reboot yes
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("reboot", scenarioDiagnosticRobot);
                        textBoxSuffix.Text = "#";
                    }
                    else
                    {
                        textBoxSuffix.Text = "#";
                        textBoxCommands.Clear();
                        return;
                    }
                }
                else
                {
                    nameCommand = RepositoryLocalSQLite.searchCommandFromBD(command, scenarioDiagnosticRobot);
                }
                              
                if(nameCommand == null)
                {                  
                    addTextToRich(command + ":    " + "команда не найдена", Brushes.Red,false);
                    printHelpCommand("команда не найдена");
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }

                    addTextToRich("#" + command, Brushes.LightGreen, false);
                    textBoxCommands.Clear();
                 

                #region команды
                // очистка консоли
                if (nameCommand.FirstOrDefault().command == "clear")
                {
                    richTextBox.Document.Blocks.Clear();
                    addTextToRich("", Brushes.Green, false);
                    return;
                }
                //диагностика
                if(nameCommand.FirstOrDefault().command == "diag all")
                {
                    colorizeModule(scenarioDiagnosticRobot, Brushes.Red);
                }

                // установка ПО
                if(nameCommand.FirstOrDefault().command == "init robot" && scenarioDiagnosticRobot == 5)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!");
                        beeper();
                        return;
                    }

                    if (GetScenarioOfFlashDrive.checkFilesFromFlashForInitScenario5())
                    {
                        // все стало хорошо ОС установлена
                        scenarioDiagnosticRobot = 199;

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
                    else
                    {
                        addTextToRich("Init flash drive not detect", Brushes.Red, false);
                        printHelpCommand("Init flash drive not detect");
                        beeper();
                        return;
                    }
                }

                if(nameCommand.FirstOrDefault().command == "cpav scan" && scenarioDiagnosticRobot == 1)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!");
                        beeper();
                        return;
                    }

                    colorizeModule(scenarioDiagnosticRobot, Brushes.Green);
                    printInModulesDateTimer();
                    scenarioDiagnosticRobot = 199;
                }

                if (nameCommand.FirstOrDefault().command == "cpav scan" && scenarioDiagnosticRobot == 2)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!");
                        beeper();
                        return;
                    }

                    colorizeModule(scenarioDiagnosticRobot, Brushes.Green);
                    printInModulesDateTimer();
                    scenarioDiagnosticRobot = 199;
                }

                if ((nameCommand.FirstOrDefault().command == "make module install ns230.bin") && scenarioDiagnosticRobot == 2)
                {
                    if (GetScenarioOfFlashDrive.checkFilesFromFlash("ns230.bin"))
                    {
                        if (sudoNotsudo == true)
                        {
                            colorizeModule(scenarioDiagnosticRobot, Brushes.Green);
                            printInModulesDateTimer();
                            scenarioDiagnosticRobot = 199;
                        }
                        else
                        {
                            addTextToRich("Only root!", Brushes.Red, false);
                            printHelpCommand("Only root!");
                            beeper();
                            return;
                        }
                    }
                    else
                    {
                        addTextToRich("Ошибка ненайден файл ns230.bin", Brushes.Red, false);
                        printHelpCommand("Ошибка ненайден файл ns230.bin");
                        beeper();
                        return;
                    }
                }

                if(scenarioDiagnosticRobot == 3 && nameCommand.FirstOrDefault().command == "diag servo")
                {
                    addTextToRich("Servo modules FAIL " + errorFileScenario3, Brushes.Red, false);
                }

                if (scenarioDiagnosticRobot == 3 && parsingCompareString("make modules install") == true && parsingCompareString(errorFileScenario3) )
                {
                    if(sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!");
                        beeper();
                        return;
                    }
                    // переустановлен модуль сбойный
                    colorizeModule(scenarioDiagnosticRobot, Brushes.Black);
                    printInModulesDateTimer();
                    scenarioDiagnosticRobot = 199;
                }
                else if(scenarioDiagnosticRobot == 3 && parsingCompareString("make modules install") == true && parsingCompareString(errorFileScenario3) != true)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!");
                        beeper();
                        return;
                    }
                    addTextToRich("module not found", Brushes.Red, false);
                    printHelpCommand("Модуль для сборки не найден");
                    beeper();
                    return;
                }

                if(nameCommand.FirstOrDefault().command == "ls" && scenarioDiagnosticRobot != 5)
                {
                    string[] files = GetScenarioOfFlashDrive.getFilesFromFlash();
                    addTextToRich(files, Brushes.White);
                }

                // команда buckup
                if(nameCommand.FirstOrDefault().command == "backup" && GetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == true
                    && searchLastCommand() != "yes" )
                {
                    //addTextToRich("Flash drive is not empty, all data will delete?", Brushes.Red, false);
                    textBoxSuffix.Text = "Flash drive is not empty, all data will delete?";
                    printHelpCommand("Flash drive is not empty, all data will delete?");
                    return;
                }
                
                if(nameCommand.FirstOrDefault().command == "save" && searchLastCommand() != "yes")
                {
                    // addTextToRich("Proceed with save?", Brushes.Red, false);
                    textBoxSuffix.Text = "Proceed with save?";
                    printHelpCommand("Proceed with save?");
                    return;
                } 
                
                 if(nameCommand.FirstOrDefault().command == "sudo" && textBoxSuffix.Text != "Password:" )
                   {
                    textBoxSuffix.Text = "Password:";
                    printHelpCommand("Password: ");
                    return;
                   }  
                 
                 if(nameCommand.FirstOrDefault().command == "reboot" && textBoxSuffix.Text != "Proceed with reboot?" && searchLastCommand() != "yes")
                  {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!");
                        beeper();
                        return;
                    }

                    textBoxSuffix.Text = "Proceed with reboot?";
                    printHelpCommand("Proceed with reboot?");
                    return;
                }         
                           
                //вывод текста команды и справки
                printHelpCommand(nameCommand);
                addTextToRich(nameCommand, Brushes.White);

                #endregion конец команд
            }

            //press key up
            if (e.Key == System.Windows.Input.Key.Up)
            {                           
                string lastCommand = searchLastCommand();
                // addTextToRich(lastCommand,Brushes.LightGreen);
                // richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
                textBoxCommands.Text = lastCommand;
                textBoxCommands.SelectionStart = textBoxCommands.Text.Length;
            }         

        }
        /// <summary>
        /// поиск ПРЕД последней каманды
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private string searchLastCommand(int v)
        {
            try
            {
                string str = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;

                List<string> lineend = new List<string>();
                str = str.ToLower().Trim();

                foreach (string line in str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
                {
                    if (line.Length > 1)
                    {
                        if (line.IndexOf("#") != -1)
                        {
                            lineend.Add(line.Substring(1));
                        }
                    }
                }
                int count = lineend.Count - 2;

                string str1 = lineend[count];
                return str1;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// проверить есть ли такая строка в команде
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Boolean parsingCompareString(string v)
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

            if(lineend.Contains(v) == true)
            {
                return true;
            }

            return false;

        }

        /// <summary>
        /// вывести в консоль
        /// </summary>
        /// <param name="files"></param>
        /// <param name="color"></param>
        private void addTextToRich(string[] files, SolidColorBrush color)
        {
            foreach(var file in files)
            {
                if(file != null)
                {
                    addTextToRich(file, color, false);
                }                
            }
           // addTextToRich("", color, true);
        }

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
                    printInModulesDateTimer();                   
                    break;

            }
        }

        private void printInModulesDateTimer()
        {           
            timerRobotWorkPrintModules.Elapsed += addTexttoModules;
            timerRobotWorkPrintModules.Interval = 1000;
            timerRobotWorkPrintModules.Start();
        }      

        private void emptyModules()
        {
            //ServoTXB.Clear();
            //CommunicationTXB.Clear();
            //NeuroTXB.Clear();
            //SystemTXB.Clear();
            //ModulesTXB.Clear();

            ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.Clear(); }));
            CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.Clear(); }));
            NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.Clear(); }));
            SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.Clear(); }));
            ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.Clear(); }));
        }

        private void addTextToRich(string v, SolidColorBrush color)
        {
            TextRange range = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
            //range.Text = v + Environment.NewLine;
            range.Text = v;
            range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
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
                addTextToRich("", Brushes.White, false);
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
                    }
                    else
                    {
                      addTextToRich(line, color, false);                     
                    }
                  
                }
            }

            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            richTextBox.ScrollToEnd();
        }
        
        /// <summary>
        /// получим случайную строку и выведем ее в модули 
        /// </summary>
        private void addTexttoModules(object sender, ElapsedEventArgs e)
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
            if (v != String.Empty)
            {
                TextRange range = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
               // range.Text = v + "\n";
                range.Text = v + Environment.NewLine;

                range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                //   range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }

            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            richTextBox.ScrollToEnd();
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
            if(str == "")
            {
                return lineend;
            }
            str = str.Substring(1);
                     
            foreach (string line in str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
            {
                if (line.Length >= 0)
                {
                    lineend = line;                    
                }
            }
            return lineend;
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

        private void Executed_New(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            // MessageBox.Show("Вызов команды 'New'");
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
    }
}
