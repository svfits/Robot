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

        /// <summary>
        /// требует ли команда повтора ввода
        /// </summary>
        private Boolean x2command = false;

        public MainWindow()
        {
           // Topmost = true;
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
            timerGetDataFlashDrive.Stop();
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
                    LogInFile.addFileLog("не известный робот его картинка " + ex.ToString());
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
                    connectOrDisconnectLbl.Dispatcher.Invoke(new Action(delegate { connectOrDisconnectLbl.Foreground = Brushes.Red; }));

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

                    sudoNotsudo = false;

                    textBoxSuffix.Dispatcher.Invoke(new Action(delegate { textBoxSuffix.Text = "#"; }));
                }
                catch(Exception ex)
                {
                    LogInFile.addFileLog("юсб не подключено   " + ex.ToString());
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


                    // logTXB.Text = "Robot ready for programming. Please use console.";
                 
                    if (!connectNotConnect)
                    {
                        connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = true; }));
                        logTXB.Dispatcher.Invoke(new Action(delegate { logTXB.Text = "Robot ready for programming. Please use console."; }));
                    }
                             
                }
                catch(Exception ex)
                {
                    LogInFile.addFileLog("юсб подключено  " + ex.ToString());
                }
            
            }

            // неизвестный робот
           if(scenarioDiagnosticRobot == 4)
            {
                connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = true; }));
            }

            timerGetDataFlashDrive.Start();
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

                    if(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                    {
                        addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                    }
                   
                    connectNotConnect = true;

                    connectBtn.IsEnabled = false;
                    printInModulesDateTimer();
                }

                if (scenarioDiagnosticRobot == 4)
                {
                    addTextToRich("Connected not known robot can not recognize", Brushes.Red, false);
                    printHelpCommand("Connected not known robot can not recognize",Brushes.Red);
                    return;
                }

                if (scenarioDiagnosticRobot == 5)
                {
                    connectNotConnect = true;
                    addTextToRich("Connected robot without software", Brushes.Red, false);
                    printHelpCommand("Connected not known robot can not recognize",Brushes.Red);
                    return;
                }

                // сценарий ошибка в одном из файлов
                if (scenarioDiagnosticRobot == 3)
                {
                    errorFileScenario3 = WarningCheckFilesRandom.RandomFiles();
                    printInModulesDateTimer();
                }
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("при нажатии кнопки конект   " + ex.ToString());
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
                  //  addTextToRich("Робот не найден, подключите его к USB", Brushes.Red,false);
                    printHelpCommand("Робот не найден, подключите его к USB",Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }

                if(connectNotConnect == false)
                {
                  //  addTextToRich("Инициализация робота не выполнена", Brushes.Red, false);
                    printHelpCommand("НЕТ СОЕДИНЕНИЯ С РОБОТОМ",Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }

                if(scenarioDiagnosticRobot == 4)
                {
                 //   addTextToRich("Connected not known robot can not recognize", Brushes.Red, false);                   
                    printHelpCommand("Connected not known robot can not recognize", Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                } 

                if(command.Length == 0)
                {
                    textBoxCommands.Clear();                  
                    textBoxSuffixAddText("#");
                    addTextToRich("#", Brushes.LightGreen, true);
                    return;
                }

                List<ListCommand> nameCommand = null;

                // подверждение команды
                if (x2command)               
                {
                    #region подверждение команд
                    
                    //backup 2 проверка на замену файлов
                    if (textBoxSuffix.Text == "Flash drive is not empty, all data will delete?" && command == "yes")
                    {
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("backup", scenarioDiagnosticRobot);
                        textBoxSuffixAddText("#");
                        x2command = false;
                    }

                   if (textBoxSuffix.Text == "Proceed with save?" && command == "yes")
                    {
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("save", scenarioDiagnosticRobot);
                        textBoxSuffixAddText("#");
                        x2command = false;
                    }

                    if(textBoxSuffix.Text.Trim() == "Password:")
                    {
                        if (command == RepositoryLocalSQLite.searchCommandFromBD("password111q!!!", scenarioDiagnosticRobot).FirstOrDefault().helpPrint)
                        {
                            sudoNotsudo = true;
                            addTextToRich("Root rights successfully", Brushes.LightGreen, false);
                            printHelpCommand("Root rights successfully",Brushes.LightGreen);
                            textBoxCommands.Clear();
                            textBoxSuffixAddText("#");
                            x2command = false;
                            return;
                        }
                        else
                        {
                            addTextToRich("Authentication failure. Sorry, try again", Brushes.Red, false);
                            printHelpCommand("Authentication failure. Sorry, try again",Brushes.Red);
                            textBoxCommands.Clear();
                            textBoxSuffixAddText("#");
                            sudoNotsudo = false;
                            beeper();
                            x2command = false;
                            return;
                        }
                    }


                   if (textBoxSuffix.Text.Trim() == "Proceed with reboot?" && command == "yes")
                    {
                        // reboot yes
                        richTextBox.Document.Blocks.Clear();
                        addTextToRich("", Brushes.Green, false);

                        if (RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                        {
                            addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                        }
                        sudoNotsudo = false;
                        textBoxSuffixAddText("#");
                        textBoxCommands.Clear();
                        x2command = false;
                        return;
                    }
                    else if (textBoxSuffix.Text.Trim() == "Proceed with reboot?")
                    {
                        textBoxSuffixAddText("#");
                        textBoxCommands.Clear();
                        x2command = false;
                        return;
                    }

                    if (nameCommand == null)
                    {
                        textBoxSuffixAddText("#");
                        printHelpCommand("unindentified command. Please use YES or NO", Brushes.Red);
                        textBoxCommands.Clear();
                        x2command = false;
                        return;
                    }

                    if (command == "no")
                    {
                        printHelpCommand("team suspended", Brushes.Red);
                        x2command = false;
                        return;
                    }
                    else if (command != "yes")
                    {
                        printHelpCommand("unindentified command. Please use YES or NO", Brushes.LightGreen);
                        x2command = false;
                        return;
                    }

                    x2command = false;
                    textBoxCommands.Clear();
                    //вывод текста команды и справки
                    printHelpCommand(nameCommand, Brushes.LightGreen);
                    addTextToRich(nameCommand, Brushes.White);
                    return;

                    #endregion подверждение команд
                }
                else
                {
                    nameCommand = RepositoryLocalSQLite.searchCommandFromBD(command, scenarioDiagnosticRobot);
                }
                              
                if(nameCommand == null)
                {                  
                    addTextToRich(command + ":    " + "команда не найдена", Brushes.Red,false);
                    printHelpCommand("команда не найдена", Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }

                    addTextToRich("# " + command, Brushes.LightGreen, false);
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
                        printHelpCommand("Only root!",Brushes.Red);
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

                        if (RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                        {
                            addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                        }

                        connectNotConnect = true;
                    }
                    else
                    {
                        addTextToRich("Init flash drive not detect", Brushes.Red, false);
                        printHelpCommand("Init flash drive not detect",Brushes.Red);
                        beeper();
                        return;
                    }
                }

                if(nameCommand.FirstOrDefault().command == "cpav scan" && scenarioDiagnosticRobot == 1)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!",Brushes.Red);
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
                        printHelpCommand("Only root!",Brushes.Red);
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
                            printHelpCommand("Only root!", Brushes.Red);
                            beeper();
                            return;
                        }
                    }
                    else
                    {
                        addTextToRich("Ошибка ненайден файл ns230.bin", Brushes.Red, false);
                        printHelpCommand("Ошибка ненайден файл ns230.bin",Brushes.Red);
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
                        printHelpCommand("Only root!",Brushes.Red);
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
                        printHelpCommand("Only root!",Brushes.Red);
                        beeper();
                        return;
                    }
                    addTextToRich("module not found", Brushes.Red, false);
                    printHelpCommand("Модуль для сборки не найден",Brushes.Red);
                    beeper();
                    return;
                }             

                // команда buckup
                if(nameCommand.FirstOrDefault().command == "backup" && GetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == true
                    && searchLastCommand() != "yes" )
                {
                    //addTextToRich("Flash drive is not empty, all data will delete?", Brushes.Red, false);
                    // textBoxSuffix.Text = "Flash drive is not empty, all data will delete?";
                    textBoxSuffixAddText("Flash drive is not empty, all data will delete?");
                    printHelpCommand("Flash drive is not empty, all data will delete?",Brushes.Red);
                    x2command = true;
                    return;
                }
                else if(GetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == false && nameCommand.FirstOrDefault().command == "backup"
                    && searchLastCommand() != "yes" )
                {
                    GetScenarioOfFlashDrive.greateFileForBackup();
                }
                
                if(nameCommand.FirstOrDefault().command == "save" && searchLastCommand() != "yes")
                {
                    // addTextToRich("Proceed with save?", Brushes.Red, false);
                   // textBoxSuffix.Text = "Proceed with save?";
                    textBoxSuffixAddText("Proceed with save?");
                    printHelpCommand("Proceed with save?",Brushes.Red);
                    x2command = true;
                    return;
                } 
                
                 if(nameCommand.FirstOrDefault().command == "sudo" && textBoxSuffix.Text != "Password:" )
                   {
                    //textBoxSuffix.Text = "Password: ";
                    textBoxSuffixAddText("Password: ");
                    printHelpCommand("Password: ",Brushes.Red);
                    x2command = true;
                    return;
                   }  
                 
                 if(nameCommand.FirstOrDefault().command == "reboot" && textBoxSuffix.Text != "Proceed with reboot?" && searchLastCommand() != "yes")
                  {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!",Brushes.Red);
                        beeper();
                        return;
                    }

                   // textBoxSuffix.Text = "Proceed with reboot?";
                    textBoxSuffixAddText("Proceed with reboot? ");
                    printHelpCommand("Proceed with reboot?",Brushes.LightGreen);
                    x2command = true;
                    return;
                }         
                           
                //вывод текста команды и справки
                printHelpCommand(nameCommand, Brushes.LightGreen);
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

        /// <summary>
        ////почистить модули вывода сделать пустыми
        /// </summary>
        private void emptyModules()
        {       
            ServoTXB.Dispatcher.Invoke(new Action(delegate { ServoTXB.Clear(); }));
            CommunicationTXB.Dispatcher.Invoke(new Action(delegate { CommunicationTXB.Clear(); }));
            NeuroTXB.Dispatcher.Invoke(new Action(delegate { NeuroTXB.Clear(); }));
            SystemTXB.Dispatcher.Invoke(new Action(delegate { SystemTXB.Clear(); }));
            ModulesTXB.Dispatcher.Invoke(new Action(delegate { ModulesTXB.Clear(); }));
        }

        /// <summary>
        /// вывод сообщения об ошибке в подсказки
        /// </summary>
        /// <param name="nameCommand"></param>
        /// <param name="color"></param>
        private void printHelpCommand(List<ListCommand> nameCommand, SolidColorBrush color)
        {
            string str = nameCommand.FirstOrDefault().helpPrint;

            if (str.Contains("#RED"))
            {
                string txt = str.Substring(4);
                logTXB.Text = txt;
                logTXB.Foreground = Brushes.Red;
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
            timerRobotWorkPrintModules.Stop();
            
            Random r = new Random();                        
            int randomString = r.Next(1, 6);

            int randomTime = r.Next(200,1500);
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

        /// <summary>
        /// вывод в консоль данных
        /// </summary>
        /// <param name="v">строка которую надо вывести</param>
        /// <param name="color">цвет строки</param>
        /// <param name="printLattice">надо ли в конце вывести знак решетки</param>
        private void addTextToRich(string v, SolidColorBrush color,Boolean printLattice)
        {
            if(sudoNotsudo && printLattice)
            {
                v = "root" + v;
            }

            if (v != String.Empty)
            {
                TextRange range = new TextRange(richTextBox.Document.ContentEnd, richTextBox.Document.ContentEnd);
             
                range.Text = v + Environment.NewLine;

                range.ApplyPropertyValue(TextElement.ForegroundProperty, color);
                //   range.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
            }

            richTextBox.CaretPosition = richTextBox.Document.ContentEnd;
            richTextBox.ScrollToEnd();
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

        #region сочетание клавиш на закрытие окна
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
        #endregion сочетание клавиш на закрытие окна
    }
}
