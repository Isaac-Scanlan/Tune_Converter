using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.Types;

/// <summary>
/// Represents the type of scale that the tune is written in 
/// </summary>
public enum KeyType
{
    /// <summary>
    /// Major scale (Ionian mode)
    /// </summary>
    Major,

    /// <inheritdoc cref="Major"/>
    major = Major,
    /// <inheritdoc cref="Major"/>
    Maj = Major,
    /// <inheritdoc cref="Major"/>
    maj = Major,

    /// <summary>
    /// Minor scale (Aolian Mode)
    /// </summary>
    Minor,

    /// <inheritdoc cref="Minor"/>
    minor = Minor,
    /// <inheritdoc cref="Minor"/>
    Min = Minor,
    /// <inheritdoc cref="Minor"/>
    min = Minor,

    /// <summary>
    /// Mixolydian scale
    /// </summary>
    Mixolydian,

    /// <inheritdoc cref="Mixolydian"/>
    mixolydian = Mixolydian,
    /// <inheritdoc cref="Mixolydian"/>
    Mix = Mixolydian,
    /// <inheritdoc cref="Mixolydian"/>
    mix = Mixolydian,

    /// <summary>
    /// Dorian Scale
    /// </summary>

    Dorian,
    /// <inheritdoc cref="Dorian"/>
    dorian = Dorian,
    /// <inheritdoc cref="Dorian"/>
    Dor = Dorian,
    /// <inheritdoc cref="Dorian"/>
    dor = Dor,
}
