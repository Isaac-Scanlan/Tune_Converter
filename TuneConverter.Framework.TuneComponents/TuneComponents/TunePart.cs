using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Represents a Part of a Tune
/// </summary>
public record TunePart
{
    /// <summary>
    /// The parts number
    /// </summary>
    public int PartNumber { get; set; }

    /// <summary>
    /// Intended length of a part in "lines"
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Current length of a part
    /// </summary>
    public int CurrentLength { get; set; } = 0;

    /// <summary>
    /// List of parts in a tune
    /// </summary>
    public List<TuneLine> part = [];

    /// <summary>
    /// The music Link to the next part
    /// </summary>
    public TuneLine Link { get; set; } = new TuneLine();

    /// <summary>
    /// Adds Lines to a part but stops after the maimum limit has been reached
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    public bool AddLine(TuneLine line)
    { 
        if (CurrentLength < MaxLength)
        {
            part.Add(line);
            CurrentLength += 1;
            return true;
        }
        return false;
    }
}
