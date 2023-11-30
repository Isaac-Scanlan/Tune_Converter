using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

public record TuneLine
{
    public int MaxLength { get; set; }
    public int CurrentLength { get; set; }
    public List<TuneBar> line = [];

    public bool AddNote(TuneBar bar)
    {
        if (CurrentLength < MaxLength)
        {
            line.Add(bar);
            CurrentLength += 1;
            return true;
        }
        return false;
    }
}
