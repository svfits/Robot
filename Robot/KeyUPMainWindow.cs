using LocalDataBase.FlashDrive;
using LocalDataBase.LocalDbSQLite;
using LocalDataBase.RandomFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Robot
{
    public partial class MainWindow
    {
        /// <summary>
        ////заблокируем все что нажимается 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Alt && e.SystemKey == Key.Space)
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }

            if (Keyboard.Modifiers == ModifierKeys.Shift && e.SystemKey == Key.F10)
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }

            if ( (Keyboard.Modifiers == ModifierKeys.Control && e.SystemKey == Key.O ) || (Keyboard.Modifiers == ModifierKeys.Control && e.SystemKey == Key.O))
            {
                e.Handled = true;
            }
            else
            {
                base.OnKeyDown(e);
            }

            if (e.Key == Key.Tab && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                e.Handled = true;
            }

            if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                e.Handled = true;
            }

        }

        private async void richTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // строка которую получили из консоли
            string command = textBoxCommands.Text.Trim();

            //press key enter
            if (e.Key == Key.Enter)
            {               

                if (scenarioDiagnosticRobot == 0)
                {
                    //  addTextToRich("Робот не найден, подключите его к USB", Brushes.Red,false);
                    printHelpCommand("Robot Disconnected. Please connect the Robot.", Brushes.Red);
                    textBoxCommands.Clear();
                    beeper();
                    return;
                }

                if (connectNotConnect == false && scenarioDiagnosticRobot != 4)
                {
                    //  addTextToRich("Инициализация робота не выполнена", Brushes.Red, false);
                    printHelpCommand("Robot is not Connected", Brushes.Red);
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
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("backup system", scenarioDiagnosticRobot);
                        textBoxSuffixAddText("#");
                        x2command = false;

                        GetSetScenarioOfFlashDrive.deleteFilesFromFlash();
                        GetSetScenarioOfFlashDrive.greateFileForBackup();
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
                        await commandsReboot();
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

                // если это справка
                if(command.Contains("?"))
                {
                    if(nameCommand == null)
                    {
                        addTextToRich(command + ":    " + "command not found", Brushes.Red, false);
                        printHelpCommand("command not found", Brushes.Red);
                        textBoxCommands.Clear();
                        beeper();
                        return;
                    }

                    addTextToRich("# " + command, Brushes.LightGreen, false);
                    addTextToRich(nameCommand, Brushes.LightGreen);
                    textBoxCommands.Clear();
                    return;
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
                                         
                        connectNotConnect = true;
                    }
                    else
                    {
                        addTextToRich("Usb flash drive is not available yet", Brushes.Red, false);
                        printHelpCommand("Usb flash drive is not available yet", Brushes.Red);
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

                if ((command == "make modules install ns230.bin") && scenarioDiagnosticRobot == 2)
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    if (GetSetScenarioOfFlashDrive.checkFilesFromFlash("ns230.bin"))
                    {                     
                            scenarioDiagnosticRobot = 199;
                            GetSetScenarioOfFlashDrive.saveScenariy(199.ToString());
                    }
                    else
                    {
                        addTextToRich("Error file not found ns230.bin", Brushes.Red, false);
                        printHelpCommand("Error file not found ns230.bin", Brushes.Red);
                        beeper();
                        return;
                    }
                }

                if ( command == "make modules install ns274.bin" )
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    if (GetSetScenarioOfFlashDrive.checkFilesFromFlash("ns274.bin"))
                    {
                        scenarioDiagnosticRobot = 199;
                        GetSetScenarioOfFlashDrive.saveScenariy(199.ToString());
                    }
                    else
                    {
                        addTextToRich("Error file not found ns274.bin", Brushes.Red, false);
                        printHelpCommand("Error file not found ns274.bin", Brushes.Red);
                        beeper();
                        return;
                    }
                }

                // сценарий 3 
                if (scenarioDiagnosticRobot == 3 && command.Contains("make modules install"))
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
                        nameCommand = RepositoryLocalSQLite.searchCommandFromBD("help make modules install", scenarioDiagnosticRobot);
                        addTextToRich(nameCommand, Brushes.White);
                        beeper();
                        return;
                    }
                    // переустановлен модуль сбойный
                    colorizeModule(scenarioDiagnosticRobot, Brushes.Black);
                    scenarioDiagnosticRobot = 199;
                }             

                if( command == "backup system")
                {
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

                    if( GetSetScenarioOfFlashDrive.getPathToFlashAliens() == String.Empty)
                    {
                        addTextToRich("Usb flash drive is not available yet", Brushes.Red, false);
                        printHelpCommand("Usb flash drive is not available yet", Brushes.Red);
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
                    if (sudoNotsudo == false)
                    {
                        addTextToRich("Only root!", Brushes.Red, false);
                        printHelpCommand("Only root!", Brushes.Red);
                        beeper();
                        return;
                    }

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
                    List<string> files = GetSetScenarioOfFlashDrive.getFilesFromFlashAliens();

                    if (files == null)
                    {
                        addTextToRich("USB flash drive or flash is not available yet", Brushes.Red, false);
                        return;
                    }

                    if (files.Find(a => a == fileName) == null)
                    {
                        addTextToRich("File not found", Brushes.Red, false);
                        printHelpCommand("File not found", Brushes.Red);
                        return;
                    }

                    string[] fileFormat = fileName.Split(new Char[] { '.' });

                    if (( fileFormat.Count() > 2 || fileFormat.Count() < 2) || ( fileFormat[1].Trim().ToLower() != "wav" && fileFormat[1].Trim().ToLower() != "mp3"))
                    {
                        addTextToRich("Unknown file format", Brushes.Red, false);
                        printHelpCommand("Unknown file format", Brushes.Red);
                        return;
                    }

                    //  string pathSounds = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "/Sounds/" + fileName;
                    string pathSounds = Path.Combine(GetSetScenarioOfFlashDrive.getPathToFlashAliens(), fileName);

                    //  MediaPlayer.MediaPlayer.startMediaPlayer(pathSounds);

                    PlaySoundsWindow plw = new PlaySoundsWindow(pathSounds);
                    plw.ShowDialog();
                    return;
                }

                if (command.Split(new Char[] { }).Count() > 0 && command.Split(new Char[] { })[0] == "ls")
                {
                    CommandsLs();
                    return;
                }

                if (command.Split(new Char[] { }).Count() == 2 && command.Split(new Char[] { })[0] == "cat")
                {
                    // имя файла для команды
                    string fileName = command.Split(new Char[] { })[1];
                    // список файлов на флешке
                    List<string> files = GetSetScenarioOfFlashDrive.getFilesFromFlashAliens();

                    if (files == null)
                    {
                        addTextToRich("USB flash drive or flash is not available yet", Brushes.Red, false);
                        return;
                    }
                                        
                    if (files.Find(a => a == fileName) == null)
                    {
                        addTextToRich("File not found", Brushes.Red, false);
                        printHelpCommand("File not found", Brushes.Red);
                        return;
                    }

                    try
                    {
                        string[] fileFormat = fileName.Split(new Char[] { '.' });

                        if(fileFormat.Count() > 2 || fileFormat.Count() < 2 || fileFormat[1].Trim().ToLower() != "txt" )
                        {
                            addTextToRich("Unknown file format", Brushes.Red, false);
                            printHelpCommand("Unknown file format", Brushes.Red);
                            return;
                        }

                        string[] contentFile = GetSetScenarioOfFlashDrive.getFileContents(fileName);

                        foreach (var s in contentFile)
                        {
                            addTextToRich(s, Brushes.White, false);
                        }

                        addTextToRich("Total Lines: " + contentFile.Count(), Brushes.White, false);
                        return;
                    }
                    catch(Exception ex)
                    {
                        LogInFile.addFileLog("Произошла ошибка при выводе команды cat " + ex.ToString());
                        MessageBox.Show("Произошла ошибка в файле: " + fileName +  ex.ToString());
                        return;
                    }                
                }


                if (nameCommand != null && sudoNotsudo == false && nameCommand.FirstOrDefault().sudo == 1)
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

        /// <summary>
        ////команда перезагрузки
        /// </summary>
        /// <returns></returns>
        private async Task commandsReboot()
        {
            objDoc = new FlowDocument();
            objParag1 = new Paragraph();               

            addTextToRich("00:22:16: %SYS-5-REBOOT: Reboot requeste", Brushes.White, false);
            addTextToRich("System Bootstrap, Version 15.7.16", Brushes.White, false);
            addTextToRich("ANDROID SOFTWARE Copyright (c) 2073 by CP Systems", Brushes.White, false);
            addTextToRich("Corp.The system will booting…", Brushes.White, false);
            addTextToRich("     ", Brushes.White, false);          

            await Task.Delay(3000);

            richTextBox.Document.Blocks.Clear();

            addTextToRich("", Brushes.Green, false);
            timerRobotWorkPrintModules.Stop();
            // очистим модули
            emptyModules();

            sudoNotsudo = false;
            textBoxSuffixAddText("#");
            textBoxCommands.Clear();

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

        /// <summary>
        ////команда LS
        /// </summary>
        public void CommandsLs()
        {
            List<string> files = GetSetScenarioOfFlashDrive.getFilesFromFlashAliens();

            if (files == null)
            {
                addTextToRich("Usb flash drive is not available yet", Brushes.Red, false);
                return;
            }

            foreach (var file in files)
            {
                addTextToRich("root  " + WarningCheckFilesRandom.RandomSizeFile() + " " + WarningCheckFilesRandom.RandomTime() + "  " + file, Brushes.White, false);
            }
            addTextToRich("Total files: " + files.Count(), Brushes.White, false);
            return;
        }

        /// <summary>
        ////список исключений в командах
        /// </summary>
        List<string> exceptionCommands = new List<string>()
        {
            "play",
            "cat" ,
            "ls"
        };
     
    }
}
