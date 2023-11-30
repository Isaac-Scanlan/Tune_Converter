using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

public record Duplet(Note Note) : NoteGroup(1, Note)
{
    public Note FirstNote { get; set; } = Note;
    public Note SecondNote { get; set; } = new Note();
}
