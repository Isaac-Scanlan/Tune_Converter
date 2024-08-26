using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using Emgu.CV;

namespace TuneConverter.Framework.PageImageIO.ImageComponents;

/// <summary>
/// Represents a page of a tune, containing the title, tune type, key, and the visual representation of the tune parts.
/// </summary>
public record TunePage
{
    /// <summary>
    /// Gets or sets the title of the tune.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the type of the tune (e.g., Reel, Jig, etc.).
    /// </summary>
    public TuneType TuneType { get; set; }

    /// <summary>
    /// Gets or sets the key of the tune.
    /// </summary>
    public Note Key { get; set; }

    /// <summary>
    /// Gets or sets the list of visual representations of the tune parts, each as a matrix image.
    /// </summary>
    public List<Mat> Parts { get; set; } = new List<Mat>();
}
