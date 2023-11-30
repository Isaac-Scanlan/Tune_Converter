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
        //var foo = new NoteImage();

        //Mat[] line2 = [foo.space, foo.A, foo.A, foo.A, foo.space, foo.A, foo.A, foo.A, foo.space, foo.A, foo.A, foo.A, foo.space, foo.A, foo.A, foo.A, foo.space];

        //Mat line = new();

        //CvInvoke.HConcat(line2, line);

        PageAssembler assemble = new PageAssembler();

        var page = assemble.CreateTitlePage("Title", TuneType.Jig, "E minor");

        //CvInvoke.Imshow("s", page);CvInvoke.WaitKey();

        Console.WriteLine(page.Width);

        var note = new Singlet(new Note()
        {
            NoteType = NoteType.C,
            AccidentalType = AccidentalType.Sharp,
            OctaveType = OctaveType.Middle
        });

        //var note1 = assemble.CreateNote(note);
        //var bar = assemble.CreateBar();

        var bar = new TuneBar
        {
            bar = new List<NoteGroup>
            {
                note,note,note
            }
        };

        //var line = assemble.CreateLine(new TuneLine
        //{
        //    line = new List<TuneBar>
        //    {
        //        bar, bar, bar
        //    }
        //});

        var line = new TuneLine
        {
            line = new List<TuneBar>
            {
                bar, bar, bar, bar
            }
        };

        //var part = assemble.CreatePart(;

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

        //CvInvoke.Imshow("s", note1); CvInvoke.WaitKey();
        CvInvoke.Imshow("s", tune); CvInvoke.WaitKey();

        //Console.WriteLine(line.Width);




        //var foo2 = new NoteImage();

        //Mat? foo3 = foo2.GetType().GetField("A").GetValue(foo2) as Mat;

        //CvInvoke.Imshow("s", foo3);CvInvoke.WaitKey();

    }
}