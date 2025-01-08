using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Represents the notes used to form the bars of a tune
/// </summary>
public record Note: NoteRoot
{
    /// <summary>
    ///  The octave of the tune (Middle, High or Low)
    /// </summary>
    public OctaveType OctaveType { get; set; } = OctaveType.Middle;

    /// <summary>
    /// If this note contains a short "Long Note"
    /// </summary>
    public bool ShortLongNote { get; set; } = false;
}

