using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;
using TuneConverter.Framework.TuneStorage;
using WPFTuneConverter.View.Pages;
using WPFTuneConverter.View.Windows;
using static Emgu.CV.Dai.OpenVino;

namespace WPFTuneConverter.View.CustomControls
{
    /// <summary>
    /// Interaction logic for TuneDisplayTable.xaml
    /// </summary>
    public partial class TuneDisplayTable : UserControl
    {
        internal List<TableRecord> TableRecord { get; set; }
        internal List<TableRecord> FullTableRecord { get; set; }
        internal List<TuneFull> tuneFullList { get; set; }

        public TuneDisplayTable()
        {
            InitializeComponent();

            TableRecord = new List<TableRecord>();

            TuneSerializer tf = new();

            tuneFullList = TuneSerializer.ConvertByteArrayListToTuneFullList("data.dat");
            tuneFullList = tuneFullList.OrderBy(record => record.Title).ToList();



            foreach (TuneFull tune in tuneFullList)
            {

                var keyNote = tune.Key.NoteType.ToString();
                string accidentalType = tune.Key.AccidentalType.ToString().Equals("Flat")? "b" :
                                        tune.Key.AccidentalType.ToString().Equals("Sharp") ? "#" : "";
                var keyType = " " + tune.Key.Keytype.ToString();

                TableRecord.Add(new TableRecord { 
                    Title = tune.Title, 
                    Type = tune.TuneType.ToString(), 
                    Key = keyNote + accidentalType + keyType, 
                    Composer = tune.Composer
                });
            }


            FullTableRecord = TableRecord;

            dataGrid.ItemsSource = TableRecord;

            SearchOptionsCombobox.textInput.Items.Add("Title");
            SearchOptionsCombobox.textInput.Items.Add("Type");
            SearchOptionsCombobox.textInput.Items.Add("Key");
            SearchOptionsCombobox.textInput.Items.Add("Composer");

            
           //mainTable. = tableRecord;
        }

        private void TuneRepo_Unloaded(object sender, RoutedEventArgs e)
        {
            List<string> stringsList = new List<string>();

            foreach (var tune in tuneFullList)
            {
                stringsList.Add(TuneSerializer.SerializeTune(tune));
            }

            // Save list of byte arrays to a file
            TuneSerializer.SaveByteArrayListToFile(
                TuneSerializer.ConvertStringListToByteArrayList(stringsList), "data.dat");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TableRecord = FullTableRecord;
            string searchName = SearchName.textInput.Text;
            string searchType = SearchOptionsCombobox.textInput.Text;

            if(searchName.Equals("") || searchType.Equals(""))
            {
                dataGrid.ItemsSource = TableRecord;
                return;
            }

            PropertyInfo propInfo = typeof(TableRecord).GetProperty(searchType);
            if (propInfo != null)
            {
                TableRecord = TableRecord.OrderBy(p => propInfo.GetValue(p, null)).ToList();
            }

            TableRecord = TableRecord.Where(p => ((string)propInfo.GetValue(p)).Contains(searchName)).ToList();

            dataGrid.ItemsSource = TableRecord;
        }

        

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Check if the double-click event was fired on a DataGridRow
            var row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            if (row != null)
            {
                var selectedItem = (TableRecord)dataGrid.SelectedItem;

                System.Drawing.Image bm;
                var file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title;
                var file1 = file_ext + ".png";

                
                var bmpTemp = new BitmapImage();
                var pageDirectory = "";
                int maxFileTotal = 1;
                bool multipage = false;
                try
                {
                    bmpTemp.BeginInit();
                    bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                    bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmpTemp.UriSource = new Uri(file1);
                    bmpTemp.EndInit();
                }
                catch (Exception es)
                {
                    bmpTemp = new BitmapImage();
                    file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title + "/" + selectedItem.Title;
                    maxFileTotal = Directory.GetFiles("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title).Length;
                    file1 = file_ext + "_page_1.png";
                    pageDirectory = file_ext;
                    multipage = true;
                    bmpTemp.BeginInit();
                    bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                    bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmpTemp.UriSource = new Uri(file1);
                    bmpTemp.EndInit();
                }


                // Open a new window and pass the selected item
                var window = new TunePageWindow(multipage, maxFileTotal);
                window.directoryString = pageDirectory;
                window.Title = selectedItem.Title;
                window.TuneImage.Opacity = 1;
                window.TuneImage.Source = bmpTemp;
                window.ShowDialog();
            }
        }

        private void viewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (TableRecord)dataGrid.SelectedItem;

            if(selectedItem is null)
            {
                return;
            }

            System.Drawing.Image bm;
            var file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title;
            var file1 = file_ext + ".png";


