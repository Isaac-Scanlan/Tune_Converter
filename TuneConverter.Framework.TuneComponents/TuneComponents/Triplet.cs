using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Three notes that have the width of two notes
/// </summary>
/// <param name="Note"></param>
/// <see cref="NoteGroup"/>
public record Triplet(Note Note) : NoteGroup(2, Note)
{
    /// <summary>
    /// First note in triplet
    /// </summary>
    public Note? FirstNote { get; set; } = Note;

    /// <summary>
    /// Second note in triplet
    /// </summary>
    public Note? SecondNote { get; set; }

    /// <summary>
    /// Third note in triplet
    /// </summary>
    public Note? ThirdNote { get; set; }
}

