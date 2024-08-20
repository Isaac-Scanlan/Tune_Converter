using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneComponents.Types;
using TuneConverter.Framework.TuneIO.TuneReader;

namespace WPFTuneConverter.View.Pages
{
    
    public partial class ConvertTunePage : Page
    {
        string fileName = string.Empty;
        List<string> tune = new List<string>();

        public int pageNumber = 0;
        public int maxPageNumber = 0;

        public string file_ext = "";

        int resizeFactor = 6;

        public static string runningDirectory = Directory.GetCurrentDirectory();

        public readonly string localFilePath;
        public readonly string filePath;
        public readonly string outImageFilePath;
        public readonly string inputNotesFilePath;
        public readonly string defaultImageFilePath;




        List<KeyNote> keys = new() {
            new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Dor}

            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Dor}

            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}

            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Dor}

            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Dor}

            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Dor}

            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Dor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Mix}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Dor}

        };

        public ConvertTunePage()
        {
            InitializeComponent();

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            localFilePath = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(baseDirectory, "..", "..", "..", ".."));

            filePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(localFilePath, "TuneConverterAppInterface/"));
            outImageFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(localFilePath, "TuneConverter.Framework.PageImageIO" + "OutImages/"));
            inputNotesFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(localFilePath, "TuneConverter.Framework.PageImageIO" + "InputNotes"));
            defaultImageFilePath = System.IO.Path.GetFullPath(System.IO.Path.Combine(localFilePath, "WPFTuneConverter/"));


           // tuneTextBlock.textInput.Text = filePath + "\n\n" + outImageFilePath;


            foreach (var item in Enum.GetNames(typeof(TuneType)))
            {
                typeComboBox.textInput.Items.Add(item.ToString());
            }

            foreach (var item in keys)
            {
                var symbol = item.AccidentalType == AccidentalType.Sharp ? "#" : item.AccidentalType == AccidentalType.Flat ? "b" : "";
                keyComboBox.textInput.Items.Add(item.NoteType.ToString() + symbol + " " + item.Keytype.ToString().ToLower());
            }

            repeatsComboBox.textInput.Items.Add("Single");
            repeatsComboBox.textInput.Items.Add("Double");
            repeatsComboBox.textInput.Items.Add("Triple");

            System.Drawing.Image bm;
            //using (var bmpTemp = new Bitmap(filePath + "default_image.png"))
            using (var bmpTemp = new Bitmap("C:/Users/Isaac/source/repos/TuneConverter/TuneConverterAppInterface/default_image.png"))

            {
                bm = new Bitmap(bmpTemp, new System.Drawing.Size(bmpTemp.Width / 4, bmpTemp.Height / 4));
            }

            //tuneImage.Source = new BitmapImage(new Uri("C:/Users/Isaac/source/repos/TuneConverter/TuneConverterAppInterface/default_image.png"));

            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            tuneImage.Opacity = 0.5;
        }


        private void ConvertButton(object sender, RoutedEventArgs e)
        {
            var tuneLine = tuneTextBlock.textInput.Text.Replace("\n", "");
            tune = tuneLine.Split('\r').ToList();

            tune.Insert(0, "__");
            tune.Insert(0, writtenByBox.textInput.Text);
            tune.Insert(0, repeatsComboBox.textInput.Text);
            tune.Insert(0, keyComboBox.textInput.Text);
            tune.Insert(0, typeComboBox.textInput.Text);
            tune.Insert(0, titleTextBox.textInput.Text);

            if (!tune[tune.Count - 1].Equals("__"))
            {
                tune.Add("__");
            }

            fileName = titleTextBox.textInput.Text;

            int count = tune.Where(x => x.Equals("__")).Count() - 1;


            double cou = count / 2.0;
            maxPageNumber = (int)Math.Round(cou, 0, MidpointRounding.AwayFromZero);

            resizeFactor = 4;
            if (count == 2 && tune.Count > 17)
            {
                resizeFactor += 2;
            }
            if (tune[1].ToLower().Equals("reel") ||
                tune[1].ToLower().Equals("hornpipe") ||
                tune[1].ToLower().Equals("barndance") ||
                tune[1].ToLower().Equals("fling") ||
                tune[1].ToLower().Equals("waltz") ||
                tune[1].ToLower().Equals("mazurka"))
            {
                resizeFactor += 1;
            }

            string tuneDirec = maxPageNumber > 1 ? "/" + tune[0] : "";
            string page_num = maxPageNumber > 1 ? "_page_1" : "";

            pageNumber = 1;

            Process p = new Process();
            ProcessStartInfo ps = new(System.AppDomain.CurrentDomain.BaseDirectory + "Main.TuneConverterApp.exe", tune);
            p.StartInfo = ps;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            //string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            System.Drawing.Image bm;
            //file_ext = outImageFilePath + tune[1] + tuneDirec + "/" + fileName;
            file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + tune[1] + tuneDirec + "/" + fileName;

            var file = file_ext + page_num + ".png";

            TuneWriter tw = new TuneWriter();
            tw.WriteFile(tune);

            //using (var bmpTemp = new Bitmap(file))
            //{
            //    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
            //}

            //tuneImage.Source = new BitmapImage(new Uri(file));


            var bmpTemp = new BitmapImage();
            bmpTemp.BeginInit();
            bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
            bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bmpTemp.UriSource = new Uri(file);
            bmpTemp.EndInit();
            tuneImage.Opacity = 1;
            tuneImage.Source = bmpTemp;


            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            //TuneWriter tw = new TuneWriter();
            //tw.WriteFile(tune);
        }

        private void NextPageButton(object sender, RoutedEventArgs e)
        {
            if (pageNumber != 0)
            {
                pageNumber += pageNumber == maxPageNumber ? 0 : 1;
            }

            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            if (maxPageNumber > 1)
            {
                System.Drawing.Image bm;

                var file = file_ext + "_page_" + pageNumber + ".png";

                //using (var bmpTemp = new Bitmap(file))
                //{
                //    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
                //}

                var bmpTemp = new BitmapImage();
                bmpTemp.BeginInit();
                bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmpTemp.UriSource = new Uri(file);
                bmpTemp.EndInit();
                tuneImage.Source = bmpTemp;

                //tuneImage.Source = new BitmapImage(new Uri(file));

            }
        }

        private void PreviousPageButton(object sender, RoutedEventArgs e)
        {
            if (pageNumber != 0)
            {
                pageNumber -= pageNumber == 1 ? 0 : 1;
            }
            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            if (maxPageNumber > 1)
            {
                System.Drawing.Image bm;

                var file = file_ext + "_page_" + pageNumber + ".png";

                //using (var bmpTemp = new Bitmap(file))
                //{
                //    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
                //}

                var bmpTemp = new BitmapImage();
                bmpTemp.BeginInit();
                bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
                bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bmpTemp.UriSource = new Uri(file);
                bmpTemp.EndInit();
                tuneImage.Source = bmpTemp;

                //tuneImage.Source = new BitmapImage(new Uri(file));
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            //var dlg = new OpenFileDialog()
            //{
            //    InitialDirectory = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/InputNotes",
            //    Filter = "All Files (*.*) | *.*",
            //    RestoreDirectory = true
            //};

            //dlg.ShowDialog();

            // Configure open file dialog box
            var dialog = new OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".txt"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            //dialog.DefaultDirectory = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/InputNotes";
            dialog.RestoreDirectory = true;
            dialog.InitialDirectory = "c:\\Users\\Isaac\\source\\repos\\TuneConverter\\TuneConverter.Framework.PageImageIO\\InputNotes";

            //dialog.InitialDirectory = inputNotesFilePath;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            bool multiPage = false;

            if (!dialog.FileName.Equals("") && !dialog.FileName.Equals("Document"))
            {
                TuneReader tr = new();
                var file = tr.readFileForView(dialog.FileName);

                titleTextBox.textInput.Text = file[0][0];
                typeComboBox.textInput.Text = file[0][1];

                var s = file[0][2].Split(' ');

                if (file[0].Count > 3)
                {
                    repeatsComboBox.textInput.Text = file[0][3];
                }
                if (file[0].Count > 4)
                {
                    writtenByBox.textInput.Text = file[0][4];
                }
                else
                {
                    writtenByBox.textInput.Text = "";
                }

                keyComboBox.textInput.Text = s[0] + " " + s[1].ToLower();

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
                    maxPageNumber = (int)Math.Round(cou, 0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    maxPageNumber = 1;
                }

                tuneTextBlock.textInput.Text = disp;
            }



            string tuneDirec = maxPageNumber > 1 ? "/" + titleTextBox.textInput.Text : "";
            string page_num = maxPageNumber > 1 ? "_page_1" : "";

            pageNumber = 1;

            System.Drawing.Image bm;
            file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + typeComboBox.textInput.Text + tuneDirec + "/" + titleTextBox.textInput.Text;
            //file_ext = outImageFilePath + typeComboBox.textInput.Text + tuneDirec + "/" + titleTextBox.textInput.Text;

            var file1 = file_ext + page_num + ".png";

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
                tuneImage.Opacity = 1;
                tuneImage.Source = bmpTemp;
            }
            catch (Exception es)
            {

            }


            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;


        }

        //string defaultImageFilePath => System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetCurrentDirectory(),
        //    System.IO.Path.Combine(runningDirectory, "..", "..", "..", "..", "WPFTuneConverter/")));

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            writtenByBox.textInput.Clear();
            repeatsComboBox.textInput.Text = "";
            keyComboBox.textInput.Text = "";
            typeComboBox.textInput.Text = "";
            titleTextBox.textInput.Clear();
            tuneTextBlock.textInput.Text = "";
            tuneImage.Opacity = 0.5;
            //tuneImage.Source = new BitmapImage(new Uri(defaultImageFilePath + "default_image.png"));
            tuneImage.Source = new BitmapImage(new Uri("C:/Users/Isaac/source/repos/TuneConverter/WPFTuneConverter/default_image.png"));

            //tuneImage.Source = new BitmapImage(new Uri(defaultImageFilePath + "default_image.png"));
        }

        private void previousPageButton_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var butt = previousPageButton.Background;
            previousPageButton.Background = (System.Windows.Media.Brush)System.Windows.Media.ColorConverter.ConvertFromString("#EA1515");;
        }
    }
}
