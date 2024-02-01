using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TuneConverter.Framework.PageComponents;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneBuilders;

public static partial class TuneAssembler
{ 
    public static int LineLength { get; set; }
    public static int BarLength { get; set; }
    public static TuneType TuneTypeGlobal { get; set; }

    public static (NoteType, AccidentalType) TuneKey { get; set; }
    public static KeyType Mode { get; set; }

    private static readonly Dictionary<string, RepeatType> _repeatsDict = new() {
        { "Single", RepeatType.Single},
        { "Double", RepeatType.Double},
    };


    public static TuneFull AssembleTune(List<List<string>> rawTune)
    {
        TuneFull tune = new();

        AssembleTitle(tune, rawTune);

        BarLength = BarAndLineLengths[tune.TuneType][0];
        LineLength = BarAndLineLengths[tune.TuneType][1];

        tune.MaxLength = rawTune.Count;
        for (int i = 0; i < rawTune.Count; i++)
        {
            var assembledPart = AssemblePart(rawTune[i]);
            assembledPart.PartNumber = i + 1;
            tune.AddPart(assembledPart);
        }
        //foreach (var part in rawTune)
        //{
        //    var assembledPart = AssemblePart(part);
        //    assembledPart.PartNumber = 1;
        //    tune.AddPart(assembledPart);
        //}
        return tune;
    }

    public static void AssembleTitle(TuneFull tune, List<List<string>> rawTune)
    {
        var titlePage = rawTune[0];
        tune.Title = titlePage[0];
        tune.TuneType = (TuneType)Enum.Parse(typeof(TuneType), titlePage[1]);

        TuneTypeGlobal = tune.TuneType;

        var tuneKey = ByBar().Split(titlePage[2]);
        var notePieces = ByCharacter().Matches(tuneKey[0])
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        tune.Key = new()
        {
            NoteType = (NoteType)Enum.Parse(typeof(NoteType), notePieces[0]),
            AccidentalType = notePieces.Count <= 1 ? Types.AccidentalType.None : notePieces[1].Equals("#") ? Types.AccidentalType.Sharp : Types.AccidentalType.Flat,
            Keytype = (KeyType)Enum.Parse(typeof(KeyType), tuneKey[1])
        };

        Mode = tune.Key.Keytype;
        TuneKey = (tune.Key.NoteType, tune.Key.AccidentalType);

        if (titlePage.Count < 4 || !_repeatsDict.ContainsKey(titlePage[3])) {
            tune.RepeatType = RepeatType.Double;
            rawTune.RemoveAt(0);
            return;
        }

        tune.RepeatType = _repeatsDict[titlePage[3]];

        if (titlePage.Count >= 5)
        {
            tune.Composer = titlePage[4];
        }
        rawTune.RemoveAt(0);
    }

    public static TunePart AssemblePart(List<string> rawTune)
    {
        TunePart part = new();
        part.MaxLength = rawTune.Count;
        foreach (var line in rawTune)
        {
            if (line.StartsWith("|"))
            {
                part.Link = AssembleLine(line, true);
                part.MaxLength--;
            }
            else
            {
                part.AddLine(AssembleLine(line));
            }
            
        }
        return part;
    }

    public static TuneLine AssembleLine(string rawTune, bool ifLink = false)
    {
        var bars = ByBar().Split(rawTune);
        
        TuneLine tuneLine = new()
        {
            MaxLength = ifLink? bars.Length : LineLength,
        };

        foreach (var bar in bars)
        {
            tuneLine.AddNote(AssembleBar(bar));
        }

        return tuneLine;
    }

    public static TuneBar AssembleBar(string bar)
    {
        var dupTrip = ByDupTrip().Matches(bar)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();
        var noteGroups = ByNoteGroup().Split(bar);
        TuneBar tuneBar = new()
        {
            MaxLength = BarLength
        };

        foreach (var noteGroup in noteGroups)
        {
            var test1 = false;
            foreach(var foo1 in dupTrip)
            {
                var notes = Regex.Replace(foo1, @"\*", "");
                if (notes.Equals(noteGroup))
                {
                    test1 = true;
                    break;
                }
            }
            
            if (noteGroup.Length > 1 && test1)
            {
                tuneBar.AddNote(AssembleDupTrip(noteGroup));
            }
            else
            {
                AssembleSingleNote(noteGroup, tuneBar);
            }
            
        }

        return tuneBar;
    }

