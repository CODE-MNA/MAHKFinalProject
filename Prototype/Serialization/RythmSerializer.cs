﻿using Prototype.Rythm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Serialization
{
    public class RythmSerializer : IRythmSerializer
    {
        public List<NoteData> Deserialize(string notesAsString)
        {
            List<string> waves = notesAsString.Split('\n').ToList();

            List<NoteData> notes = new List<NoteData>();

            foreach (string line in waves)
            {
                string[] parts = line.Split('-');
                if(parts.Length == 2)
                {
                    notes.Add(new NoteData(float.Parse(parts[0]), int.Parse(parts[1])));

                }
            }
            return notes;
        }

        public string Serialize(List<NoteData> notes)
        {
            string output = "";
            foreach (var item in notes)
            {
                output += $"{item.Beat}-{item.Count}\n";
            }
           return output.Remove(output.Length);
           
        }
    }
}
