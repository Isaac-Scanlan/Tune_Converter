using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.Types;

/// <summary>
/// Represents the full notes to be added to a page
/// </summary>
public enum NoteType
{
    /// <summary>
    /// The note of A
    /// </summary>
    A,

    /// <summary>
    /// The note of B
    /// </summary>
    B,

    /// <summary>
    /// The note of C
    /// </summary>
    C,

    /// <summary>
    /// The note of D
    /// </summary>
    D,

    /// <summary>
    /// The note of E
    /// </summary>
    E,

    /// <summary>
    /// The note of F
    /// </summary>
    F,

    /// <summary>
    /// The note of G
    /// </summary>
    G,

    /// <summary>
    /// A short "Long Note"
    /// </summary>
    _,

    /// <summary>
    /// First half of a roll
    /// </summary>
    r,

    /// <summary>
    /// second half of a roll
    /// </summary>
    l
}