    public static void AssembleSingleNote(string bar, TuneBar tuneBar)
    {
        var notes = ByNote().Matches(bar)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToArray();


        foreach (var note in notes)
        {
            tuneBar.AddNote(new Singlet(BuildNote(note)));
        }
    }

    public static NoteGroup AssembleDupTrip(string noteGroup)
    {
        
        var notes = ByNote().Matches(noteGroup)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToArray();

        NoteGroup noteGroupOut;

        var notesLeft = TuneTypeGlobal == TuneType.Polka ? 2 : TuneTypeGlobal == TuneType.Jig ? 3 : TuneTypeGlobal == TuneType.Reel ? 4 : 6;

        if (notes.Length == 2)
        {
            noteGroupOut = AssembleDuplet(notes);
        }
        else
        {
            noteGroupOut = AssembleTriplet(notes);
        }

        return noteGroupOut;
    }

    public static Duplet AssembleDuplet(string[] notes)
    {
        var duplet = new Duplet(BuildNote(notes[0]))
        {
            SecondNote = BuildNote(notes[1])
        };
        return duplet;
    }

    public static Triplet AssembleTriplet(string[] notes)
    {
        var triplet = new Triplet(BuildNote(notes[0]))
        {
            SecondNote = BuildNote(notes[1]),
            ThirdNote = BuildNote(notes[2])
        };
        return triplet;
    }

    public static Note BuildNote(string bar)
    {
        List<string> chars = ByCharacter().Matches(bar)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        

        
        var note = new Note()
        {
            NoteType = NoteType[chars[0]],
            
        };

        if (chars[0] == "F")
        {

        }

        if (!chars[0].Equals("_") && !chars[0].Equals("r") && !chars[0].Equals("l"))
        {
            if(Mode == KeyType.minor)
            {
                note.AccidentalType = MinorScaleType[TuneKey][NoteType[chars[0]]];
            }
            else
            {
                note.AccidentalType = ScaleType[TuneKey][NoteType[chars[0]]];
            }
            
        }

        chars.RemoveAt(0);
        
        foreach (var c in chars)
        {
            if (c.Equals("\'") || c.Equals("L"))
            {
                note.OctaveType = OctaveType[c];
            }
            else if (c.Equals("b") || c.Equals("#") || c.Equals("#") || c.Equals("n"))
            {
                note.AccidentalType = AccidentalType[c];
            }
            if (c.Equals("-"))
            {
                note.ShortLongNote = true;
            }
        }

        return note;
    }
    private static Dictionary<TuneType, List<int>> BarAndLineLengths => new()
    {
        { TuneType.Polka     , new(){ 2, 4 } },
        { TuneType.Slipjig   , new(){ 3, 3 } },
        { TuneType.Jig       , new(){ 3, 4 } },
        { TuneType.Slide     , new(){ 3, 4 } },
        { TuneType.Reel      , new(){ 4, 4 } },
        { TuneType.Hornpipe  , new(){ 4, 4 } },
        { TuneType.Barndance , new(){ 4, 4 } },
        { TuneType.Fling     , new(){ 4, 4 } },
        { TuneType.Waltz     , new(){ 6, 4 } },
        { TuneType.Mazurka   , new(){ 6, 4 } },
};

    private static Dictionary<string, NoteType> NoteType => new()
    {
        { "A" , Types.NoteType.A }
        , { "B" , Types.NoteType.B }
        , { "C" , Types.NoteType.C }
        , { "D" , Types.NoteType.D }
        , { "E" , Types.NoteType.E }
        , { "F" , Types.NoteType.F }
        , { "G" , Types.NoteType.G }
        , { "_" , Types.NoteType._ }
        , { "r" , Types.NoteType.r }
        , { "l" , Types.NoteType.l }
    };

    private static Dictionary<string, AccidentalType> AccidentalType => new()
    {
        { "b", Types.AccidentalType.Flat},
        { "#", Types.AccidentalType.Sharp},
        { "n", Types.AccidentalType.Natural}
    };

    private static Dictionary<string, OctaveType> OctaveType => new()
    {
        { "\'", Types.OctaveType.High },
        { "L", Types.OctaveType.Low }
    };

