using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using System.Reflection;


namespace TuneConverter.Framework.TuneIO.TuneReader;

public class TuneImageWriter
{
    string filePath => "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + tuneDirectory + "/";
    //public static string runningDirectory = Directory.GetCurrentDirectory();
    //public string filePath => Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(),
    //    Path.Combine(runningDirectory, "..", "..", "..", "..", "TuneConverter.Framework.PageImageIO", "OutImages", tuneDirectory)));
    string filePathCeili => "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/Ceili_Directory/";
    string tuneDirectory {  get; set; }

    public TuneImageWriter(TuneType tuneType)
    {
        tuneDirectory = tuneType.ToString();
    }
    public bool WriteImage(Image<Gray, byte> assembledPage, string fileName, string directory = "")
    {
        if (!Directory.Exists(filePath + directory))
        {
            Directory.CreateDirectory(filePath + directory);
        }

        return CvInvoke.Imwrite(filePath + directory + "/" + fileName + ".png", assembledPage);
    }
}
