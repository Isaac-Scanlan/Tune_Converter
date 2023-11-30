using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;
public record TuneBar
{
    public int MaxLength { get; set; }
    public int CurrentLength { get; set; } = 0;
    public List<NoteGroup> bar = [];

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

