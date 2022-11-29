using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Rythm
{

    //Maybe change to Wave
    public struct NoteData
    {
        public int Count;
        public float Beat;
        
        public NoteData(float beat, int count = 1)
        {
            Count = count;
            Beat = beat;
        }
    }
}
