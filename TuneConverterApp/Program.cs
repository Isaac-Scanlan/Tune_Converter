using Emgu.CV;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using System.Drawing;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;

namespace Main.TuneConverterApp;

public class Program
{
    public static void Main(string[] args)
    {
        var file = ReadTune("The Britches Sull of Stitches.txt");

        var tuneFull = TuneAssembler.AssembleTune(file);

        var assembledPage = AssemblePage(tuneFull);

        DisplayImage(assembledPage);

        WriteImage(assembledPage, tuneFull.Title, tuneFull.TuneType);
    }

    public static List<List<string>> ReadTune(string fileName)
    {
        TuneReader reader = new();
        return reader.readFile(fileName);
    }

    public static List<Image<Gray, byte>> AssemblePage(TuneFull tuneFull)
    {
        PageAssembler assemble = new PageAssembler();
        return assemble.CreateTune(tuneFull);
    }

    public static void DisplayImage(List<Image<Gray, byte>> assembledPages)
    {
        foreach (var assembledPage in assembledPages)
        {
            Image<Gray, byte> resizedPage = new(assembledPage.Width, assembledPage.Height);

            CvInvoke.Resize(assembledPage, resizedPage, new Size(), 0.5, 0.5);

            CvInvoke.Imshow("s", resizedPage);
            CvInvoke.WaitKey();
        }
    }

    public static bool WriteImage(List<Image<Gray, byte>> assembledPages, string fileName, TuneType tuneType)
    {
        bool outVal = true;
        foreach (var assembledPage in assembledPages.Select((value, i) => (value, i)))
        {
            var fileDirec = assembledPages.Count > 1 ? fileName : "";
            var pageNum = assembledPages.Count > 1 ? "_page_" + (assembledPage.i + 1) : "";

            TuneWriter writer = new(tuneType);
            if (!writer.WriteImage(assembledPage.value, fileName + pageNum, fileDirec) ) 
            {
                outVal = false;
            }
            
        }
        return outVal;
    }
}
