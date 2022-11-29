using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.RhythmComponents
{
    internal class UnusedNote
    {
        public float Beat { get; set; }


    }

    public enum NoteStatus
    {
        NotSpawned,
        Spawned,
        Active,
        Ended
    }
}
