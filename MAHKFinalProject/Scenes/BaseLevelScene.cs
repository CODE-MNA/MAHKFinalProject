using MAHKFinalProject.DrawableComponents;
using MAHKFinalProject.GameComponents;
using MAHKFinalProject.Helpers;
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
    public abstract class BaseLevelScene : GameScene
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

        public Texture2D _bg;
        protected Rectangle _bgRect;
        bool bgLoaded = false;

        public Action OnLevelEnd;
        protected bool levelEnded = false;
        protected int framesToEndGame = 60* 2;
        protected readonly int _perfectTapScore = 800;

        protected bool _verticalLevel = true;

        public float effectDelayInBeats = 2;
        protected float _bgAlpha;
        protected bool flashing = false;
     
        protected float shakePower = 2.2f;
        protected float effectOffset;
        protected Color _bgColor;

        public BaseLevelScene(Game game,string levelName, int injectedBpm) : base(game)
        {
            g = (Game1)game;
            _oldState = Mouse.GetState();
            _levelFileHandler = new LevelFileHandler(new RythmSerializer());
            _font = g.GlobalFont;

         
            _levelName = levelName;
            _bpm = injectedBpm ;
            _song = g.Content.Load<Song>("Songs/" + _levelName + "Song");

            _bgColor = Color.AntiqueWhite;

            scoreManager = new ScoreManager();
            _levelConductor = new Conductor(g, _levelName, _song, _bpm);

            this.GameComponents.Add(_levelConductor);

            SpawnedNotes = new Queue<VisualizedNote>();

            //For each loaded beat level, read the floats and generate new points for it

            _loadedLevel = LoadBeatLevel();
            OnLevelEnd += () =>
            {
                _bgColor = Color.Gray;
                _bgAlpha = 0.08f;
            };

            try
            {

            _bg = g.Content.Load<Texture2D>("Backgrounds/" + _levelName + "BG");
                bgLoaded = true;
             }catch (Exception ex)
            {
            
            }
            _bgRect = new Rectangle(0, 0, (int)SharedVars.STAGE.X, (int)SharedVars.STAGE.Y); 
        }

        public Queue<VisualizedNote> SpawnedNotes { get; set; }

        public void RecordScores()
        {
            // record score
            ScoreFileManager scoreFileManager = new ScoreFileManager(_levelName);
            scoreFileManager.recordScore(scoreManager.CurrentScore);
        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin(SpriteSortMode.BackToFront);
            if (levelEnded)
            {
                string overMessage = "Game End, Your Score was : " + scoreManager.CurrentScore;
                g.SpriteBatch.DrawString(_font, overMessage, new Vector2((SharedVars.STAGE.X / 2) - _font.MeasureString(overMessage).X, (SharedVars.STAGE.Y / 2) - _font.MeasureString(overMessage).Y), Color.White);
            }
            if (bgLoaded) g.SpriteBatch.Draw(_bg, Vector2.Zero,_bgRect, _bgColor * _bgAlpha,0f,Vector2.Zero,1f,SpriteEffects.None,layerDepth:1f);
            g.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (levelEnded) return;

            if (!flashing && _levelConductor.OnBeat(effectDelayInBeats,effectOffset))
            {
             
                _bgRect.Inflate(shakePower, -shakePower);

                
                flashing = true;
            }

            if (flashing)
            {
              
                 
                if (_bgAlpha <= 0.55)
                {
                    _bgAlpha += 0.1f / effectDelayInBeats;

                    
                    
                }
                else
                {
                    _bgRect.Inflate(-shakePower, shakePower);


                    //max
                    _bgAlpha = 0.55f;
                    flashing = false;
                    
                  
                }
                
            }
            else
            {
                if (_bgAlpha >= 0.1)
                {
                    //decreasing
                    _bgAlpha -= 0.05f / effectDelayInBeats;

                }
                else
                {
                    //min 
                    _bgAlpha = 0.1f;

                }
            }



            if (_loadedLevel.NoteList.Count <= 0 && SpawnedNotes.Count <= 0)
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




                if (_verticalLevel)
                {
                    if (frontNote._position.Y > SharedVars.STAGE.Y - 80)
                    {
                        SpawnedNotes.Dequeue();

                        DeregisterRecentNote();
                        base.Update(gameTime);
                        return;
                    }


                }
                else
                {
                    if ( frontNote._position.X < 89)
                    {
                        SpawnedNotes.Dequeue();

                        DeregisterRecentNote();
                        base.Update(gameTime);
                        return;
                    }
                }


                if (ks.IsKeyDown(frontNote.GetAssignedKey()) && _oldKeyboardState.IsKeyUp(frontNote.GetAssignedKey()))
                {

                    frontNote.ActivateNote();
                    SpawnedNotes.Dequeue();

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
            RecordScores();
            OnLevelEnd?.Invoke();

        }

        protected void DeregisterRecentNote()
        {
            if(_loadedLevel.NoteList.Count > 0)
            {
            _loadedLevel.NoteList.RemoveAt(0);

            }
        
        }

        protected BeatLevel LoadBeatLevel()
        {
            return _levelFileHandler.LoadRythmFromFile(_levelName + ".rdat");

        }


        public virtual void AssignTapHandlers(VisualizedNote note)
        {
            note.OnTapped += () =>
            {
                int tapScore = (int)note.CalculateScore();
                if (tapScore >= _perfectTapScore)
                {
                    tapScore += tapScore + (200 * scoreManager.CurrentCombo);
                    scoreManager.CurrentCombo = scoreManager.CurrentCombo + 1;
                }
                else
                {
                    scoreManager.CurrentCombo = 0;
                }

                scoreManager.CurrentScore += (int)MathF.Floor(tapScore);

                Droplet drop = (Droplet)note;
                if (tapScore >= _perfectTapScore)
                {
                    drop._lane.FlashLane(true);

                }
                else
                {
                    drop._lane.FlashLane(false);
                }

            };
        }
        protected abstract void ImplementNoteConstruction();
    }
}