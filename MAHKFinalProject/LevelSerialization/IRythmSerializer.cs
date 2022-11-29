using MAHKFinalProject.LevelSerialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Serialization
{
    public interface IRythmSerializer
    {
        string Serialize(BeatLevel level);
        BeatLevel Deserialize(string notesAsString);
    }
}
