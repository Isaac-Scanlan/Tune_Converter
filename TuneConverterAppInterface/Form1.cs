using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.TuneIO.TuneReader;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Emgu.CV.ML.KNearest;
using System.IO;
using TuneConverter.Framework.TuneComponents.Types;
using static System.Windows.Forms.LinkLabel;


namespace TuneConverterAppInterface
{
    public partial class Form1 : Form
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

        public Form1()
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

            assembledTuneDisplay.Image = bm;
            pageNumberLabel.Text = pageNumber + " out of " + maxPageNumber;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tuneLine = tuneBox.Text.Replace("\n", "");
            tune = tuneLine.Split('\r').ToList();

            tune.Insert(0, "__");
            tune.Insert(0, keyComboBox.Text);
            tune.Insert(0, typeComboBox.Text);
            tune.Insert(0, tuneName.Text);

            if (tune[tune.Count - 1].Equals("__"))
            {

            }
            else
            {
                tune.Add("__");
            }
            

            fileName = tuneName.Text;

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
            ProcessStartInfo ps = new(Application.StartupPath.ToString() + "Main.TuneConverterApp.exe", tune);
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

            using (var bmpTemp = new Bitmap(file))
            {
                bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
            }

            assembledTuneDisplay.Image = bm;
            pageNumberLabel.Text = pageNumber + " out of " + maxPageNumber;

            TuneWriter tw = new TuneWriter();
            tw.WriteFile(tune);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            tune = tuneBox.Text.Replace("\n", "").Split('\r').ToList().ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fileName = tuneName.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void nextImage_Click(object sender, EventArgs e)
        {
            pageNumber += pageNumber == maxPageNumber ? 0 : 1;
            pageNumberLabel.Text = pageNumber + " out of " + maxPageNumber;

            if (maxPageNumber > 1)
            {
                Image bm;

                var file = file_ext + "_page_" + pageNumber + ".png";

                using (var bmpTemp = new Bitmap(file))
                {
                    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
                }

                assembledTuneDisplay.Image = bm;
            }
        }

        private void previousImage_Click(object sender, EventArgs e)
        {
            pageNumber -= pageNumber == 1 ? 0 : 1;
            pageNumberLabel.Text = pageNumber + " out of " + maxPageNumber;

            if (maxPageNumber > 1)
            {
                Image bm;

                var file = file_ext + "_page_" + pageNumber + ".png";

                using (var bmpTemp = new Bitmap(file))
                {
                    bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / resizeFactor, bmpTemp.Height / resizeFactor));
                }

                assembledTuneDisplay.Image = bm;
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void browse_Click(object sender, EventArgs e)
        {
            //string p = @"C:\Users\Isaac\source\repos\TuneConverter\TuneConverter.Framework.PageImageIO\InputNotes\Allistrum's.txt";
            //string args = string.Format("/e, /select, \"{0}\"", p);

            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = "explorer";
            //info.Arguments = args;
            //Process.Start(info);

            var dlg = new OpenFileDialog()
            {
                InitialDirectory = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/InputNotes",
                Filter = "All Files (*.*) | *.*",
                RestoreDirectory = true
            };

            dlg.ShowDialog();

            if (!dlg.FileName.Equals(""))
            {
                TuneReader tr = new();
                var file = tr.ReadFileForView(dlg.FileName);

                tuneName.Text = file[0][0];
                typeComboBox.Text = file[0][1];
                keyComboBox.Text = file[0][2];

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

                tuneBox.Text = disp;
            }

            //var file = Process.Start("explorer.exe", "/ select, \"" + @"C:\Users\Isaac\source\repos\TuneConverter\TuneConverter.Framework.PageImageIO\InputNotes" + "\"");
        }
    }

}
