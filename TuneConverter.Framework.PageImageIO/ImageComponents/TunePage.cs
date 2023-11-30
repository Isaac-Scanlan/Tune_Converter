using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuneConverter.Framework.TuneComponents;
using TuneConverter.Framework.TuneComponents.TuneComponents;
using Emgu.CV;

namespace TuneConverter.Framework.PageImageIO.ImageComponents;

public record TunePage
{
    public string Title { get; set; }
    public TuneType TuneType { get; set; }
    public Note Key { get; set; }

    public List<Mat> Parts = new List<Mat>();
}

