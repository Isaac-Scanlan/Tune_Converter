using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents.Types;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// The key of a tune
/// </summary>
public record KeyNote : NoteRoot
{
    /// <summary>
    /// The Mode that the tune is written in 
    /// (Major, Minor, Mixolydian, Dorian etc.)
    /// </summary>
    public KeyType Keytype {  get; set; }
}
