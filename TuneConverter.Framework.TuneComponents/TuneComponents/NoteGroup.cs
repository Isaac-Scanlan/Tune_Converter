using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// The Root object for a Duplet or a triplet
/// </summary>
/// <param name="NoteLength"></param>
/// <param name="Note"></param>
public record NoteGroup(int NoteLength, Note Note) { }