            var bmpTemp = new BitmapImage();
            var pageDirectory = "";
            int maxFileTotal = 1;
            bool multipage = false;
            try
            {
                bmpTemp.BeginInit();
                bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmpTemp.UriSource = new Uri(file1);
                bmpTemp.EndInit();
            }
            catch (Exception es)
            {
                bmpTemp = new BitmapImage();
                file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title + "/" + selectedItem.Title;
                maxFileTotal = Directory.GetFiles("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title).Length;
                file1 = file_ext + "_page_1.png";
                pageDirectory = file_ext;
                multipage = true;
                bmpTemp.BeginInit();
                bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmpTemp.UriSource = new Uri(file1);
                bmpTemp.EndInit();
            }


            // Open a new window and pass the selected item
            var window = new TunePageWindow(multipage, maxFileTotal);
            window.directoryString = pageDirectory;
            window.Title = selectedItem.Title;
            window.TuneImage.Opacity = 1;
            window.TuneImage.Source = bmpTemp;
            window.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (TableRecord)dataGrid.SelectedItem;

            if (selectedItem is null)
            {
                return;
            }

            FullTableRecord.RemoveAll(record => record.Title == selectedItem.Title);
            TableRecord.RemoveAll(record => record.Title == selectedItem.Title);

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = TableRecord;

            tuneFullList = tuneFullList.Where(tune => 
            !tune.Title.Equals(selectedItem.Title) 
            //||
            //!tune.Key.Equals(selectedItem.Key) ||
            //!tune.TuneType.ToString().Equals(selectedItem.Type)
            ).ToList();
            tuneFullList = tuneFullList.OrderBy(record => record.Title).ToList();

            
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (TableRecord)dataGrid.SelectedItem;

            if (selectedItem is null)
            {
                return;
            }

            System.Drawing.Image bm;
            var file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title;
            var file1 = file_ext + ".png";


            var bmpTemp = new BitmapImage();

            try
            {
                bmpTemp.BeginInit();
                bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmpTemp.UriSource = new Uri(file1);
                bmpTemp.EndInit();
                SaveBitmapImageToFile(bmpTemp, "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/ExportFolder/" + selectedItem.Title + ".png");
            }
            catch (Exception es)
            {
                string[] files = Directory.GetFiles("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title, "*.*", SearchOption.TopDirectoryOnly);
                
                foreach(var sa in files)
                {
                    string pattern = @"\\(.+)\.";

                    // Use Regex.Matches to find all matches
                    MatchCollection matches = Regex.Matches(sa, pattern);
                    var s = matches[0].Groups[1].Value;
                    bmpTemp = new BitmapImage();
                    file1 = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title + "/" + s + ".png"; 
                    bmpTemp.BeginInit();
                    bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                    bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmpTemp.UriSource = new Uri(file1);
                    bmpTemp.EndInit();
                    SaveBitmapImageToFile(bmpTemp, "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/ExportFolder/" + s + ".png");
                }
                
            }

        }

        static void SaveBitmapImageToFile(BitmapImage bitmapImage, string filePath)
        {
            // Create a BitmapEncoder based on the provided file extension
            BitmapEncoder encoder = new PngBitmapEncoder(); // For PNG format, change to JpegBitmapEncoder or other encoders if needed

            // Create a MemoryStream to hold the encoded image data
            MemoryStream memoryStream = new MemoryStream();

            // Encode the BitmapImage and save it to the MemoryStream
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(memoryStream);

            // Write the encoded image data from the MemoryStream to a file using a FileStream
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                memoryStream.WriteTo(fileStream);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the parent window
            var mainWindow = Window.GetWindow(this) as MainWindow;

            var convertPage = new ConvertTunePage();

            var selectedItem = (TableRecord)dataGrid.SelectedItem;

            if (selectedItem is null)
            {
                return;
            }

            var inTuneDirec = "c:\\Users\\Isaac\\source\\repos\\TuneConverter\\TuneConverter.Framework.PageImageIO\\InputNotes/" + selectedItem.Title + ".txt";

            bool multiPage = false;

            if (true)
            {
                TuneReader tr = new();
                var file = tr.readFileForView(inTuneDirec);

                convertPage.titleTextBox.textInput.Text = file[0][0];
                convertPage.typeComboBox.textInput.Text = file[0][1];

                var s = file[0][2].Split(' ');

                if (file[0].Count > 3)
                {
                    convertPage.repeatsComboBox.textInput.Text = file[0][3];
                }
                if (file[0].Count > 4)
                {
                    convertPage.writtenByBox.textInput.Text = file[0][4];
                }
                else
                {
                    convertPage.writtenByBox.textInput.Text = "";
                }

                convertPage.keyComboBox.textInput.Text = s[0] + " " + s[1].ToLower();

                file.RemoveAt(0);

                var disp = "";

                foreach (var part in file.Select((value, i) => (value, i)))
                {
                    foreach (var line in part.value)
                    {
                        disp += line + "\r\n";
                    }
                    disp += "__";
                    if (part.i + 1 < file.Count)
                    {
                        disp += "\r\n";
                    }

                }

                if (file.Count > 2)
                {
                    multiPage = true;
                    double cou = file.Count / 2.0;
                    convertPage.maxPageNumber = (int)Math.Round(cou, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    convertPage.maxPageNumber = 1;
                }

                convertPage.tuneTextBlock.textInput.Text = disp;
            }



            string tuneDirec = convertPage.maxPageNumber > 1 ? "/" + convertPage.titleTextBox.textInput.Text : "";
            string page_num = convertPage.maxPageNumber > 1 ? "_page_1" : "";

            convertPage.pageNumber = 1;

            System.Drawing.Image bm;
            convertPage.file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + convertPage.typeComboBox.textInput.Text + tuneDirec + "/" + convertPage.titleTextBox.textInput.Text;
            var file1 = convertPage.file_ext + page_num + ".png";

            //using (var bmpTemp = new Bitmap(file))
            //{
            //    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
            //}

            //tuneImage.Source = new BitmapImage(new Uri(file));

            try
            {
                var bmpTemp = new BitmapImage();
                bmpTemp.BeginInit();
                bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmpTemp.UriSource = new Uri(file1);
                bmpTemp.EndInit();
                convertPage.tuneImage.Opacity = 1;
                convertPage.tuneImage.Source = bmpTemp;
            }
            catch (Exception es)
            {

            }


            convertPage.pageCountLabel.Content = convertPage.pageNumber + " out of " + convertPage.maxPageNumber;



            // Navigate to Page 2
            mainWindow?.mainFrame.Navigate(convertPage);

        }
    }


    internal record TableRecord
    {
        public required String Title { get; set; }
        public required String Type { get; set; }
        public required String Key { get; set; }
        public String Composer { get; set; } = string.Empty;
    }
}
