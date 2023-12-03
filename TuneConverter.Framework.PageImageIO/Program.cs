using Emgu.CV;
using System.Runtime.InteropServices;
using TuneConverter.Framework.PageImageIO.ImageBuilder;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.PageImageIO;

public class Program
{
    public static void Main(string[] args)
    {
        PageAssembler assemble = new PageAssembler();

        var page = assemble.CreateTitlePage("Title", TuneType.Jig, "E minor");

        Console.WriteLine(page.Width);

        var note = new Singlet(new Note()
        {
            NoteType = NoteType.C,
            AccidentalType = AccidentalType.Sharp,
            OctaveType = OctaveType.Middle
        });

        var bar = new TuneBar
        {
            bar = new List<NoteGroup>
            {
                note,note,note
            }
        };

        var line = new TuneLine
        {
            line = new List<TuneBar>
            {
                bar, bar, bar, bar
            }
        };

        var part = new TunePart
        {
            part = new List<TuneLine>
            {
                line, line, line, line
            }
        };

        var tune = assemble.CreateTune(new TuneFull
        {
            Title = "Title",
            TuneType = TuneType.Jig,
            Key = new KeyNote
            {
                NoteType = NoteType.C,
                AccidentalType = AccidentalType.Sharp,
                Keytype = KeyType.Major
            },
            tune = new List<TunePart> { part, part }
        });

        CvInvoke.Imshow("s", tune); CvInvoke.WaitKey();

    }
}