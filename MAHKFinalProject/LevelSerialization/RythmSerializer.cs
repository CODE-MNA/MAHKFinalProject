using MAHKFinalProject.LevelSerialization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Serialization
{
    public class RythmSerializer : IRythmSerializer
    {

        public BeatLevel Deserialize(string notesAsString)
        {
            throw new NotImplementedException();    


            List<string> waves = notesAsString.Split('\n').ToList();

          

            foreach (string line in waves)
            {
                string[] parts = line.Split('-');
                if(parts.Length == 2)
                {
                    //notes.Add(new Note(float.Parse(parts[0]), int.Parse(parts[1])));

                }
            }
            return new BeatLevel();
        }

        public string Serialize(BeatLevel notes)
        {
            throw new NotImplementedException();

            string output = "";
            foreach (var item in notes.NoteList)
            {
                output += $"{item}\n";
            }
           return output.Remove(output.Length);
           
        }
    }
}
