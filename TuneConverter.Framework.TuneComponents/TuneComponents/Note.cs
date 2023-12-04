using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

public record Note: NoteRoot
{
    public OctaveType OctaveType { get; set; } = OctaveType.Middle;

    public bool ShortLongNote { get; set; } = false;
}

