using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Rythm
{

    //Maybe change to Wave
    public struct Note
    {
        public int Count;
        public float Beat;
        
        public Note(float beat, int count = 1)
        {
            Count = count;
            Beat = beat;
        }
    }
}
