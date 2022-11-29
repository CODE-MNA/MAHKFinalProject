using MAHKFinalProject.LevelSerialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prototype.Serialization
{
    public class LevelFileHandler
    {
        IRythmSerializer _serializer;
        const string PATH = "Test/";

        public LevelFileHandler(IRythmSerializer serializer)
        {
            _serializer = serializer;
        }

        public void SaveRythmToFile(string fileName,BeatLevel rythm)
        {
            

            
                using (StreamWriter writer = new StreamWriter($"{PATH}{fileName}"))
                {
                  writer.WriteLine(_serializer.Serialize(rythm));
                }


        }

        public BeatLevel LoadRythmFromFile(string fileName)
        {
            string parsed;
            using (StreamReader reader = new StreamReader($"{PATH}{fileName}"))
            {
                parsed = reader.ReadToEnd();
            }
            if(parsed == null)
            {
                throw new Exception("File couldn't be read properly");
            }
            return _serializer.Deserialize(parsed);
        }
    }
}
