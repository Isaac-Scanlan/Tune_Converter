using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TuneConverter.Framework.PageImageIO.ImageComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneBuilders;

public static partial class TuneAssembler
{
    public static int LineLength { get; set; } = 4;
    public static int BarLength { get; set; } = 3;

    public static TuneFull AssembleTune(List<List<string>> rawTune)
    {
        TuneFull tune = new();

        AssembleTitle(tune, rawTune);

        tune.MaxLength = rawTune.Count;
        foreach (var line in rawTune)
        {
            tune.AddPart(AssemblePart(line));
        }
        return tune;
    }

    public static void AssembleTitle(TuneFull tune, List<List<string>> rawTune)
    {
        var titlePage = rawTune[0];
        tune.Title = titlePage[0];
        tune.TuneType = (TuneType)Enum.Parse(typeof(TuneType), titlePage[1]);

        var tuneKey = ByBar().Split(titlePage[2]);
        var notePieces = ByCharacter().Matches(tuneKey[0])
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        tune.Key = new()
        {
            NoteType = (NoteType)Enum.Parse(typeof(NoteType), notePieces[0]),
            AccidentalType = notePieces.Count <= 1 ? AccidentalType.Natural : notePieces[1].Equals("#") ? AccidentalType.Sharp : AccidentalType.Flat,
            Keytype = (KeyType)Enum.Parse(typeof(KeyType), tuneKey[1])
        };

        rawTune.RemoveAt(0);
    }

    public static TunePart AssemblePart(List<string> rawTune)
    {
        TunePart part = new();
        part.MaxLength = LineLength;
        foreach (var line in rawTune)
        {
            part.AddLine(AssembleLine(line));
        }
        return part;
    }

    public static TuneLine AssembleLine(string rawTune)
    {
        var bars = ByBar().Split(rawTune);
        TuneLine tuneLine = new()
        {
            MaxLength = LineLength,
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
                if (foo1.Contains(noteGroup))
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

        if (notes.Length == 2)
        {
            noteGroupOut = AssembleDuplet(notes);
        }
        else// if (notes.Length == 3)
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
        //List<string> chars = [.. ByCharacter().Split(bar)];
        List<string> chars = ByCharacter().Matches(bar)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        var note = new Note()
        {
            NoteType = noteType[chars[0]]
        };
        chars.RemoveAt(0);
        
        foreach (var c in chars)
        {
            if (c.Equals("\'") || c.Equals("L"))
            {
                note.OctaveType = octaveType[c];
            }
            else if (c.Equals("\b") || c.Equals("#") || c.Equals("#"))
            {
                note.AccidentalType = accidentalType[c];
            }
        }

        return note;
    }

    private static Dictionary<string, NoteType> noteType => new()
    {
        { "A" , NoteType.A }
        , { "B" , NoteType.B }
        , { "C" , NoteType.C }
        , { "D" , NoteType.D }
        , { "E" , NoteType.E }
        , { "F" , NoteType.F }
        , { "G" , NoteType.G }
        , { "_" , NoteType._ }
    };

    private static Dictionary<string, AccidentalType> accidentalType => new()
    {
        { "b", AccidentalType.Flat},
        { "#", AccidentalType.Sharp},
        { "n", AccidentalType.Natural}
    };

    private static Dictionary<string, OctaveType> octaveType => new()
    {
        { "\'", OctaveType.High },
        { "L", OctaveType.Low }
    };


    [GeneratedRegex(@"\s+")]
    private static partial Regex ByBar();


    [GeneratedRegex(@"[\*]")]
    private static partial Regex ByNoteGroup();


    [GeneratedRegex(@"\*[\w#]+\*")]
    private static partial Regex ByDupTrip();


    [GeneratedRegex(@"[A-Ga-g_][^A-Ga-g_]*")]
    private static partial Regex ByNote();


    [GeneratedRegex(@".")]
    private static partial Regex ByCharacter();
}

