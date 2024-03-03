using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.PageImageIO.ImageComponents;

public enum TuneType
{
    Polka = (13 * 10),
    Slipjig = (14 * 20),
    Jig = (17 * 30),
    Reel = (21 * 40),
    Hornpipe = 21 * 50,
    Barndance = 21 * 60,
    Fling = 21 * 70,
    Slide = (29 * 80),
    Waltz = (29 * 90),
    Mazurka = Waltz * 100
}
