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

        public CryptoWindow()
        {
            InitializeComponent();

            keyT = new KeyT();
            RotaionStep.DataContext = keyT;
        }

        public CryptoWindow(string fileNameContains)
        {
            InitializeComponent();
            richForCrypto.IsReadOnly = true;

            if(!String.IsNullOrEmpty(fileNameContains))
            {               
                addTextToRichCrypto(fileNameContains,Brushes.LightGreen);
            }

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
            switch(metodCrypto)
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

            textDecrypt.Text = "Wrong selection leads to the data lost Are you sure? Press again";
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
                await Task.Delay(200);
            }

            addTextToRich(v, color);

            stopTextCryptoSound();

            //for(int i = 1; i <= 10; i++)
            //{
            //    char[] randomStr = WarningCheckFilesRandom.randomStr(nStr);
            //    int result = WarningCheckFilesRandom.randomSleep(20, 300);

            //    addTextToRich(new string(randomStr), color);
            //    await Task.Delay(500);
            //}

            //addTextToRich(nStr.ToString(), color);

            //foreach (var dd in nStr)
            //{
            //    addTextToRich(dd.ToString(), color);
            //    int result = WarningCheckFilesRandom.randomSleep(20, 300);
            //    await Task.Delay(result);
            //}  

            //for(int i =0; i < nStr.Length; i++ )
            //{
            //    addTextToRich(nStr[i].ToString(), color);
            //    await Task.Delay(200);
            //}
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
            return new TextRange(richForCrypto.Document.ContentStart, richForCrypto.Document.ContentEnd).Text;
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
