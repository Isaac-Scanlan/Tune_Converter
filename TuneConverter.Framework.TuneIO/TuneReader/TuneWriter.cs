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


namespace TuneConverter.Framework.TuneIO.TuneReader;

public class TuneWriter
{
    public bool WriteImage(Image<Gray, byte> assembledPage, string fileName)
    {
        return CvInvoke.Imwrite("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/" + fileName + ".png", assembledPage);
    }
}
