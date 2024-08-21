using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.PageComponents;

/// <summary>
/// Used internally to represent how many times the part of a tune is repeated
/// </summary>
public enum RepeatType
{
    /// <summary>
    /// A Single type tune
    /// </summary>
    Single,

    /// <summary>
    /// A Double type tune
    /// </summary>
    Double,

    /// <summary>
    /// A Triple type tune
    /// </summary>
    Triple
}
