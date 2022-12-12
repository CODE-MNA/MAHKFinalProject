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


            List<string> parts = notesAsString.Split("EVENTS").ToList();

            List<string> waves = parts[0].Split('\n').ToList();

            List<string> eventStrings = null;

            if(parts.Count > 1)
            {
             eventStrings =   parts[1].Split('\n').ToList();
            }
           
          
            List<float> timings = new List<float>();
          
            List<float> events = new List<float>();
            
            foreach (string line in waves)
            {
                //string[] parts = line.Split('-');
                //if(parts.Length == 2)
                //{
                //    //notes.Add(new Note(float.Parse(parts[0]), int.Parse(parts[1])));

                //}
                try
                {
                    timings.Add(float.Parse(line));

                }
                catch (FormatException)
                {
                    continue;
                }

            }

            if(parts.Count > 1 && eventStrings != null)
            {
                foreach (var line in eventStrings)
                {

                    try
                    {

                        events.Add(float.Parse(line));
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }

            }

            return new BeatLevel()
            {
                NoteList = timings,
                EventList = events
            };
        }

        public string Serialize(BeatLevel notes)
        {
           

            string output = "";
            foreach (var item in notes.NoteList)
            {
                output += $"{item}\n";
            }
           return output.Remove(output.Length);
           
        }
    }
}
