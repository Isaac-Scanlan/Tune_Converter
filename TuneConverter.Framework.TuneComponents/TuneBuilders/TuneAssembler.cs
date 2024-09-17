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

/// <summary>
/// Takes a Tune that has been read into a List and returns a Tune object
/// </summary>
public static partial class TuneAssembler
{ 
    private static int LineLength { get; set; }
    private static int BarLength { get; set; }

    private static (NoteType, AccidentalType) TuneKey { get; set; }
    private static KeyType Mode { get; set; }

    private static readonly Dictionary<string, RepeatType> _repeatsDict = new() {
        { "Single", RepeatType.Single},
        { "Double", RepeatType.Double},
    };

    private static Dictionary<string, NoteType> NoteTypeDict = [];
    private static Dictionary<string, AccidentalType> AccidentalTypeDict = [];
    private static Dictionary<string, OctaveType> OctaveTypeDict = [];
    private static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> ScaleType = [];
    private static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> MinorScaleType = [];
    private static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> DorianScaleType = [];
    private static Dictionary<(NoteType, AccidentalType), Dictionary<NoteType, AccidentalType>> MixolydianScaleType = [];

    /// <summary>
    /// Takes a tune that was read and converts it into a TuneFull object
    /// </summary>
    /// <param name="rawTune"></param>
    /// <returns>Processed full tune</returns>
    public static TuneFull AssembleTune(List<List<string>> rawTune)
    {
        TuneAssemblerConfig.LoadFromJson();
        NoteTypeDict = TuneAssemblerConfig.NoteType;
        AccidentalTypeDict = TuneAssemblerConfig.AccidentalType;
        OctaveTypeDict = TuneAssemblerConfig.OctaveType;
        ScaleType = TuneAssemblerConfig.ScaleType;
        MinorScaleType = TuneAssemblerConfig.MinorScaleType;
        DorianScaleType = TuneAssemblerConfig.DorianScaleType;
        MixolydianScaleType = TuneAssemblerConfig.MixolydianScaleType;

        TuneFull tune = new();

        AssembleTitle(tune, rawTune);

        BarLength = TuneAssemblerConfig.BarAndLineLengths[tune.TuneType][0];
        LineLength = TuneAssemblerConfig.BarAndLineLengths[tune.TuneType][1];

        tune.MaxLength = rawTune.Count;

        int partNumber = 1;
        foreach (var rawPart in rawTune)
        {
            var assembledPart = AssemblePart(rawPart);
            assembledPart.PartNumber = partNumber++;
            tune.AddPart(assembledPart);
        }

        return tune;
    }

    private static void AssembleTitle(TuneFull tune, List<List<string>> rawTune)
    {
        List<string> titlePage = rawTune[0];

        tune.Title = titlePage[0];
        tune.TuneType = Enum.Parse<TuneType>(titlePage[1]);

        var tuneKeyParts = Regex.Split(titlePage[2], @"\s+");
        var notePieces = tuneKeyParts[0].Select(c => c.ToString()).ToList();

        tune.Key = new()
        {
            NoteType = Enum.Parse<NoteType>(notePieces[0]),
            AccidentalType = notePieces.Count > 1 ? (notePieces[1] == "#" ? AccidentalType.Sharp : AccidentalType.Flat) : AccidentalType.None,
            Keytype = Enum.Parse<KeyType>(tuneKeyParts[1])
        };

        Mode = tune.Key.Keytype;
        TuneKey = (tune.Key.NoteType, tune.Key.AccidentalType);

        if (titlePage.Count >= 4 && _repeatsDict.TryGetValue(titlePage[3], out var repeatType))
        {
            tune.RepeatType = repeatType;
        }
        else
        {
            tune.RepeatType = RepeatType.Double;
            rawTune.RemoveAt(0);
            return;
        }

        if (titlePage.Count >= 5)
        {
            tune.Composer = titlePage[4];
        }

        rawTune.RemoveAt(0);
    }

