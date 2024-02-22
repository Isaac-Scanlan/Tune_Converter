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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneComponents.Types;

namespace WPFTuneConverter.View.Pages
{
    /// <summary>
    /// Interaction logic for ABCConvertTune.xaml
    /// </summary>
    public partial class ABCConvertTune : Page
    {
        public ABCConvertTune()
        {
            InitializeComponent();

            string fileName = string.Empty;
            List<string> tune = new List<string>();

            int pageNumber = 0;
            int maxPageNumber = 0;

            string file_ext = "";

            int resizeFactor = 6;

            List<KeyNote> keys = new() {
            new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Major}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.A, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.B, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.C, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.D, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.E, AccidentalType = AccidentalType.Flat, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.F, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Natural, Keytype = KeyType.Minor}
            ,new() { NoteType = NoteType.G, AccidentalType = AccidentalType.Sharp, Keytype = KeyType.Minor}
        };

        }


        //    private void convert3_Click(object sender, RoutedEventArgs e)
        //    {
        //        var tuneLine = tuneTextBlock1.Text.Replace("\n", "");
        //        tune = tuneLine.Split('\r').ToList();

        //        Converter converter = new Converter();
        //        var file = converter.ConvertToInternalABC(tune);

        //        string disp = "";

        //        foreach (var line in file)
        //        {
        //            disp += line + "\r\n";
        //        }

        //        tuneTextBlock2.Text = disp;
        //    }

        //    private void convertButton4_Click(object sender, RoutedEventArgs e)
        //    {
        //        var tuneLine = tuneTextBlock2.Text.Replace("\n", "");
        //        tune = tuneLine.Split('\r').ToList();

        //        fileName = tune[0];

        //        int count = tune.Where(x => x.Equals("__")).Count() - 1;


        //        double cou = count / 2.0;
        //        maxPageNumber = (int)Math.Round(cou, 0, MidpointRounding.AwayFromZero);

        //        resizeFactor = 4;
        //        if (count == 2 && tune.Count > 17)
        //        {
        //            resizeFactor += 2;
        //        }
        //        if (tune[1].ToLower().Equals("reel") ||
        //            tune[1].ToLower().Equals("hornpipe") ||
        //            tune[1].ToLower().Equals("barndance") ||
        //            tune[1].ToLower().Equals("fling") ||
        //            tune[1].ToLower().Equals("waltz") ||
        //            tune[1].ToLower().Equals("mazurka"))
        //        {
        //            resizeFactor += 1;
        //        }

        //        string tuneDirec = maxPageNumber > 1 ? "/" + tune[0] : "";
        //        string page_num = maxPageNumber > 1 ? "_page_1" : "";

        //        pageNumber = 1;

        //        Process p = new Process();
        //        ProcessStartInfo ps = new(System.AppDomain.CurrentDomain.BaseDirectory + "Main.TuneConverterApp.exe", tune);
        //        p.StartInfo = ps;

        //        p.StartInfo.UseShellExecute = false;
        //        p.StartInfo.RedirectStandardOutput = true;
        //        p.StartInfo.CreateNoWindow = true;
        //        p.Start();

        //        //string output = p.StandardOutput.ReadToEnd();
        //        p.WaitForExit();

        //        Image bm;
        //        file_ext = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + tune[1] + tuneDirec + "/" + fileName;
        //        var file = file_ext + page_num + ".png";

        //        //using (var bmpTemp = new Bitmap(file))
        //        //{
        //        //    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
        //        //}

        //        //tuneImage.Source = new BitmapImage(new Uri(file));


        //        var bmpTemp = new BitmapImage();
        //        bmpTemp.BeginInit();
        //        bmpTemp.CacheOption = BitmapCacheOption.OnLoad;
        //        bmpTemp.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
        //        bmpTemp.UriSource = new Uri(file);
        //        bmpTemp.EndInit();
        //        tuneImage1.Source = bmpTemp;


        //        pageCountLabel.Content = pageNumber + " out of " + maxPageNumber;

        //        TuneWriter tw = new TuneWriter();
        //        tw.WriteFile(tune);
        //    }

        //    private void Button_Click(object sender, RoutedEventArgs e)
        //    {
        //        writtenByBox.textInput.Clear();
        //        repeatsComboBox.textInput.Text = "";
        //        keyComboBox.textInput.Text = "";
        //        typeComboBox.textInput.Text = "";
        //        titleTextBox.textInput.Clear();
        //        tuneTextBlock.textInput.Text = "";
        //    }
    }

}
