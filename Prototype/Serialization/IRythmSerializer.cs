using Prototype.Rythm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Serialization
{
    public interface IRythmSerializer
    {
        string Serialize(List<Note> notes);
        List<Note> Deserialize(string notesAsString);
    }
}
