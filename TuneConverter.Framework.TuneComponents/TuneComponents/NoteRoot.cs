using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Represents a musical note
/// </summary>
public record NoteRoot
{
    /// <summary>
    /// 
    /// </summary>
    public NoteType NoteType { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public AccidentalType AccidentalType { get; set; } = AccidentalType.None;
}
