using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

public record NoteRoot
{
    public NoteType NoteType { get; set; }
    public AccidentalType AccidentalType { get; set; } = AccidentalType.None;
}