    private static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> ScaleType => new()
    {
           { (Types.NoteType.A, Types.AccidentalType.None ), new() { {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.A, Types.AccidentalType.Flat ), new() { {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.Flat} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.Flat} } }
           , { (Types.NoteType.B, Types.AccidentalType.None ), new() { {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}, {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.B, Types.AccidentalType.Flat ), new() { {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} } }
           , { (Types.NoteType.C, Types.AccidentalType.None ), new() { {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} } }
           , { (Types.NoteType.C, Types.AccidentalType.Sharp), new() { {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}, {Types.NoteType.E,Types.AccidentalType.Sharp}, {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.Sharp}, {Types.NoteType.B,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.D, Types.AccidentalType.None ), new() { {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.D, Types.AccidentalType.Flat ), new() { {Types.NoteType.D,Types.AccidentalType.Flat} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.Flat} , {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} } }
           , { (Types.NoteType.E, Types.AccidentalType.None ), new() { {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.E, Types.AccidentalType.Flat ), new() { {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} } }
           , { (Types.NoteType.F, Types.AccidentalType.None ), new() { {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} } }
           , { (Types.NoteType.F, Types.AccidentalType.Sharp), new() { {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.Sharp}, {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}, {Types.NoteType.E,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.G, Types.AccidentalType.None ), new() { {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}} }
           , { (Types.NoteType.G, Types.AccidentalType.Flat ), new() { {Types.NoteType.G,Types.AccidentalType.Flat} , {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.Flat} , {Types.NoteType.D,Types.AccidentalType.Flat} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} } }

    };

    private static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> MinorScaleType => new()
    {
        { (Types.NoteType.A, Types.AccidentalType.None ), new() { {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} } }
        , { (Types.NoteType.A, Types.AccidentalType.Sharp), new() { {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}, {Types.NoteType.E,Types.AccidentalType.Sharp}, {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.Sharp}, {Types.NoteType.B,Types.AccidentalType.Sharp}} }
        , { (Types.NoteType.B, Types.AccidentalType.Flat ), new() { {Types.NoteType.D,Types.AccidentalType.Flat} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.Flat} , {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} } }
        , { (Types.NoteType.B, Types.AccidentalType.None ), new() { {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}} }
        , { (Types.NoteType.C, Types.AccidentalType.None ), new() { {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} } }
        , { (Types.NoteType.C, Types.AccidentalType.Sharp), new() { {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}} }
        , { (Types.NoteType.D, Types.AccidentalType.None ), new() { {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} } }
        , { (Types.NoteType.D, Types.AccidentalType.Sharp), new() { {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.Sharp}, {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}, {Types.NoteType.E,Types.AccidentalType.Sharp}} }
        , { (Types.NoteType.E, Types.AccidentalType.Flat ), new() { {Types.NoteType.G,Types.AccidentalType.Flat} , {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.Flat} , {Types.NoteType.D,Types.AccidentalType.Flat} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} } }
        , { (Types.NoteType.E, Types.AccidentalType.None ), new() { {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}} }
        , { (Types.NoteType.F, Types.AccidentalType.None ), new() { {Types.NoteType.A,Types.AccidentalType.Flat} , {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.Flat} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.Flat} } }
        , { (Types.NoteType.F, Types.AccidentalType.Sharp), new() { {Types.NoteType.A,Types.AccidentalType.None} , {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}} }
        , { (Types.NoteType.G, Types.AccidentalType.None ), new() { {Types.NoteType.B,Types.AccidentalType.Flat} , {Types.NoteType.C,Types.AccidentalType.None} , {Types.NoteType.D,Types.AccidentalType.None} , {Types.NoteType.E,Types.AccidentalType.Flat} , {Types.NoteType.F,Types.AccidentalType.None} , {Types.NoteType.G,Types.AccidentalType.None} , {Types.NoteType.A,Types.AccidentalType.None} } }
        , { (Types.NoteType.G, Types.AccidentalType.Sharp), new() { {Types.NoteType.B,Types.AccidentalType.None} , {Types.NoteType.C,Types.AccidentalType.Sharp}, {Types.NoteType.D,Types.AccidentalType.Sharp}, {Types.NoteType.E,Types.AccidentalType.None} , {Types.NoteType.F,Types.AccidentalType.Sharp}, {Types.NoteType.G,Types.AccidentalType.Sharp}, {Types.NoteType.A,Types.AccidentalType.Sharp}} }

    };


    [GeneratedRegex(@"\s+")]
    private static partial Regex ByBar();


    [GeneratedRegex(@"[\*]")]
    private static partial Regex ByNoteGroup();


    [GeneratedRegex(@"\*[\w#b']+\*")]
    private static partial Regex ByDupTrip();


    [GeneratedRegex(@"[A-G_rl][^A-G_rl]*")]
    private static partial Regex ByNote();


    [GeneratedRegex(@".")]
    private static partial Regex ByCharacter();
}

