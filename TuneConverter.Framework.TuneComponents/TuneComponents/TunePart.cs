﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

public record TunePart
{
    public int PartNumber { get; set; }
    public int MaxLength { get; set; }
    public int CurrentLength { get; set; }
    public List<TuneLine> part = [];
    public TuneLine Link { get; set; } = new TuneLine();

    public bool AddLine(TuneLine line)
    { 
        if (CurrentLength < MaxLength)
        {
            part.Add(line);
            CurrentLength += 1;
            return true;
        }
        return false;
    }
}
