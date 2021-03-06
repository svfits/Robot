﻿using LocalDataBase.LocalDbSQLite;
using LocalDataBase.RandomFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Robot
{
    /// <summary>
    /// Interaction logic for CryptoWindow.xaml
    /// </summary>
    public partial class CryptoWindow : Window
    {
        string metodCrypto = "Hex";        

        FlowDocument objDoc = new FlowDocument();
        Paragraph objParag1 = new Paragraph();
        KeyT keyT;
        /// <summary>
        /// показать красный или не показать 
        /// </summary>
        private bool? decryptMessage;

        int scenario = 0;

        List<ListCommand> lsCommand = new List<ListCommand>();

        string methodCryptoInFile;

        /// <summary>
        /// crypto file or crypto
        /// </summary>
        private bool cryptnotCryptTextFile = false;

        /// <summary>
        /// имя файла заблокированного
        /// </summary>
        string fileName;

        /// <summary>
        /// просто окно для шифрования new 
        /// </summary>
        public CryptoWindow()
        {
            InitializeComponent();
          //  decryptMessage = false;
            keyT = new KeyT();
            RotaionStep.DataContext = keyT;           
        }

        /// <summary>
        /// выведем содержимое файла
        /// </summary>
        /// <param name="fileNameContains"></param>
        public CryptoWindow(string fileNameContains, int _scenario, string _fileName)
        {
            InitializeComponent();

            fileName = _fileName;
            decryptMessage = false;
            richForCrypto.IsReadOnly = true;
            scenario = _scenario;

            List<string> resultList = error198BlockInterface.error198BlockInterfaceCryptoTextFile.Where(t => t.Contains(_fileName)).ToList<string>();

            if (resultList.Count > 0 )
            {
                EncryptBtn.IsEnabled = false;
                DecryptBtn.IsEnabled = false;
            }

            if (!String.IsNullOrEmpty(fileNameContains))
            {
                if(fileNameContains.Contains("#hex"))
                {
                    methodCryptoInFile = "hex";
                    fileNameContains = fileNameContains.Replace("#hex", "");
                    cryptnotCryptTextFile = true;
                }

                if (fileNameContains.Contains("#rotx"))
                {
                    methodCryptoInFile = "rotx";
                    fileNameContains = fileNameContains.Replace("#rotx", "");
                    cryptnotCryptTextFile = true;
                }

                if (fileNameContains.Contains("#withkey"))
                {
                    methodCryptoInFile = "with key";
                    fileNameContains = fileNameContains.Replace("#withkey", "");
                    cryptnotCryptTextFile = true;
                }

                if (fileNameContains.Contains("#none"))
                {
                    methodCryptoInFile = "#none";
                    fileNameContains = fileNameContains.Replace("#none", "");
                    //addTextToRich("TEXT IS NOT ENCRYPTED", Brushes.Red);
                    //addTextToRichCrypto(fileNameContains, Brushes.LightGreen);

                    //addTextToRich("TEXT IS NOT ENCRYPTED", Brushes.Red);
                    cryptnotCryptTextFile = true;
                    //return;
                }

                addTextToRichCrypto(fileNameContains,Brushes.LightGreen);
            }
                    
            keyT = new KeyT();
            this.DataContext = keyT;
        }

        /// <summary>
        ////вывод по сценарию crypto main tasks
        /// </summary>
        /// <param name="vv"></param>
        /// <param name="_scenario"></param>
        public CryptoWindow(List<ListCommand> vv, int _scenario)
        {
            InitializeComponent();
            richForCrypto.IsReadOnly = true;
            lsCommand = vv;

            if(_scenario == 198)
            {
                decryptMessage = false;
                addTextToRichCrypto(vv.FirstOrDefault().monitorPrint, Brushes.LightGreen);
                keyT = new KeyT();
                this.DataContext = keyT;

                if(error198BlockInterface.error198BlockInterfaceCryptoMainTasks == true)
                {
                    EncryptBtn.IsEnabled = false;
                    DecryptBtn.IsEnabled = false;
                }

                return;
            }

            //if (lsCommand.FirstOrDefault().helpPrint.Trim().ToLower() == "#none")
            //{
            //    //TEXT IS NOT ENCRYPTED
            //   // addTextToRichEnCrypto("TEXT IS NOT ENCRYPTED", Brushes.LightGreen);
            // //   await Task.Delay(delayShowData);
            //    System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

            //    addTextToRichCrypto(vv.FirstOrDefault().monitorPrint, Brushes.LightGreen);
            //    addTextToRich("TEXT IS NOT ENCRYPTED", Brushes.Red);
            //    return;
            //}

            if (!String.IsNullOrEmpty(vv.FirstOrDefault().monitorPrint))
            {
                addTextToRichCrypto(vv.FirstOrDefault().monitorPrint , Brushes.LightGreen);
            }

            keyT = new KeyT();
            this.DataContext = keyT;
        }

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {
            textDecrypt.Text = "";
            textDecrypt2.Text = "";
            textDecrypt3.Text = "";

            switch (metodCrypto)
            {
                case "Hex":
                    cryptotoHex();
                    break;
                case "Rotx":
                    cryptoCaesar();
                    break;
                case "With Key":
                    cryptoWithKey();
                    break;
                default:
                    break;
            }

            textDecrypt.Text = String.Empty;
        }

        private void cryptoCaesar()
        {
            try
            {
                string textNotCrypted = readRichCrypto();
                string cryptedTxt = String.Empty;

                foreach (var txtStr in textNotCrypted.Split(new string[] { "\r\n" }, StringSplitOptions.None))
                {
                    foreach (var word in txtStr.Split(new string[] { " " }, StringSplitOptions.None))
                    {
                        if (word != "")
                        {
                            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.encryptCaesar(word, Int32.Parse(RotionTextBox.Text));
                            cryptedTxt = cryptedTxt + ss;
                        }

                        cryptedTxt = cryptedTxt + " ";
                    }

                    cryptedTxt = cryptedTxt + Environment.NewLine;
                }

                addTextToRichEnCrypto(cryptedTxt, Brushes.LightGreen);
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cryptoWithKey()
        {
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.stringToWithKey(readRichCrypto(), Key.Content.ToString());
            addTextToRichEnCrypto(ss, Brushes.LightGreen);
        }

        private void cryptotoHex()
        {
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.stringToHEx(readRichCrypto());
            addTextToRichEnCrypto(ss, Brushes.LightGreen);      
        }

        /// <summary>
        //// шифровка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int delayShowData = 8000;

                if (decryptMessage == false)
                {
                    flashes();
                    decryptMessage = true;
                    beeper();
                    return;
                }
                else if (decryptMessage != null)
                {
                    decryptMessage = false;
                    textDecrypt.Text = "";
                    textDecrypt2.Text = "";
                    textDecrypt3.Text = "";


                    // crypto main tasks       
                    if (lsCommand.Count > 0)
                    {
                        if (lsCommand.FirstOrDefault().helpPrint.Trim().ToLower() == "#none")
                        {
                            //TEXT IS NOT ENCRYPTED
                            addTextToRichEnCrypto("TEXT IS NOT ENCRYPTED", Brushes.Red);
                            await Task.Delay(delayShowData);
                            System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            addTextToRich("TEXT IS NOT ENCRYPTED", Brushes.Red);
                            return;
                        }

                        if ((lsCommand.Count != 0) && (lsCommand != null) && (lsCommand.FirstOrDefault().helpPrint.Trim().ToLower() != metodCrypto.ToLower()))
                        {
                            EncryptBtn.IsEnabled = false;
                            DecryptBtn.IsEnabled = false;
                            //  beeper();
                            //Not correct decrypt method. 
                            //addTextToRichCrypto("Not correct decrypt method.", Brushes.Red);
                            addTextToRichEnCrypto("WRONG CRYPTO METHOD", Brushes.LightGreen);
                            await Task.Delay(delayShowData);
                            System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            addTextToRich("WRONG CRYPTO METHOD", Brushes.Red);
                            error198BlockInterface.error198BlockInterfaceCryptoMainTasks = true;
                            return;
                        }

                        startMethodCrypto();
                        return;
                    }


                    //file crypto
                    if (!cryptnotCryptTextFile)
                    {
                        EncryptBtn.IsEnabled = false;
                        DecryptBtn.IsEnabled = false;
                        //beeper();
                        //text is not encrypted
                        addTextToRichEnCrypto("Text is not correct crypted", Brushes.LightGreen);
                        await Task.Delay(delayShowData);
                        addTextToRich("Text is not correct crypted", Brushes.Red);
                        error198BlockInterface.error198BlockInterfaceCryptoTextFile.Add(fileName);
                        return;

                    }
                    else if (cryptnotCryptTextFile == true)
                    {
                        if (methodCryptoInFile == "#none")
                        {
                            //TEXT IS NOT ENCRYPTED
                            addTextToRichEnCrypto("TEXT IS NOT ENCRYPTED", Brushes.LightGreen);
                            await Task.Delay(delayShowData);
                            System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            addTextToRich("TEXT IS NOT ENCRYPTED", Brushes.Red);
                            return;
                        }


                        if (metodCrypto.ToLower() != methodCryptoInFile)
                        {
                            EncryptBtn.IsEnabled = false;
                            DecryptBtn.IsEnabled = false;
                            //beeper();
                            //text is not encrypted
                            addTextToRichEnCrypto("WRONG CRYPTO METHOD", Brushes.LightGreen);
                            await Task.Delay(delayShowData);
                            addTextToRich("WRONG CRYPTO METHOD", Brushes.Red);
                            error198BlockInterface.error198BlockInterfaceCryptoTextFile.Add(fileName);
                            return;
                        }
                    }

                    startMethodCrypto();
                    return;


                }

                // CRYPTO NEW
                if (decryptMessage == null)
                {
                    if (lsCommand != null && lsCommand.Count != 0)
                    {

                        if (lsCommand.FirstOrDefault().helpPrint.Trim().ToLower() == "#none")
                        {
                            //text is not encrypted
                            addTextToRichEnCrypto("TEXT IS NOT ENCRYPTED", Brushes.LightGreen);
                            return;
                        }

                        if (lsCommand.FirstOrDefault().helpPrint.Trim().ToLower() != metodCrypto.ToLower())
                        {
                            EncryptBtn.IsEnabled = false;
                            DecryptBtn.IsEnabled = false;
                            beeper();
                            //Not correct decrypt method. 
                            addTextToRichEnCrypto("WRONG KEY WORD. ", Brushes.LightGreen);
                            await Task.Delay(delayShowData);
                            System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            addTextToRich("WRONG KEY WORD. ", Brushes.Red);
                            return;
                        }

                    }
                    startMethodCrypto();
                    return;
                }
            }
            catch(Exception ex)
            {
                LogInFile.addFileLog("Произошла ошибка при нажатии кнопки дешифрования " + ex.ToString());
            }

        }

        /// <summary>
        /// запустим шифрование
        /// </summary>
        private void startMethodCrypto()
        {
            switch (metodCrypto)
            {
                case "Hex":
                    encryptoHextoString();
                    break;
                case "Rotx":
                    encryptoCeasar();
                    break;
                case "With Key":
                    encryptorWithKey();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// animation TEXT decrypt
        /// </summary>
        private void flashes()
        {
            textDecrypt.Text = "Wrong selection leads to the data lost.";
            textDecrypt2.Text = "Are you sure?";
            textDecrypt3.Text = "press DECRYPT again";

            (Resources["animationTextBlock3"] as Storyboard).Begin();
            (Resources["animationTextBlock2"] as Storyboard).Begin();
            (Resources["animationTextBlock"] as Storyboard).Begin();

            //DoubleAnimation Animation = new DoubleAnimation();
            //Animation.From = textDecrypt.FontSize;
            //Animation.To = 250;
            //Animation.AutoReverse = true;
            //Animation.FillBehavior = FillBehavior.Stop;
            //Animation.RepeatBehavior = new RepeatBehavior(1);
            //Animation.Duration = TimeSpan.FromSeconds(1);
            //textDecrypt.BeginAnimation(WidthProperty, Animation);
        }

        /// <summary>
        /// шифрование
        /// </summary>
        private void encryptoCeasar()
        {
            
            try
            {             
                string textNotCrypted = readRichCrypto();
                string cryptedTxt = String.Empty;

                foreach (var txtStr in textNotCrypted.Split(new string[] { "\n" }, StringSplitOptions.None))
                {
                    foreach (var word in txtStr.Split(new string[] { " " }, StringSplitOptions.None))
                    {
                        if (word != "")
                        {
                            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.decryptCaesar(word, Int32.Parse(RotionTextBox.Text));
                            cryptedTxt = cryptedTxt + ss;
                        }

                        cryptedTxt = cryptedTxt + " ";
                    }

                    cryptedTxt = cryptedTxt + Environment.NewLine;
                }

                addTextToRichEnCrypto(cryptedTxt, Brushes.LightGreen);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!    " + ex);
            }
        }

        private void encryptorWithKey()
        {
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.withKeytoString(readRichCrypto(), Key.Content.ToString());
            addTextToRichEnCrypto(ss, Brushes.LightGreen);
        }

        /// <summary>
        /// из Hex в string
        /// </summary>
        private void encryptoHextoString()
        {
           string ss =  LocalDataBase.CryptoEncrypter.CryptoEncrypter.hexToString(readRichCrypto());
            addTextToRichEnCrypto(ss, Brushes.LightGreen);
        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            try
            {             
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                var cnbSelected = selectedItem.Content.ToString();

                System.Diagnostics.Debug.WriteLine(cnbSelected);
                
                switch (cnbSelected)
                {
                    case "Hex":
                        //System.Diagnostics.Debug.WriteLine("HEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEX");
                        Key.Visibility = Visibility.Collapsed;
                        RotaionStep.Visibility = Visibility.Collapsed;
                        metodCrypto = "Hex";
                        break;
                    case "Rotx":
                        //System.Diagnostics.Debug.WriteLine("ROoooooooooooooooooooooooooooooooooooooooooooootx");
                        Key.Visibility = Visibility.Collapsed;
                        RotaionStep.Visibility = Visibility.Visible;
                        Key.Header = "Rotation Step";
                        metodCrypto = "Rotx";
                        break;
                    case "With Key":
                      //  System.Diagnostics.Debug.WriteLine("Wiiiiiiiiiiiiiiiiiiiiiith Keeeeeeeeeeeeeeeeeeeeey");
                        Key.Visibility = Visibility.Visible;
                        RotaionStep.Visibility = Visibility.Collapsed;
                        Key.Header = "Key";
                        metodCrypto = "With Key";
                        break;
                    default:
                        break;
                }
            }
            catch
            {
               
            }
        }

        public void beeper()
        {
            SystemSounds.Beep.Play();           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
#else
                  Topmost = true;
#endif

        }

        /// <summary>
        ////Добавление в Крипто исходный код
        /// </summary>
        /// <param name="v"></param>
        /// <param name="color"></param>
        private void addTextToRichCrypto(string v, SolidColorBrush color)
        {
            if (v != String.Empty)
            {
                TextRange range = new TextRange(richForCrypto.Document.ContentEnd, richForCrypto.Document.ContentEnd);
                objParag1.Inlines.Add(new Run(v) { Foreground = color });
                objDoc.Blocks.Add(objParag1);
            }

            try
            {
                richForCrypto.Dispatcher.Invoke(new Action(delegate { richForCrypto.Document = objDoc; }));
                richForCrypto.Dispatcher.Invoke(new Action(delegate { richForCrypto.CaretPosition = richForCrypto.Document.ContentEnd; }));
                richForCrypto.Dispatcher.Invoke(new Action(delegate { richForCrypto.ScrollToEnd(); }));
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("ошибка при добавлении консоль крипто " + ex.ToString());
            }
        }

        /// <summary>
        /// Добавление в richtexbox ENcryptor
        /// </summary>
        /// <param name="v"></param>
        /// <param name="color"></param>
        private async void addTextToRichEnCrypto(string v, SolidColorBrush color)
        {  
            if(v == String.Empty)
            {
                return;
            }   
            
            // original crypt 
            char[] chrKey = v.ToCharArray();
            
            // то что будем выводить мусор
            List<string> randomString = new List<string>();

            //случайные позиции котрые совпали с шифром
            List<int> notEdit = new List<int>();
                             
            //перемешаем и заполним 
            for (int i = 0; i <= 30 ; i++)
            {
                char[] randomKey = WarningCheckFilesRandom.randomStr(chrKey.Length);

                for (int j = 0; j <= ( chrKey.Length / 30) ; j++)
                {
                    int rdmNumber = WarningCheckFilesRandom.randomSleep(0, (randomKey.Length - 1));
                    notEdit.Add(rdmNumber);                  
                }            
           
                foreach(var tt in notEdit)
                {                    
                    randomKey[tt] =  chrKey[tt];
                   // System.Diagnostics.Debug.WriteLine(randomKey[tt] + "  " + tt + "   " + chrKey[tt]);
                }

                await Task.Delay(1);
                randomString.Add(new string(randomKey));
            }

            playTextCryptoSound();
            
            foreach (var gg in randomString)
            {
                addTextToRich(gg, color);
              //  System.Diagnostics.Debug.WriteLine(gg);
                await Task.Delay(150);
            }

            addTextToRich(v, color);

            stopTextCryptoSound();
                     
        }

        private void addTextToRich(string v, SolidColorBrush color)
        {
            objDoc = new FlowDocument();
            objParag1 = new Paragraph();

            richEncrypto.Document.Blocks.Clear();

            if (v != String.Empty)
            {
                TextRange range = new TextRange(richEncrypto.Document.ContentEnd, richEncrypto.Document.ContentEnd);
                objParag1.Inlines.Add(new Run(v /*+ Environment.NewLine*/) { Foreground = color });
                objDoc.Blocks.Add(objParag1);
            }

            try
            {
                richForCrypto.Dispatcher.Invoke(new Action(delegate { richEncrypto.Document = objDoc; }));
                richForCrypto.Dispatcher.Invoke(new Action(delegate { richEncrypto.CaretPosition = richEncrypto.Document.ContentEnd; }));
                richForCrypto.Dispatcher.Invoke(new Action(delegate { richEncrypto.ScrollToEnd(); }));
            }
            catch (Exception ex)
            {
                LogInFile.addFileLog("ошибка при добавлении консоль крипто Encrypto " + ex.ToString());
            }
        }

        /// <summary>
        /// Содержимое Rich
        /// </summary>
        /// <returns></returns>
        private string readRichCrypto()
        {
            return new TextRange(richForCrypto.Document.ContentStart, richForCrypto.Document.ContentEnd).Text.Trim();
        }

        private void RotionTextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                MessageBox.Show(e.Error.ErrorContent.ToString());
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void playTextCryptoSound()
        {
            SoundPlayer player = new SoundPlayer();
            player.Stream = Robot.Properties.Resources.enterFileName;
            player.Play();
        }

        private void stopTextCryptoSound()
        {
            SoundPlayer player = new SoundPlayer();
            player.Stream = Robot.Properties.Resources.enterFileName;
            player.Stop();
        }
    }

    public class KeyT
    {
        private int _key = 1;
        /// <summary>
        ///  проверка ключа только цифры
        /// </summary>
        public int key
        {
            get
            {
                return _key;
            }

            set
            {
                if (value < 0 || value > 99)
                {
                    throw new ArgumentException("Key is not included in the range of values");
                }
                else
                {
                    _key = value;
                }
            }
        }     
    }

    /// <summary>
    /// Заблокирован ли интерфейс шифрования или доступен
    /// </summary>
    public static class error198BlockInterface
    {
        private static bool _error198BlockInterfaceCryptoMainTasks;
        private static List<string> _error198BlockInterfaceCryptoTextFile = new List<string>();

        public static Boolean error198BlockInterfaceCryptoMainTasks
        {
            get
            {
                return _error198BlockInterfaceCryptoMainTasks;
            }

            set
            {
                _error198BlockInterfaceCryptoMainTasks = value;
            }
        }

        public static List<string> error198BlockInterfaceCryptoTextFile
        {
            get
            {
                return _error198BlockInterfaceCryptoTextFile;
            }

            set
            {
                _error198BlockInterfaceCryptoTextFile = value;
            }
        }
    }
}
