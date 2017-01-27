using System;
using System.Collections.Generic;
using System.Linq;
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

        public CryptoWindow()
        {
            InitializeComponent();
        }

        public CryptoWindow(string fileNameContains)
        {
            InitializeComponent();
            richForCrypto.IsReadOnly = true;

            if(!String.IsNullOrEmpty(fileNameContains))
            {               
                addTextToRichCrypto(fileNameContains,Brushes.LightGreen);
            }            
        }

        private void EncryptBtn_Click(object sender, RoutedEventArgs e)
        {
            switch(metodCrypto)
            {
                case "Hex":
                    cryptotoHex();
                    break;
                case "Rotx":
                    break;
                case "With Key":
                    cryptoWithKey();
                    break;
                default:
                    break;
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
                    break;
                case "With Key":
                    encryptorWithKey();
                    break;
                default:
                    break;
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
                        System.Diagnostics.Debug.WriteLine("HEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEX");
                        Key.Visibility = Visibility.Hidden;
                        metodCrypto = "Hex";
                        break;
                    case "Rotx":
                        System.Diagnostics.Debug.WriteLine("ROoooooooooooooooooooooooooooooooooooooooooooootx");
                        Key.Visibility = Visibility.Visible;
                        Key.Header = "Rotation Step";
                        metodCrypto = "Rotx";
                        break;
                    case "With Key":
                        System.Diagnostics.Debug.WriteLine("Wiiiiiiiiiiiiiiiiiiiiiith Keeeeeeeeeeeeeeeeeeeeey");
                        Key.Visibility = Visibility.Visible;
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
            Topmost = true;
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
                objParag1.Inlines.Add(new Run(v + Environment.NewLine) { Foreground = color });
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
        private void addTextToRichEnCrypto(string v, SolidColorBrush color)
        {
            objDoc = new FlowDocument();
            objParag1 = new Paragraph();

            richEncrypto.Document.Blocks.Clear();

            if (v != String.Empty)
            {
                TextRange range = new TextRange(richEncrypto.Document.ContentEnd, richEncrypto.Document.ContentEnd);
                objParag1.Inlines.Add(new Run(v + Environment.NewLine) { Foreground = color });
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

    }
}
