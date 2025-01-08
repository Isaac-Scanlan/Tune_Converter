using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneConverter.Framework.TuneComponents.TuneComponents;

/// <summary>
/// A Single note that has a width of one note
/// </summary>
/// <param name="Note"></param>
/// <see cref="NoteGroup"/>
public record Singlet(Note Note) : NoteGroup(1, Note);

