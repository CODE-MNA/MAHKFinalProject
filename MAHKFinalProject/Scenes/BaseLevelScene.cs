using MAHKFinalProject.DrawableComponents;
using MAHKFinalProject.GameComponents;
using MAHKFinalProject.LevelSerialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Prototype.Serialization;
using System;
using System.Collections.Generic;

namespace MAHKFinalProject.Scenes
{
    public class BaseLevelScene : GameScene
    {
        //Take it to abstract base Level
        protected Conductor _levelConductor;
        protected Game1 g;
        protected KeyboardState _oldKeyboardState;
        protected MouseState _oldState;
        protected string _levelName;
        protected int _bpm;
        protected Song _song;
        protected LevelFileHandler _levelFileHandler;
        protected ScoreManager scoreManager;
        protected SpriteFont _font;
        protected BeatLevel _loadedLevel;

        public Action OnLevelEnd;
        protected bool levelEnded = false;
        protected int framesToEndGame = 60* 2;
        public BaseLevelScene(Game game,string levelName, int injectedBpm) : base(game)
        {
            g = (Game1)game;
            _oldState = Mouse.GetState();
            _levelFileHandler = new LevelFileHandler(new RythmSerializer());
            _font = g.GlobalFont;


    
            _levelName = levelName;
            _bpm = injectedBpm ;
            _song = g.Content.Load<Song>("Songs/" + _levelName + "Song");




            scoreManager = new ScoreManager();
            _levelConductor = new Conductor(g, _levelName, _song, _bpm);

            this.GameComponents.Add(_levelConductor);

            SpawnedNotes = new Queue<VisualizedNote>();

            //For each loaded beat level, read the floats and generate new points for it

            _loadedLevel = LoadBeatLevel();

            
        }

        public Queue<VisualizedNote> SpawnedNotes { get; set; }


        public override void Update(GameTime gameTime)
        {
            if (levelEnded) return;

            if(_loadedLevel.NoteList.Count <= 0 && SpawnedNotes.Count <= 0)
            {
                framesToEndGame--;

                if (!_levelConductor.FadingTriggered)
                {
                    _levelConductor.FadeOutSong();
                }

                if(framesToEndGame < 0 && !levelEnded)
                {

                TriggerFinishGame();
                    return;
                }

                base.Update(gameTime);
                    return;
            }

            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

          
            if (ms.LeftButton == ButtonState.Pressed && _oldState.LeftButton == ButtonState.Released)
            {

                if (!_levelConductor.HasStarted)
                {
                    _levelConductor.FadeInSong();
                }
            }



            if (SpawnedNotes.TryPeek(out var frontNote))
            {



                if (frontNote.Status == RhythmComponents.NoteStatus.NotSpawned)
                {
                    return;
                }

                if (frontNote._position.Y > Helpers.SharedVars.STAGE.Y - 1)
                {
                    DeregisterRecentNote();
                    return;
                }


                if (ks.IsKeyDown(frontNote.GetAssignedKey()) && _oldKeyboardState.IsKeyUp(frontNote.GetAssignedKey()))
                {

                    frontNote.ActivateNote();

                    DeregisterRecentNote();


                }





            }



            _oldKeyboardState = ks;
            _oldState = ms;
            base.Update(gameTime);

        }

        protected void TriggerFinishGame()
        {
            levelEnded = true;
            OnLevelEnd?.Invoke();

        }

        void DeregisterRecentNote()
        {
            SpawnedNotes.Dequeue();
            if(_loadedLevel.NoteList.Count > 0)
            {
            _loadedLevel.NoteList.RemoveAt(0);

            }
        
        }

        protected BeatLevel LoadBeatLevel()
        {
            return _levelFileHandler.LoadRythmFromFile(_levelName + ".rdat");

        }
    }
}