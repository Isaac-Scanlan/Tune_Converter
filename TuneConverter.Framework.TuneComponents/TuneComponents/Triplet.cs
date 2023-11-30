using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;
public record Triplet(Note Note) : NoteGroup(2, Note)
{
    public Note? FirstNote { get; set; } = Note;
    public Note? SecondNote { get; set; }
    public Note? ThirdNote { get; set; }
}

