using LocalDataBase.FlashDrive;
using LocalDataBase.LocalDbSQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Robot
{
    public partial class MainWindow
    {
        private async void richTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // строка которую получили из консоли
            string command = textBoxCommands.Text.Trim();

            //press key enter
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (scenarioDiagnosticRobot == 0)
                {
                    //  addTextToRich("Робот не найден, подключите его к USB", Brushes.Red,false);
                    printHelpCommand("Робот не найден, подключите его к USB", Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }

                if (connectNotConnect == false && scenarioDiagnosticRobot != 4)
                {
                    //  addTextToRich("Инициализация робота не выполнена", Brushes.Red, false);
                    printHelpCommand("НЕТ СОЕДИНЕНИЯ С РОБОТОМ", Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }         

                if (command.Length == 0)
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
                        if (sudoNotsudo == false)
                        {
                            addTextToRich("Only root!", Brushes.Red, false);
                            printHelpCommand("Only root!", Brushes.Red);
                            beeper();
                            return;
                        }

                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("save", scenarioDiagnosticRobot);
                        textBoxSuffixAddText("#");
                        x2command = false;

                        if (scenarioDiagnosticRobot == 199)
                        {
                            GetSetScenarioOfFlashDrive.saveScenariy(199.ToString());
                        }
                        else
                        {
                            addTextToRich("No changes in system detected", Brushes.LightGreen, false);
                            printHelpCommand("No changes in system detected", Brushes.LightGreen);
                            textBoxSuffixAddText("#");
                            return;
                        }
                    }

                    if (textBoxSuffix.Text.Trim() == "Password:")
                    {
                        if (command == RepositoryLocalSQLite.searchCommandFromBD("password111q!!!", scenarioDiagnosticRobot).FirstOrDefault().helpPrint)
                        {
                            sudoNotsudo = true;
                            addTextToRich("Root rights successfully", Brushes.LightGreen, false);
                            printHelpCommand("Root rights successfully", Brushes.LightGreen);
                            textBoxCommands.Clear();
                            textBoxSuffixAddText("#");
                            x2command = false;
                            return;
                        }
                        else
                        {
                            addTextToRich("Authentication failure. Sorry, try again", Brushes.Red, false);
                            printHelpCommand("Authentication failure. Sorry, try again", Brushes.Red);
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
                        objDoc = new FlowDocument();
                        objParag1 = new Paragraph();

                        richTextBox.Document.Blocks.Clear();

                        addTextToRich("", Brushes.Green, false);
                        timerRobotWorkPrintModules.Stop();
                        // очистим модули
                        emptyModules();
                        addTextToRich("The system will rebooting . . .", Brushes.Red, false);
                        printHelpCommand("The system will rebooting . . .", Brushes.Red);
                        sudoNotsudo = false;
                        textBoxSuffixAddText("#");
                        textBoxCommands.Clear();

                        await Task.Delay(2000);

                        if (RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                        {
                            addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                        }

                        if ((GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario() == null) || (GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario() != 199))
                        {
                            scenarioDiagnosticRobot = GetSetScenarioOfFlashDrive.getNameFlashisAlive();
                        }

                        x2command = false;
                        printHelpCommand("", Brushes.Green);
                        timerRobotWorkPrintModules.Start();
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
                
                //поиск описания команды                               
                nameCommand = RepositoryLocalSQLite.searchCommandFromBD(command, scenarioDiagnosticRobot);
                
                // сколько слов в команде
                var yy = command.Split(new Char[] { }).Count();
                // первая команда
                var tt = command.Split(new Char[] { })[0];
                //если такая команда в списке исключений
                var zz = exceptionCommands.Find(a => a.Contains(tt));

                //    if (nameCommand == null || nameCommand.Count == 0 || exceptionCommand.Where(a => a ==  command.Split(new Char[] { })[0] ).Count() == 0)
                if ( nameCommand == null )
                {
                    if (zz == null)
                    {
                        addTextToRich(command + ":    " + "command not found", Brushes.Red, false);
                        printHelpCommand("command not found", Brushes.Red);
                        textBoxCommands.Clear();
                        beeper();
                        return;
                    }
                    
                }

                addTextToRich("# " + command, Brushes.LightGreen, false);
                textBoxCommands.Clear();

                #region команды
                // очистка консоли
                if ( command == "clear")
                {
                    objDoc = new FlowDocument();
                    objParag1 = new Paragraph();

                    richTextBox.Document.Blocks.Clear();
                    addTextToRich("", Brushes.Green, false);
                    return;
                }
                //диагностика
                if ( command == "diag all")
                {
                    if (scenarioDiagnosticRobot == 199)
                    {
                        colorizeModule(scenarioDiagnosticRobot, Brushes.Black);
                    }
                    else
                    {
                        colorizeModule(scenarioDiagnosticRobot, Brushes.Red);
                    }
                }

                // установка ПО
                if ( command == "init robot" )
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    if (GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenario5())
                    {
                        // все стало хорошо ОС установлена
                        scenarioDiagnosticRobot = 199;

                        //randomBatteryCharge();
                        //statusBataryLbl.Content = batteryCharge;
                        //modeLbl.Content = "Prog";
                        //modeLbl.Foreground = Brushes.Green;

                        //versionProgrammLbl.Foreground = Brushes.Green;
                        //versionProgrammLbl.Content = "v.15.7.16";

                        //connectOrDisconnectLbl.Content = "CONNECTED";
                        //connectOrDisconnectLbl.Foreground = Brushes.Green;

                        //if (RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot) != null)
                        //{
                        //    addTextToRich(RepositoryLocalSQLite.serachCOnnecting(scenarioDiagnosticRobot), Brushes.White);
                        //}

                        connectNotConnect = true;
                    }
                    else
                    {
                        addTextToRich("Init flash drive not detect", Brushes.Red, false);
                        printHelpCommand("Init flash drive not detect", Brushes.Red);
                        beeper();
                        return;
                    }
                }

                if ( command == "cpav scan" && scenarioDiagnosticRobot == 1)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    colorizeModule(scenarioDiagnosticRobot, Brushes.Green);
                    scenarioDiagnosticRobot = 199;
                    GetSetScenarioOfFlashDrive.saveScenariy("199");
                }

                if (( command == "cpav scan") && (scenarioDiagnosticRobot == 2))
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    colorizeModule(scenarioDiagnosticRobot, Brushes.Green);
                    scenarioDiagnosticRobot = 199;
                    GetSetScenarioOfFlashDrive.saveScenariy(199.ToString());
                }

                if ((command == "make module install ns230.bin") && scenarioDiagnosticRobot == 2)
                {
                    if (GetSetScenarioOfFlashDrive.checkFilesFromFlash("ns230.bin"))
                    {
                        if (sudoNotsudo == true)
                        {
                            colorizeModule(scenarioDiagnosticRobot, Brushes.Green);
                            scenarioDiagnosticRobot = 199;
                            GetSetScenarioOfFlashDrive.saveScenariy(199.ToString());
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
                        printHelpCommand("Ошибка ненайден файл ns230.bin", Brushes.Red);
                        beeper();
                        return;
                    }
                }

                if (scenarioDiagnosticRobot == 3 && command == "diag servo")
                {
                    addTextToRich("Servo modules FAIL " + errorFileScenario3, Brushes.Red, false);
                }

                // сценарий 3 
                if (scenarioDiagnosticRobot == 3 && command.Contains("make modules install") )
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    if(!command.Contains(errorFileScenario3))
                    {
                        addTextToRich("Сломан дуругой модуль", Brushes.Red, false);
                        printHelpCommand("Сломан дуругой модуль", Brushes.Red);
                        beeper();
                        return;
                    }
                    // переустановлен модуль сбойный
                    colorizeModule(scenarioDiagnosticRobot, Brushes.Black);
                    scenarioDiagnosticRobot = 199;
                }
                //else if (scenarioDiagnosticRobot == 3 && command == "make modules install " + errorFileScenario3)
                //{
                //    if (sudoNotsudo == false)
                //    {
                //        addTextToRich("Only root!", Brushes.Red, false);
                //        printHelpCommand("Only root!", Brushes.Red);
                //        beeper();
                //        return;
                //    }
                //    addTextToRich("module not found", Brushes.Red, false);
                //    printHelpCommand("Модуль для сборки не найден", Brushes.Red);
                //    beeper();
                //    return;
                //}

                //// команда buckup
                //if (nameCommand.FirstOrDefault().command == "backup" && GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == true
                //    && searchLastCommand() != "yes")
                //{
                //    if (scenarioDiagnosticRobot != 199)
                //    {
                //        // textBoxSuffixAddText("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary");
                //        addTextToRich("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary", Brushes.Red, false);
                //        printHelpCommand("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary", Brushes.Red);
                //        return;
                //    }

                //    textBoxSuffixAddText("Flash drive is not empty, all data will delete?");
                //    printHelpCommand("Flash drive is not empty, all data will delete?", Brushes.Red);
                //    x2command = true;
                //    return;
                //}
                //else if (GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == false && nameCommand.FirstOrDefault().command == "backup"
                //    && searchLastCommand() != "yes")
                //{
                //    if (scenarioDiagnosticRobot != 199)
                //    {
                //        // textBoxSuffixAddText("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary");
                //        addTextToRich("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary", Brushes.Red, false);
                //        printHelpCommand("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary", Brushes.Red);
                //        return;
                //    }
                //    GetSetScenarioOfFlashDrive.greateFileForBackup();
                //}

                if( command == "backup")
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    if (scenarioDiagnosticRobot != 199)
                    {
                        addTextToRich("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary", Brushes.Red, false);
                        //        printHelpCommand("Robot status does not allow backups. Please diagnose and repair any errors in the robot, if it’s necessary", Brushes.Red);
                               return;
                    }
                    else
                    {
                        if(GetSetScenarioOfFlashDrive.checkFilesFromFlashForInitScenarioBackup() == true)
                        {
                            textBoxSuffixAddText("Flash drive is not empty, all data will delete?");
                            printHelpCommand("Flash drive is not empty, all data will delete?", Brushes.Red);
                            x2command = true;
                            x2command = true;
                            return;
                            
                        }
                        else
                        {
                            GetSetScenarioOfFlashDrive.greateFileForBackup();
                        }
                    }
                }

                if (command == "save" && searchLastCommand() != "yes")
                {
                    // addTextToRich("Proceed with save?", Brushes.Red, false);
                    // textBoxSuffix.Text = "Proceed with save?";
                    textBoxSuffixAddText("Proceed with save?");
                    printHelpCommand("Proceed with save?", Brushes.Red);
                    x2command = true;
                    GetSetScenarioOfFlashDrive.saveScenariy(scenarioDiagnosticRobot.ToString());
                    return;
                }

                if (command == "sudo" && textBoxSuffix.Text != "Password:")
                {
                    if(sudoNotsudo)
                    {
                        addTextToRich("Already logged as Administrator ", Brushes.Red, false);
                        printHelpCommand("Already logged as Administrator ", Brushes.Red);
                        return;
                    }
                    //textBoxSuffix.Text = "Password: ";
                    textBoxSuffixAddText("Password: ");
                    printHelpCommand("Password: ", Brushes.Red);
                    x2command = true;
                    return;
                }

                if ( command == "reboot" && textBoxSuffix.Text != "Proceed with reboot?" && searchLastCommand() != "yes")
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    // textBoxSuffix.Text = "Proceed with reboot?";
                    textBoxSuffixAddText("Proceed with reboot? ");
                    printHelpCommand("Proceed with reboot?", Brushes.LightGreen);
                    x2command = true;
                    return;
                }

               // if (command.Split(new Char[] { }).Count() == 2 && command.Split(new Char[] { })[0] == "play" && command.Split(new Char[] { })[1] == "111")
               if((command.Split(new Char[] { }).Count() == 2 && command.Split(new Char[] { })[0] == "play" ))
                {
                    string fileName = command.Split(new Char[] { })[1];
                    nameCommand = RepositoryLocalSQLite.searchCommandFromBD("play", scenarioDiagnosticRobot);

                    if(nameCommand == null)
                    {
                        return;
                    }
                    
                    if (! nameCommand.FirstOrDefault().monitorPrint.Contains(fileName) )
                    {
                        addTextToRich("Unable to find a file to play", Brushes.Red, false);
                        printHelpCommand("Unable to find a file to play", Brushes.Red);

                        return;
                    }

                  //  System.Windows.Resources.StreamResourceInfo res = Application.GetResourceStream(new Uri("Sounds/trrr.mp3", UriKind.Relative));

                    string pathSounds = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/Sounds/" + fileName;
                    MediaPlayer.MediaPlayer.startMediaPlayer(pathSounds);
                    return;
                }


                if (sudoNotsudo == false && nameCommand.FirstOrDefault().sudo == 1)
                {
                    addTextToRich("Only root!", Brushes.Red, false);
                    printHelpCommand("Only root!", Brushes.Red);
                    beeper();
                    return;
                }

                //вывод текста команды и справки
             //   printHelpCommand(nameCommand, Brushes.LightGreen);
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

        List<string> exceptionCommands = new List<string>()
        {
            "play"
        };
     
    }
}
