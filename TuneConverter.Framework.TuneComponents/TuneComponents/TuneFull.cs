using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.PageImageIO.ImageComponents;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

public record TuneFull
{
    public string Title { get; set; }
    public KeyNote Key { get; set; }
    public TuneType TuneType { get; set; }
    public int MaxLength { get; set; }
    public int CurrentLength { get; set; } = 0;
    public List<TunePart> tune = [];

    public bool AddPart(TunePart part)
    {
        if (CurrentLength < MaxLength)
        {
            tune.Add(part);
            CurrentLength += 1;
            return true;
        }
        return false;
    }
}
