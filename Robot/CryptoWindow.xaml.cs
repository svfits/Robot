using LocalDataBase.LocalDbSQLite;
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
        private bool decryptMessage;

        int scenario = 0;
        
        /// <summary>
        /// просто окно для шифрования
        /// </summary>
        public CryptoWindow()
        {
            InitializeComponent();

            keyT = new KeyT();
            RotaionStep.DataContext = keyT;
            decryptMessage = false;
        }

        /// <summary>
        /// выведем содержимое файла
        /// </summary>
        /// <param name="fileNameContains"></param>
        public CryptoWindow(string fileNameContains, int _scenario)
        {
            InitializeComponent();
            richForCrypto.IsReadOnly = true;
            scenario = _scenario;

            if(_scenario == 198)
            {
                decryptMessage = true;
            }

            if(!String.IsNullOrEmpty(fileNameContains))
            {               
                addTextToRichCrypto(fileNameContains,Brushes.LightGreen);
            }

            decryptMessage = false;
            keyT = new KeyT();
            this.DataContext = keyT;
        }

        public CryptoWindow(List<ListCommand> vv = null)
        {
            InitializeComponent();
            MessageBox.Show("Command crypto main tasks not found");

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
            string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.encryptCaesar(readRichCrypto(), Int32.Parse(RotionTextBox.Text));
            addTextToRichEnCrypto(ss, Brushes.LightGreen);
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
        ////Расшифровка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DecryptBtn_Click(object sender, RoutedEventArgs e)
        {
            if(decryptMessage == false && scenario != 198 && scenario != 4)
            {
                flashes();
                decryptMessage = true;
                beeper();
                return;
            }
            else
            {
                decryptMessage = false;
                textDecrypt.Text = "";
                textDecrypt2.Text = "";
                textDecrypt3.Text = "";
            }

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
            textDecrypt.Text = "Wrong selection leads to the data lost";
            textDecrypt2.Text = "Are you sure?";
            textDecrypt3.Text = "DECRYPT again";

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
                string ss = LocalDataBase.CryptoEncrypter.CryptoEncrypter.decryptCaesar(readRichCrypto(), Int32.Parse(RotionTextBox.Text));
                addTextToRichEnCrypto(ss, Brushes.LightGreen);
            }
            catch
            { }
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
             
            char[] chrKey = v.ToCharArray();
            // то что будем выводить
            List<string> randomString = new List<string>();
            List<int> notEdit = new List<int>();
                             
            //перемешаем и заполним 
            for (int i = 0; i <= 20 ; i++)
            {
                char[] randomKey = WarningCheckFilesRandom.randomStr(chrKey.Length);

                for (int j = 0; j <= ( chrKey.Length / 20) ; j++)
                {
                    int rdmNumber = WarningCheckFilesRandom.randomSleep(0, (randomKey.Length - 1));
                    notEdit.Add(rdmNumber);
                }            
           
                foreach(var tt in notEdit)
                {
                    randomKey[tt] = chrKey[tt];
                  //  System.Diagnostics.Debug.WriteLine(randomKey[tt] + "  " + tt + chrKey[tt]);
                }
              
                randomString.Add(new string(randomKey));
            }

            playTextCryptoSound();
            
            foreach (var gg in randomString)
            {
                addTextToRich(gg, color);
                System.Diagnostics.Debug.WriteLine(gg);
                await Task.Delay(300);
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
}
