using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;
using TuneConverter.Framework.TuneStorage;
using WPFTuneConverter.View.Pages;
using WPFTuneConverter.View.Windows;


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

        private static string runningDirectory = Directory.GetCurrentDirectory();
        private static readonly string outImageFilePath = Path.GetFullPath(Path.Combine(runningDirectory,
            Path.Combine(runningDirectory, "..", "..", "..", "..", "TuneConverter.Framework.PageImageIO", "OutImages/")));
        private static readonly string exportFilePath = Path.GetFullPath(Path.Combine(runningDirectory,
            Path.Combine(runningDirectory, "..", "..", "..", "..", "TuneConverter.Framework.PageImageIO", "ExportFolder/")));

        public TuneDisplayTable()
        {
            InitializeComponent();

            TableRecord = new List<TableRecord>();

            TuneSerializer tf = new();

            tuneFullList = TuneSerializer.ConvertByteArrayListToTuneFullList("data.dat");
            tuneFullList = tuneFullList.Where(tune => tune.Title is not null).ToList();
            tuneFullList = tuneFullList.OrderBy(record => record.Title).ToList();

            foreach (TuneFull tune in tuneFullList)
            {
                if (tune.Title is null)
                {
                    
                    continue;
                }
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

        }

        private void TuneRepo_Unloaded(object sender, RoutedEventArgs e)
        {
            List<string> stringsList = new List<string>();

            foreach (var tune in tuneFullList)
            {
                stringsList.Add(TuneSerializer.SerializeTune(tune));
            }

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

                //var file_ext = outImageFilePath + selectedItem.Type + "/" + selectedItem.Title;
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

                    //file_ext = outImageFilePath + selectedItem.Type + "/" + selectedItem.Title + "/" + selectedItem.Title;
                    //maxFileTotal = Directory.GetFiles(outImageFilePath + selectedItem.Type + "/" + selectedItem.Title).Length;
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

            //var file_ext =  outImageFilePath + selectedItem.Type + "/" + selectedItem.Title;
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
                //file_ext = outImageFilePath + selectedItem.Type + "/" + selectedItem.Title + "/" + selectedItem.Title;
                //maxFileTotal = Directory.GetFiles(outImageFilePath + selectedItem.Type + "/" + selectedItem.Title).Length;
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

            tuneFullList = tuneFullList.Where(tune => !tune.Title.Equals(selectedItem.Title)).ToList();
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

            //var file_ext = outImageFilePath + selectedItem.Type + "/" + selectedItem.Title;
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

                //SaveBitmapImageToFile(bmpTemp, exportFilePath + selectedItem.Title + ".png");
            }
            catch (Exception es)
            {
                string[] files = Directory.GetFiles("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title, "*.*", SearchOption.TopDirectoryOnly);

                //string[] files = Directory.GetFiles(outImageFilePath + selectedItem.Type + "/" + selectedItem.Title, "*.*", SearchOption.TopDirectoryOnly);
                
                foreach(var sa in files)
                {
                    string pattern = @"\\(.+)\.";

                    MatchCollection matches = Regex.Matches(sa, pattern);
                    var s = matches[0].Groups[1].Value;
                    bmpTemp = new BitmapImage();
                    file1 = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + selectedItem.Type + "/" + selectedItem.Title + "/" + s + ".png";

                    //file1 =     outImageFilePath + selectedItem.Type + "/" + selectedItem.Title + "/" + s + ".png"; 
                    bmpTemp.BeginInit();
                    bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                    bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    bmpTemp.UriSource = new Uri(file1);
                    bmpTemp.EndInit();
                    SaveBitmapImageToFile(bmpTemp, "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/ExportFolder/" + s + ".png");

                    //SaveBitmapImageToFile(bmpTemp, exportFilePath + s + ".png");
                }
                
            }

        }

        static void SaveBitmapImageToFile(BitmapImage bitmapImage, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder(); 

            MemoryStream memoryStream = new MemoryStream();

            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(memoryStream);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                memoryStream.WriteTo(fileStream);
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            var convertPage = new ConvertTunePage();
            //convertPage.Opacity = 0;
            var selectedItem = (TableRecord)dataGrid.SelectedItem;

            if (selectedItem is null)
            {
                return;
            }

            var inTuneDirec = "c:\\Users\\Isaac\\source\\repos\\TuneConverter\\TuneConverter.Framework.PageImageIO\\InputNotes/" + selectedItem.Title + ".txt";

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

            //convertPage.file_ext = outImageFilePath + convertPage.typeComboBox.textInput.Text + tuneDirec + "/" + convertPage.titleTextBox.textInput.Text;
            var file1 = convertPage.file_ext + page_num + ".png";


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

            mainWindow?.mainFrame.Navigate(convertPage);


            //while (convertPage.pageCountLabel.Opacity < 1)
            //{
            //    convertPage.pageCountLabel.Opacity += (0.08);
            //    mainWindow?.mainFrame.Navigate(convertPage);
            //    await Task.Delay(1);
            //}
            //convertPage.pageCountLabel.Opacity = 1;
            //mainWindow?.mainFrame.Navigate(convertPage);

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
