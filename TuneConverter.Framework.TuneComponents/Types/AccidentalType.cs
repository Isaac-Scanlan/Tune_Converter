using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.Types;

/// <summary>
/// Indicates an alteration of a given pitch to a note
/// </summary>
public enum AccidentalType
{
    /// <summary>
    /// Represents a flattened note "♭"
    /// </summary>
    Flat,

    /// <summary>
    /// Represents a sharpened note "♯"
    /// </summary>
    Sharp,

    /// <summary>
    /// Represents a natural note "♮"
    /// (A note that was or flat that had the alteration removed)
    /// </summary>
    Natural,

    /// <summary>
    /// Represents no alterations to a note
    /// </summary>
    None
}

