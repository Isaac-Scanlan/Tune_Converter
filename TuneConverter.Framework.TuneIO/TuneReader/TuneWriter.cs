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


namespace TuneConverter.Framework.TuneIO.TuneReader;

public class TuneWriter
{
    string filePath => "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + tuneDirectory + "/";
    string filePathCeili => "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/Ceili_Directory/";
    string tuneDirectory {  get; set; }

    public TuneWriter(TuneType tuneType)
    {
        tuneDirectory = tuneType.ToString();
    }
    public bool WriteImage(Image<Gray, byte> assembledPage, string fileName, string directory = "")
    {
        if (!Directory.Exists(filePath + directory))
        {
            Directory.CreateDirectory(filePath + directory);
        }
        //if (!Directory.Exists(filePathCeili + directory))
        //{
        //    Directory.CreateDirectory(filePathCeili + directory);
        //}

        //var foo = CvInvoke.Imwrite(filePathCeili + directory + "/" + fileName + ".png", assembledPage);
        return CvInvoke.Imwrite(filePath + directory + "/" + fileName + ".png", assembledPage);
    }
}