    private static TunePart AssemblePart(List<string> rawTune)
    {
        char linkChar = '|'; 
        int maxLength = rawTune.Count(line => !line.StartsWith(linkChar));

        TunePart part = new() { MaxLength = maxLength };

        foreach (var line in rawTune)
        {
            if (line.StartsWith(linkChar))
            {
                part.Link = AssembleLine(line, true);
            }
            else
            {
                part.AddLine(AssembleLine(line));
            }
        }

        return part;
    }


    private static TuneLine AssembleLine(string rawTune, bool ifLink = false)
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

    private static TuneBar AssembleBar(string bar)
    {
        var dupTrip = ByDupTrip()
            .Matches(bar)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToList();

        TuneBar tuneBar = new() { MaxLength = BarLength };

        foreach (var noteGroup in bar.Split('*', StringSplitOptions.RemoveEmptyEntries))
        {
            bool isDupletOrTriplet = dupTrip
                .Select(foo => foo.Replace("*", ""))
                .Contains(noteGroup);

            if (noteGroup.Length > 1 && isDupletOrTriplet)
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


    private static void AssembleSingleNote(string bar, TuneBar tuneBar)
    {
        foreach (var note in ByNote().Matches(bar).Cast<Match>().Select(m => m.Value))
        {
            tuneBar.AddNote(new Singlet(BuildNote(note)));
        }
    }

    private static NoteGroup AssembleDupTrip(string noteGroup)
    {
        var notes = ByNote()
            .Matches(noteGroup)
            .Cast<Match>()
            .Select(m => m.Value)
            .ToArray();

        return notes.Length == 2 ? AssembleDuplet(notes) : AssembleTriplet(notes);
    }


    private static Duplet AssembleDuplet(string[] notes)
    {
        var duplet = new Duplet(BuildNote(notes[0]))
        {
            SecondNote = BuildNote(notes[1])
        };
        return duplet;
    }

    private static Triplet AssembleTriplet(string[] notes)
    {
        var triplet = new Triplet(BuildNote(notes[0]))
        {
            SecondNote = BuildNote(notes[1]),
            ThirdNote = BuildNote(notes[2])
        };
        return triplet;
    }

    private static Note BuildNote(string noteString)
    {
        string[] letters = noteString.ToCharArray().Select(c => c.ToString()).ToArray();

        string noteLetter = letters[0];
        char noteChar = noteLetter[0];

        var note = new Note
        {
            NoteType = NoteTypeDict[noteLetter],
        };

        if (noteChar >= 'A' && noteChar <= 'G')
        {
            note.AccidentalType = Mode switch
            {
                KeyType.minor => MinorScaleType[TuneKey][NoteTypeDict[noteLetter]],
                KeyType.mix => MixolydianScaleType[TuneKey][NoteTypeDict[noteLetter]],
                KeyType.Dor => DorianScaleType[TuneKey][NoteTypeDict[noteLetter]],
                _ => ScaleType[TuneKey][NoteTypeDict[noteLetter]]
            };
        }
        
        for (int i = 1; i < letters.Length; i++)
        {
            string letter = letters[i];
            if (OctaveTypeDict.TryGetValue(letter, out OctaveType octaveTypeValue))
            {
                note.OctaveType = octaveTypeValue;
            }
            else if (AccidentalTypeDict.TryGetValue(letter, out AccidentalType accidentalTypeValue))
            {
                note.AccidentalType = accidentalTypeValue;
            }
            if (letter == "-")
            {
                note.ShortLongNote = true;
            }
        }

        return note;
    }    


    [GeneratedRegex(@"\s+")]
    private static partial Regex ByBar();

    [GeneratedRegex(@"\*[\w#b']+\*")]
    private static partial Regex ByDupTrip();

    [GeneratedRegex(@"[A-G_rl][^A-G_rl]*")]
    private static partial Regex ByNote();
}

