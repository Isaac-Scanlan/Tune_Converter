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


namespace TuneConverterAppInterface
{
    public partial class Form1 : Form
    {
        string fileName = string.Empty;
        List<string> tune = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        { 
            tune = tuneBox.Text.Replace("\n", "").Split('\r').ToList().ToList();
            fileName = tuneName.Text;


            Process p = new Process();
            ProcessStartInfo ps = new (Application.StartupPath.ToString() + "Main.TuneConverterApp.exe", tune);
            p.StartInfo = ps;

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            //string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            Image bm;

            using (var bmpTemp = new Bitmap("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + tune[1] + "/" + fileName + ".png"))
            {
                bm = new Bitmap(bmpTemp, new Size(bmpTemp.Width / 4, bmpTemp.Height / 4));
            }

            assembledTuneDisplay.Image = bm;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            tune = tuneBox.Text.Replace("\n","").Split('\r').ToList().ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            fileName = tuneName.Text;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

    }

}
