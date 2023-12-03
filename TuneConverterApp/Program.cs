using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.TuneComponents.TuneBuilders;

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
                "GF#G GAB ABA ABD'",
                "E'D'D' G'D'D' E'D'B D'BA",
                "GF#G GAB ABA ABD'",
                "E'D'D' G'D'B AGF G_D",
                "|GD'C"
            },
            new()
            {
                "BAB D'BD' E'G'E' D'BG",
                "BAB D'BG ABA AGA",
                "BAB D'BD' E'G'E' D'BD'",
                "G'F#'G' A'G'A' B'G'F#' G'D'C",
                "|G_D"
            }
        ];

        List<List<string>> tune2 =
        [
            new()
            {
                "Em Polka",
                "Polka",
                "E minor",
            },
            new()
            {
                "GE ED BLE EF",
                "GE ED GA BA",
                "GE ED BLE EA",
                "BA GF E_ BA"
            },
            new()
            {
                "BE' E'F' E'B BA",
                "BE' E'F' E'_ E'F'",
                "G'_ F'_ E'B BA",
                "BE GA B_ BA"
            }
        ];

        TuneAssembler.BarLength = 3;

        var tuneFull = TuneAssembler.AssembleTune(tune2);

        PageAssembler assemble = new PageAssembler();
        var tune22 = assemble.CreateTune(tuneFull);

        Image<Gray, byte> image23 = new Image<Gray, byte>(tune22.Width, tune22.Height);
        Size size = new Size();
        CvInvoke.Resize(tune22, image23, size, 0.75, 0.75);

        CvInvoke.Imshow("s", image23); CvInvoke.WaitKey();
        //CvInvoke.Imshow("s", tune22); CvInvoke.WaitKey();

        CvInvoke.Imwrite("C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/OutImages/"+ tuneFull.Title+ ".png", tune22);

    }
}
