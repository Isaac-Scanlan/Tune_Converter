using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.Types;

/// <summary>
/// Represents ovtave of the given note
/// </summary>
public enum OctaveType
{
    /// <summary>
    /// Low / Baritone
    /// </summary>
    /// <remarks>(Range shifted to D above bass C to middle C)</remarks>
    Low,

    /// <summary>
    /// Mid-Range / Tenor octave
    /// </summary>
    /// <remarks>(Range shifted to D above middle C to treble C)</remarks>
    Middle,

    /// <summary>
    /// High / Alto octave
    /// </summary>
    /// <remarks>(Range shifted to D above treble C to Top C)</remarks>
    High,

    /// <summary>
    /// Very High / Soprano
    /// </summary>
    /// <remarks>(Range shifted to D above Top C to Double Top C)</remarks>
    HighHigh
}
