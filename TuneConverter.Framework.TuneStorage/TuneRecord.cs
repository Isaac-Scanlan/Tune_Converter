using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneStorage;

internal record TuneRecord
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Key { get; set; }
    public string Tune { get; set; }
}
