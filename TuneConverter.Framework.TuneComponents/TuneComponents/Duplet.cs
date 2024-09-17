using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Two notes that have the width of one note
/// </summary>
/// <param name="Note"></param>
/// <see cref="NoteGroup"/>
public record Duplet(Note Note) : NoteGroup(1, Note)
{
    /// <summary>
    /// First note of duplet
    /// </summary>
    public Note FirstNote { get; set; } = Note;

    /// <summary>
    /// Second note of Duplet
    /// </summary>
    public Note SecondNote { get; set; } = new Note();
}
