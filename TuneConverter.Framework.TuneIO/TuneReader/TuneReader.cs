using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneIO.TuneReader;

/// <summary>
/// Reads tune .txt and returns a List
/// </summary>
public partial class TuneReader
{
    private static readonly string _notesPath = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/InputNotes/";

    /// <summary>
    /// Reads a tune from a file and returns the converted tune List
    /// </summary>
    /// <param name="tune"></param>
    /// <returns></returns>
    public static List<List<string>> ReadFile(string tune)
    {
        var lines = ByNewLine().Split(File.ReadAllText(_notesPath + tune)).ToList();
        TuneWriter tw = new();
        tw.WriteFile(lines);

        var output = ArrangeTuneList(lines);

        return output;
    }

    /// <summary>
    /// Reads a tune from a file and returns the converted tune List
    /// </summary>
    /// <param name="tune"></param>
    /// <returns></returns>
    public static List<List<string>> ReadFileForView(string tune)
    {
        var lines = ByNewLine().Split(File.ReadAllText(tune)).ToList();

        return ArrangeTuneList(lines);
    }

    /// <summary>
    /// Splits read tune into Tune header and parts
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public static List<List<string>> ArrangeTuneList(List<string> lines)
    {
        List<List<string>> output = new();
        List<string> partlist = new();
        foreach (var line in lines)
        {
            var newLine = ByTab().Replace(line, "");
            if (newLine.StartsWith("__"))
            {
                //if
                output.Add(partlist);
                partlist = new();
                continue;
            }
            partlist.Add(newLine);
        }

        return output;
    }

    [GeneratedRegex(@"\r\n")]
    private static partial Regex ByNewLine();
    [GeneratedRegex(@"\t")]
    private static partial Regex ByTab();
}
