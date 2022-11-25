using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Prototype.Managers;
using Prototype.Rythm;
using Prototype.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Helpers
{
    public class NoteMaker : GameComponent
    {
        List<Note> _notes;
        SongStage _songStage;
        KeyboardState _oldState;

        LevelFileHandler _levelFileHandler;

        public NoteMaker(Game game,SongStage ss) : base(game)
        {
            _songStage = ss;
            _notes = new List<Note>();

            _levelFileHandler = new LevelFileHandler(new RythmSerializer());
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if(ks.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space)){
                AddRythmToList();
            }

            if(ks.IsKeyDown(Keys.A) && _oldState.IsKeyUp(Keys.A))
            {
                SaveRythms();
            }
            _oldState = ks;
            base.Update(gameTime);
        }

        private void AddRythmToList()
        {
          

            float beat = _songStage.GetQuantizedBeat();

            _notes.Add(new Note()
            {
                Count = 1,
                Beat = beat,
            });
        }


        public void SaveRythms()
        {
       
            _levelFileHandler.SaveRythmToFile(_songStage.StageName,_notes);
            Console.WriteLine("Saved File FOR : " + _songStage.StageName);
            _notes.Clear();
        }
    }
}
