using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TuneConverter.Framework.ABCConverter;

public partial class Converter
{
    public List<string> ConvertToInternalABC(List<string> file)
    {
        List<string> tune = new();

        List<string> prefixes = new() { "T:", "R:", "K:"};

        bool title = true;
        string part = "";
        foreach (string tun in file)
        {
            if (title)
            {
                if (prefixes.Any(x => tun.StartsWith(x)))
                {
                    var line = tun.Remove(0, 3);
                    line = char.ToUpper(line[0]) + line.Substring(1);
                    
                    if (tun.StartsWith("K:"))
                    {
                        var keyLine = line[0] + " " + line.Substring(1);
                        tune.Add(keyLine);
                        tune.Add("__");
                        title = false;
                        continue;
                    }
                    tune.Add(line);
                    continue;

                }

                if (ByTitlePage().IsMatch(tun))
                {
                    continue;
                }
                
            }

            part += tun;

            if (tun.EndsWith(":|"))
            {
                part = part.Replace(":", "");

                switch (tune[1])
                {
                    case "Polka":
                        tune = ConvertPolkaPart(part, tune);
                        break;

                    case "Jig":
                        tune = ConvertJigPart(part, tune);
                        break;

                    case "Reel":
                    case "Hornpipe":
                    case "Barndance":
                        tune = ConvertReelPart(part, tune);
                        break;
                }
                

                tune.Add("__");
                part = "";
            }
        }

        return tune;
    }

    private List<string> ConvertJigPart(string part, List<string> tune)
    {
        var part2 = ByBar().Split(part).ToList();
        part2.RemoveAt(0);
        part2.RemoveAt(part2.Count - 1);

        for (int i = 0; i < part2.Count; i += 4)
        {
            var lin = ConvertString(part2[i]) + " " + ConvertString(part2[i + 1]) + " " + ConvertString(part2[i + 2]) + " " + ConvertString(part2[i + 3]);
            tune.Add(lin);
        }
        return tune;
    }

    private List<string> ConvertReelPart(string part, List<string> tune)
    {
        var part2 = ByBigBar().Split(part).ToList();
        part2.RemoveAt(0);
        part2.RemoveAt(part2.Count - 1);

        var line = "";
        for (int i = 0; i < part2.Count; i ++ )
        {
            var bar = part2[i];
            if (part2[i].Length < 2)
            {
                continue;
            }

            var parts = part2[i].Split(' ').ToList();
            var biggest = parts.OrderByDescending(s => s.Length).First();
            var newParts = parts.Select((x) => parts.IndexOf(x) != parts.IndexOf(biggest)? x: "").ToList();
            string smallest = string.Join("", newParts);

            string first = string.Join(" ", parts.Take(parts.Count / 2));
            string second = string.Join("", parts.Skip(parts.Count / 2));

            if ((biggest.Length - smallest.Length) < Math.Abs(first.Length - second.Length))
            {
                bar = smallest;
                var start = part2.IndexOf(biggest) > part2.Count / 2.0? false : true;
                if (start)
                {
                    bar = biggest + " " + bar;
                }
                else
                {
                    bar += " " + bar;
                }
            }
            else
            {
                bar = first + " " + second;
            }

            

            line += ConvertString(bar);
            line += " ";
            if(i % 2 == 0 && i != 0)
            {
                tune.Add(line);
                line = "";
            }
            
        }
        return tune;
    }

    private List<string> ConvertPolkaPart(string part, List<string> tune)
    {
        var part2 = ByBigBar().Split(part).ToList();
        part2.RemoveAt(0);
        part2.RemoveAt(part2.Count - 1);

        var line = "";
        for (int i = 0; i < part2.Count; i++)
        {
            var bar = part2[i];
            if (part2[i].Length < 2)
            {
                continue;
            }

            string[] parts = part2[i].Split(' ');
            string first = string.Join(" ", parts.Take(parts.Length / 2));
            string second = string.Join(" ", parts.Skip(parts.Length / 2));
            second.Replace(" ", "");

            bar = first + " " + second;

            line += ConvertString(bar);
            
            if (i % 2 == 1 && i != 0)
            {
                tune.Add(line);
                line = "";
                continue;
            }

            line += " ";

        }
        return tune;
    }

    private string ConvertString(string bar)
    {
        string returnBar = "";
        bool isSharp = false;
        int dupTripMax = 0;
        int dupTripCount = 0;
        bool dupTrip = false;

        for (int i = 0; i < bar.Length; i++)
        {
            char c = bar[i];

            if (dupTrip)
            {
                var foo = ByNote().Matches(c + "")
                       .Cast<Match>()
                       .Select(m => m.Value)
                       .ToArray();

                if (dupTripCount >= dupTripMax && foo.Length > 0)
                {
                    dupTrip = false;
                    returnBar += "*";

                }
                else
                {
                   
                    dupTripCount++;
                }
            }

            if (c == '(')
            {
                i++;
                c = bar[i];
                dupTripMax = int.Parse(c + "");
                dupTrip = true;
                returnBar += "*";
                continue;

            }
            if(c == '~')
            {
                i++;
                c = bar[i];
                returnBar += c + "rl"; 
                i++;
                continue;
            }
            if (c == '>')
            {
                returnBar += "-";
                continue;
            }
            if (Char.IsDigit(c))
            {
                foreach(int j in Enumerable.Range(0, (int.Parse(c + "")) - 1))
                {
                    returnBar += "_";
                }
                continue;
            }
            if (Char.IsLower(c) && c != 'c')
            {
                string note = c + "";
                returnBar += note.ToUpper() + "'";
                continue;
            }
            if (c == ',')
            {
                returnBar += "L";
                continue;
            }
            if (c == '^')
            {
                isSharp = true;
                continue;
            }

            returnBar += (c + "").ToUpper();
            if (isSharp)
            {
                returnBar += "#";
                isSharp = false;
            }
        }
        return returnBar;
    }

    [GeneratedRegex(@"^\w:\s.+")]
    private static partial Regex ByTitlePage();

    [GeneratedRegex(@"[|\s]+")]
    private static partial Regex ByBar();

    [GeneratedRegex(@"[|]+")]
    private static partial Regex ByBigBar();

    [GeneratedRegex(@"[A-G]")]
    private static partial Regex ByNote();
}
