using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneIO.TuneReader;

public partial class TuneReader
{

    private static readonly string _imagePath = "C:/Users/Isaac/source/repos/TuneConverter/TuneConverter.Framework.PageImageIO/InputNotes/";

    public List<List<string>> readFile(String tune)
    {
        var lines = ByNewLine().Split(File.ReadAllText(_imagePath + tune)).ToList();

        var output = arrangeTuneList(lines);

        return output;
    }

    public List<List<string>> arrangeTuneList(List<string> lines)
    {
        List<List<string>> output = new();
        List<string> partlist = new();
        foreach (var line in lines)
        {
            var newLine = ByTab().Replace(line, "");
            if (newLine.Equals("__"))
            {
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
