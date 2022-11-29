using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Prototype.Components;
using Prototype.Helpers;
using Prototype.Rythm;
using Prototype.Serialization;

namespace Prototype.Managers
{
    public class SongStage : GameComponent
    {
        string _stageName;
        public string StageName
        {
            get { return _stageName; }
            set { _stageName = value; }
        }
        Song _song;
        float bpm = 69f;
        List<NoteData> _notes;
        ExplosionAnimation _noteInstance;
        Game g;
        float preNoteTime = 1f;
        Dictionary<float, NoteData> _notesDict;
        LevelFileHandler _fileHandler;
        public SongStage(Game game,Song song,string stageName, ExplosionAnimation noteInstance) :base(game)
        {
            g = game;
            _stageName = stageName;
            _song = song;
            _notesDict = new Dictionary<float, NoteData>();
            _noteInstance = noteInstance;
            _notes = new List<NoteData>();

            _fileHandler = new LevelFileHandler(new RythmSerializer());

            try
            {

                _notes = _fileHandler.LoadRythmFromFile(_stageName);
            }catch (Exception ex)
            {
                //throw new Exception("Couldn't Load File using Stage Name : " + ex.Message);
                
            }

            foreach (var item in _notes)
            {
                _notesDict.TryAdd(item.Beat, item);
            }
            
        }
        public override void Initialize()
        {
            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            if(_notesDict.TryGetValue(GetQuantizedBeat()  ,out NoteData newNote))
            {
                for (int i = 0; i < newNote.Count; i++)
                {
                    GenerateNote();
                    _notesDict.Remove(GetQuantizedBeat() );
                }
            }
            base.Update(gameTime);
        }

        internal float GetQuantizedBeat()
        {
            int shortBeat = (int)MathF.Round(GetCurrentBeat() * 4);
            return (float)shortBeat / 8f;
            
        }

        public void GenerateNote()
        {
            Random random = new Random();
            Vector2 pos = new Vector2(random.Next((int)SharedVars.STAGE.X - 200), random.Next((int)SharedVars.STAGE.Y - 200));

            Game1 ourGame = (Game1)g;
            ExplosionAnimation newComponent = ourGame.CreateNewExplosion(pos,0.01f);

            
            ourGame.Components.Add(newComponent);
        }

        public void PlayFromStart()
        {
            MediaPlayer.Play(_song);
            
        }

        public float GetCurrentBeat()
        {
            //If bpm is 120, at 60 seconds, curr beat should be 120
            return (float)GetSongSeconds() * (bpm / 60f);
        }
        public double GetSongSeconds()
        {
            return MediaPlayer.PlayPosition.TotalSeconds;
        }
    }
}
