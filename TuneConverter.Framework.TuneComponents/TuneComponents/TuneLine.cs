using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Represents a Line of music in a Tune
/// </summary>
public record TuneLine
{
    /// <summary>
    /// Intended length of a line in "bars"
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Current length of a line
    /// </summary>
    public int CurrentLength { get; set; } = 0;

    /// <summary>
    /// List of bars in a line
    /// </summary>
    public List<TuneBar> line = [];

    /// <summary>
    /// Adds Bars to a line but stops after the maimum limit has been reached
    /// </summary>
    /// <param name="bar"></param>
    /// <returns></returns>
    public bool AddNote(TuneBar bar)
    {
        if (CurrentLength < MaxLength)
        {
            line.Add(bar);
            CurrentLength += 1;
            return true;
        }
        return false;
    }
}
