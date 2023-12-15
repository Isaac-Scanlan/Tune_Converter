﻿using System.Drawing;
using System.Text;
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
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.TuneIO.TuneReader;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;
using System.Diagnostics;
using Microsoft.Win32;


namespace WPFTuneConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fileName = string.Empty;
        List<string> tune = new List<string>();

        int pageNumber = 0;
        int maxPageNumber = 0;

        string file_ext = "";

        int resizeFactor = 6;

        List<KeyNote> keys = new() {
            new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
        };

        public MainWindow()
        {
            InitializeComponent();

            foreach (var item in Enum.GetNames(typeof(TuneType)))
            {
                typeComboBox.Items.Add(item.ToString());
            }

            foreach (var item in keys)
            {
                var symbol = item.AccidentalType == AccidentalType.Sharp ? "#" : item.AccidentalType == AccidentalType.Flat ? "b" : "";
                keyComboBox.Items.Add(item.NoteType.ToString() + symbol + " " + item.Keytype.ToString().ToLower());
            }

            Image bm;
            using (var bmpTemp = new Bitmap("C:/Users/Isaac/source/repos/TuneConverter/TuneConverterAppInterface/default_image.png"))
            {
                bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / 4, bmpTemp.Height / 4));
            }

            tuneImage.Source = new BitmapImage(new Uri("C:/Users/Isaac/source/repos/TuneConverter/TuneConverterAppInterface/default_image.png"));

            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;
        }
        private void ConvertButton(object sender, RoutedEventArgs e)
        {
            var tuneLine = tuneTextBlock.Text.Replace("\n", "");
            tune = tuneLine.Split('\r').ToList();

            tune.Insert(0, "__");
            tune.Insert(0, keyComboBox.Text);
            tune.Insert(0, typeComboBox.Text);
            tune.Insert(0, titleTextBox.Text);

            if (!tune[tune.Count - 1].Equals("__"))
            {
                tune.Add("__");
            }

            fileName = titleTextBox.Text;

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

            Image bm;
            file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + tune[1] + tuneDirec + "/" + fileName;
            var file = file_ext + page_num + ".png";

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
            tuneImage.Source = bmpTemp;


            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            TuneWriter tw = new TuneWriter();
            tw.WriteFile(tune);
        }

        private void NextPageButton(object sender, RoutedEventArgs e)
        {
            pageNumber += pageNumber == maxPageNumber ? 0 : 1;
            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            if (maxPageNumber > 1)
            {
                Image bm;

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
            pageNumber -= pageNumber == 1 ? 0 : 1;
            pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

            if (maxPageNumber > 1)
            {
                Image bm;

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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

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

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            if (!dialog.FileName.Equals("") && !dialog.FileName.Equals("Document"))
            {
                TuneReader tr = new();
                var file = tr.readFileForView(dialog.FileName);

                titleTextBox.Text = file[0][0];
                typeComboBox.Text = file[0][1];

                var s = file[0][2].Split(' ');

                keyComboBox.Text = s[0] + " " + s[1].ToLower();

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

                tuneTextBlock.Text = disp;
            }
        }
    }
}