using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Represents a Bar of music in a Tune
/// </summary>
public record TuneBar
{
    /// <summary>
    /// Intended length of a bar in "notes"
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Current length of a bar
    /// </summary>
    public int CurrentLength { get; set; } = 0;

    /// <summary>
    /// List of Notes in a bar
    /// </summary>
    public List<NoteGroup> bar = [];

    /// <summary>
    /// Adds Notes to a bar but stops after the maimum limit has been reached
    /// </summary>
    /// <param name="note"></param>
    /// <returns></returns>
    public bool AddNote(NoteGroup note)
    {
        if (CurrentLength < MaxLength)
        {
            bar.Add(note);
            CurrentLength += note.NoteLength;
            return true;
        }
        return false;
    }
}

