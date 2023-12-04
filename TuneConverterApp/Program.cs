using Emgu.CV;
using Emgu.CV.ML;
using Emgu.CV.Structure;
using System.Drawing;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneIO.TuneReader;

namespace Main.TuneConverterApp;

public class Program
{
    public static void Main(string[] args)
    {
        //var file = ReadTune("Kesh Jig.txt");
        var file = ReadTune("Step it out Joe.txt");

        var tuneFull = TuneAssembler.AssembleTune(file);

        var assembledPage = AssemblePage(tuneFull);

        DisplayImage(assembledPage);

        WriteImage(assembledPage, tuneFull.Title);
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

    public static bool WriteImage(List<Image<Gray, byte>> assembledPages, string fileName)
    {
        bool outVal = true;
        foreach (var assembledPage in assembledPages.Select((value, i) => (value, i)))
        {
            TuneWriter writer = new();
            if (!writer.WriteImage(assembledPage.value, fileName + "_page_" + (assembledPage.i + 1), fileName) ) 
            {
                outVal = false;
            }
            
        }
        return outVal;
    }
}
