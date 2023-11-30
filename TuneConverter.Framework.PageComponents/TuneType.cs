using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.PageImageIO.ImageComponents;

public enum TuneType
{
    Polka = 13,
    Slipjig = 10,
    Jig = 17,
    Slide = Jig,
    Reel = 21,
    Hornpipe = Reel,
    Barndance = Reel,
    Fling = Reel,
    Waltz = 29,
    Mazurka = Waltz
}
