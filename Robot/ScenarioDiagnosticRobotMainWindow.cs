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
    public partial class MainWindow
    {
        /// <summary>
        /// Получение сценария с флешки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setScenarioDiagnosticRobot(object sender, ElapsedEventArgs e)
        {
            timerGetDataFlashDrive.Stop();
            if(GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario() > 0)
            {
                scenarioDiagnosticRobot = GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario();
            }
            
            // проверим вылечен ли робот статус флешки 
            if (GetSetScenarioOfFlashDrive.getScenarioApplyNotapplyscenario() == 0 )
            {
                scenarioDiagnosticRobot = GetSetScenarioOfFlashDrive.getNameFlashisAlive();
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
                    // бипер при подклюении флешки
                    beeperLoadFlash();
                }
                catch (Exception ex)
                {
                    LogInFile.addFileLog("не известный робот его картинка ошибка " + ex.ToString());
                }
            }

            // usb не подключено
            if (scenarioDiagnosticRobot == 0)
            {
                try
                {
                    //LINK
                    //dateTimeLbl.Dispatcher.Invoke(new Action(delegate { dateTimeLbl.Content = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"); }));
                    statusConnectionLbl.Dispatcher.Invoke(new Action(delegate { statusConnectionLbl.Content = "OFF"; }));
                    statusConnectionLbl.Dispatcher.Invoke(new Action(delegate { statusConnectionLbl.Foreground = Brushes.Black; }));
                    //statusConnectionLbl.Content = "OFF";
                    //statusConnectionLbl.Foreground = Brushes.Gray;

                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Content = "N/A"; }));
                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Foreground = Brushes.Black; }));
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
                    logTXB.Dispatcher.Invoke(new Action(delegate { logTXB.Clear(); }));

                    //
                    //textBoxCommands.Clear
                    textBoxCommands.Dispatcher.Invoke(new Action(delegate { textBoxCommands.Clear(); }));

                    sudoNotsudo = false;

                    textBoxSuffix.Dispatcher.Invoke(new Action(delegate { textBoxSuffix.Text = "#"; }));

                    // бипер при подклюении флешки
                    beeperLoadFlash();

                }
                catch (Exception ex)
                {
                    LogInFile.addFileLog("юсб не подключено   " + ex.ToString());
                }
            }

            // usb подключено 
            if (scenarioDiagnosticRobot > 0)
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

                    modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Foreground = Brushes.Green; }));

                    if (!connectNotConnect)
                    {
                        modeLbl.Dispatcher.Invoke(new Action(delegate { modeLbl.Content = "Standby"; }));
                        connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.IsEnabled = true; }));
                        logTXB.Dispatcher.Invoke(new Action(delegate { logTXB.Text = "Link established. Use CONNECT button."; }));
                        logTXB.Dispatcher.Invoke(new Action(delegate { logTXB.Foreground = Brushes.LightGreen; }));
                        connectBtn.Dispatcher.Invoke(new Action(delegate { connectBtn.Focus(); }));
                    }

                    // бипер при подклюении флешки
                    beeperLoadFlash();

                }
                catch (Exception ex)
                {
                    LogInFile.addFileLog("юсб подключено  " + ex.ToString());
                }

            }
            timerGetDataFlashDrive.Start();
        }
             
        /// <summary>
        /// бипер при подключении флешки
        /// </summary>
        private void beeperLoadFlash()
        {
            //SystemSounds.Beep.Play();
            //SystemSounds.Asterisk.Play();
            //SystemSounds.Exclamation.Play();
               SystemSounds.Question.Play();
            //SystemSounds.Hand.Play();
        }

        private void searchFlashDriveandScenarioGet()
        {
            timerGetDataFlashDrive.Elapsed += setScenarioDiagnosticRobot;
            timerGetDataFlashDrive.Interval = 5000;
            timerGetDataFlashDrive.Start();
        }
    }
}
