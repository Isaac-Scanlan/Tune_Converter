using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.PageComponents;
using TuneConverter.Framework.PageImageIO.ImageComponents;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// Represents a full tune
/// </summary>
public record TuneFull
{
    /// <summary>
    /// Title of the Tune
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// The key of the tune
    /// </summary>
    public KeyNote? Key { get; set; }

    /// <summary>
    /// The type of dance tune the tune is
    /// </summary>
    public TuneType TuneType { get; set; }

    /// <summary>
    /// How many times each part of the tune is repeated
    /// </summary>
    public RepeatType RepeatType { get; set; }

    /// <summary>
    /// The Composer of the tune
    /// </summary>
    public string Composer { get; set; } = "";

    /// <summary>
    /// Intended length of a bar in notes
    /// </summary>
    public int MaxLength { get; set; }

    /// <summary>
    /// Current length of a bar
    /// </summary>
    public int CurrentLength { get; set; } = 0;

    /// <summary>
    /// List of Notes in a bar
    /// </summary>
    public List<TunePart> tune = [];

    /// <summary>
    /// Adds Parts to a Tune but stops after the maximum limit has been reached
    /// </summary>
    /// <param name="part"></param>
    /// <returns></returns>
    public bool AddPart(TunePart part)
    {
        if (CurrentLength < MaxLength)
        {
            tune.Add(part);
            CurrentLength += 1;
            return true;
        }
        return false;
    }
}
