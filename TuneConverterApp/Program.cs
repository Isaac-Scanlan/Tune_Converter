using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Reflection;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneBuilders;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneComponents.Types;

namespace Main.TuneConverterApp;

public class Program
{
    public static void Main(string[] args)
    {
        List<List<string>> tune =
        [
            new()
            {
                "Kesh Jig",
                "Jig",
                "G Major",
            },
            new()
            {
                "G#'F#G GAB ABA ABD'",
                "E'D'D' G'D'D' E'D'B D'BA",
                "GFG GAB ABA ABD'",
                "E'D'D' G'D'B AGF G_D"
            },
            new()
            {
                "BAB D'BD' E'G'E' D'BG",
                "BAB D'BG ABA AGA",
                "BAB D'BD' E'G'E' D'BD'",
                "G'F'G' A'G'A' B'G'F' G'D'C"
            }
        ];

        TuneAssembler.BarLength = 3;

        var tuneFull = TuneAssembler.AssembleTune(tune);

        PageAssembler assemble = new PageAssembler();
        var tune22 = assemble.CreateTune(tuneFull);

        CvInvoke.Imshow("s", tune22); CvInvoke.WaitKey();

    }
}
