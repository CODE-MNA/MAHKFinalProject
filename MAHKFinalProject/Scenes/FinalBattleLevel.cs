using MAHKFinalProject.DrawableComponents;
using MAHKFinalProject.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.Scenes
{
    public class FinalBattleLevel : BaseLevelScene
    {
        Texture2D _temp;

        bool drawSprite;

        float stay;

        bool freestyleMode = false;

        public Action onFreestyleMode;

        int nextZoneNote;
        float modeChangeTime;

        private float DEFAULT_MODE_TIME = 20;

        float hitXLine = 160;
        List<Droplet> dropBeatNotes = new List<Droplet>();

        List<TeleportZone> zones = new List<TeleportZone>();
        TeleportZone _zoneWithPlayer;
        TeleportPlayer _player;
        DropletLane _tempLane;
        public FinalBattleLevel(Game game) : base(game, "WF_FinalBattle", 73)
        {
            //This Level uses seconds to sync notes instead of beats
            _levelConductor.Mode = MAHKFinalProject.GameComponents.Conductor.SyncMode.Seconds;

            stay = 0;
           _temp = g.Content.Load<Texture2D>("dropletLane");

            modeChangeTime = DEFAULT_MODE_TIME;

            if (_loadedLevel.EventList != null)
            {

                if (_loadedLevel.EventList.Count > 0)
                {

                    //Use the time from file if given
                    modeChangeTime = _loadedLevel.EventList[0];
                }
            }

            _tempLane = new DropletLane(g, Vector2.One, 1, 1, _temp);
            _tempLane.TriggerKey = Keys.K;

            //Only fill till the event guy says
            foreach (var item in _loadedLevel.NoteList)
            {
                if (item >= modeChangeTime) continue;
                Droplet drop = new Droplet(g, item, SharedVars.STAGE / 2, new Vector2(hitXLine, 140), _levelConductor, this, _tempLane);
                dropBeatNotes.Add(drop);
            }

            GameComponents.AddRange(dropBeatNotes);
         
        }

        protected override void ImplementNoteConstruction()
        {
         
        }

        void ChangeZones()
        {
            Random range = new Random();

            foreach(TeleportZone zone in zones) zone.IsDangerous = false;
            
            int maxHarmfulZoneCount = zones.Count - 2;

            if (maxHarmfulZoneCount <= 1)
            {
                _zoneWithPlayer.IsDangerous = true;
                return;
            }



            for (int i = 0; i < maxHarmfulZoneCount; i++)
            {
                int randIndex = range.Next(0, zones.Count);
                zones[randIndex].IsDangerous = true;
            }
            _zoneWithPlayer.IsDangerous = true;

        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();
            g.SpriteBatch.DrawString(_font,scoreManager.CurrentScore.ToString(),new Vector2(600, 30),Color.Bisque);

            if (freestyleMode)
            {

            }
            else
            {
                g.SpriteBatch.Draw(_temp, new Rectangle(160, 0, 80, (int)SharedVars.STAGE.Y), Color.Azure);
            }

            g.SpriteBatch.End();

            base.Draw(gameTime);
        }

        void MovePlayer(TeleportZone toZone)
        {
            if (_zoneWithPlayer == toZone) return;

            if(scoreManager.CurrentCombo < 4 && scoreManager.CurrentScore >=100)
            {
                scoreManager.CurrentScore -= 100;
                if(scoreManager.CurrentScore < 0)
                {
                    scoreManager.CurrentScore = 0;
                }

            }

            _player.SetPosition(new Vector2(toZone.Position.X - _player.Width/2 , toZone.Position.Y - _player.Height /2));

            _zoneWithPlayer = toZone;


        }


        public override void Update(GameTime gameTime)
        {
            if (levelEnded || _loadedLevel.NoteList.Count <= 0 || g.Paused)
            {
                base.Update(gameTime);
                return;
            }



            KeyboardState ks = Keyboard.GetState();
            //ModeChage
            if (!freestyleMode &&_levelConductor.GetSongSeconds() >= modeChangeTime)
            {
                freestyleMode = true;
                onFreestyleMode?.Invoke();


               TeleportZone zone1 =  new TeleportZone(g, new Vector2(Helpers.SharedVars.STAGE.X / 2, Helpers.SharedVars.STAGE.Y / 2), Keys.D,150,150);
               TeleportZone zone2 =  new TeleportZone(g, new Vector2((Helpers.SharedVars.STAGE.X + 3 )/ 6 , (Helpers.SharedVars.STAGE.Y - 33) / 7), Keys.A,150,150);
                
                zone1.IsDangerous = true;
                zone2.IsDangerous = true;

                _player = new TeleportPlayer(g, zone1.Position);

                MovePlayer(zone1);

                zones.Add(zone1);
                zones.Add(zone2);

                GameComponents.AddRange(zones);
                GameComponents.Add(_player);
            
            }



            if (freestyleMode)
            {

                foreach (var zone in zones)
                {
                    if(ks.IsKeyDown(zone.ZoneKey) && _oldKeyboardState.IsKeyUp(zone.ZoneKey))
                    {
                        MovePlayer(zone);
                    }

                }



                //Second Mode gameplay
                if (MathF.Abs(_loadedLevel.NoteList[nextZoneNote] - (float)_levelConductor.GetSongSeconds()) < 0.036f)
                {

                    if(_zoneWithPlayer.IsDangerous == false)
                    {
                        scoreManager.CurrentScore += 800 + (scoreManager.CurrentCombo * 100);
                        scoreManager.CurrentCombo++;
                    }
                    else
                    {
                        scoreManager.CurrentCombo = 0;
                    }
                    ChangeZones();
                    _loadedLevel.NoteList.RemoveAt(0);
                }



                base.Update(gameTime);
                return;

            }

            //Standard Mode
         

            
                    if (MathF.Abs(_loadedLevel.NoteList[0] - (float)_levelConductor.GetSongSeconds()) < 0.036f)
                    {
                      
                        _loadedLevel.NoteList.RemoveAt(0);
                    }
              
             


            

            if (stay > 1)
            {
                stay = 0;
                drawSprite = false;
            }

            base.Update(gameTime);


        }
    }
}
