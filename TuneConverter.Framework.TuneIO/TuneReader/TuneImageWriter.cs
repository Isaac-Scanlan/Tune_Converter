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

/// <summary>
/// Used to Save the Tune Images generated
/// </summary>
public class TuneImageWriter
{
    private string FilePath => $"C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/{TuneDirectory}/";
    private static string filePathCeili => "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/Ceili_Directory/";
    private string TuneDirectory {  get; set; }

    /// <summary>
    /// Constructor for TuneImageWriter
    /// </summary>
    /// <param name="tuneType">Specifies the directory the Tune is written to</param>
    public TuneImageWriter(TuneType tuneType)
    {
        TuneDirectory = tuneType.ToString();
    }

    /// <summary>
    /// Writes Image to a file
    /// </summary>
    /// <param name="assembledPage"></param>
    /// <param name="fileName"></param>
    /// <param name="directory"></param>
    /// <returns></returns>
    public bool WriteImage(Image<Gray, byte> assembledPage, string fileName, string directory = "")
    {
        string fullDirectory = FilePath + directory;
        if (!Directory.Exists(fullDirectory))
        {
            Directory.CreateDirectory(fullDirectory);
        }

        return CvInvoke.Imwrite($"{fullDirectory}{fileName}.png", assembledPage);
    }
}
