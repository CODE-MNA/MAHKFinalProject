using MAHKFinalProject.DrawableComponents;
using static MAHKFinalProject.Helpers.SharedVars;
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
        Texture2D _strumBarTexture;

  
        

        bool freestyleMode = false;

        public Action onFreestyleMode;

        int nextZoneNote;
        float modeChangeTime;

        private float DEFAULT_MODE_TIME = 6;

        float hitXLine = 60;
        List<Droplet> dropBeatNotes = new List<Droplet>();

        List<TeleportZone> zones = new List<TeleportZone>();
        List<DropletLane> Lanes { get; set; }
        TeleportZone _zoneWithPlayer;
        TeleportPlayer _player;
       
        private int numberOfLanes = 3;

        int laneIndex = 0;
        int laneIncrement= 1;
        int counter = 0;
        private int numOffreeZones = 4;

        DropletLane GetNextLane()
        {
            counter++;  

            if(counter %  25 == 0)
            {
                laneIncrement = -laneIncrement;
            }
            
            laneIndex += laneIncrement;
            if(laneIndex >= numberOfLanes)
            {
                laneIndex = 0;
                
            }else if( laneIndex < 0)
            {
                laneIndex = numberOfLanes - 1;
              
            }
            return Lanes[laneIndex];
        }

        public FinalBattleLevel(Game game) : base(game, "WF_FinalBattle", 73)
        {
            //This Level uses seconds to sync notes instead of beats
            _levelConductor.Mode = MAHKFinalProject.GameComponents.Conductor.SyncMode.Seconds;
            _verticalLevel = false;
          
           _strumBarTexture = g.Content.Load<Texture2D>("dropletLane");

            modeChangeTime = DEFAULT_MODE_TIME;

            if (_loadedLevel.EventList != null)
            {

                if (_loadedLevel.EventList.Count > 0)
                {

                    //Use the time from file if given
                    modeChangeTime = _loadedLevel.EventList[0];
                }
            }
            InitializeLanes();
 

            //Only fill till the event guy says
            foreach (var item in _loadedLevel.NoteList)
            {
                if (item >= modeChangeTime) continue;

                DropletLane lane = GetNextLane();
                


                Droplet drop = new Droplet(g, item, lane.dropletSpawnPos, new Vector2(hitXLine, lane.dropletSpawnPos.Y), _levelConductor, this, lane);
                drop.DELAY_BETWEEN_SPAWN_AND_HIT = 0.9f;
                AssignTapHandlers(drop);
                
                dropBeatNotes.Add(drop);
            }

   

            GameComponents.AddRange(dropBeatNotes);
         
        }


        void InitializeLanes()
        {
            Vector2 nextPos = new Vector2(0, 0);

            Lanes = new List<DropletLane>();

            int laneHeight = (int)(STAGE.Y / numberOfLanes);

            for (int i = 0; i < numberOfLanes; i++)
            {

                DropletLane newLane = new DropletLane(g, nextPos, (int)STAGE.X, laneHeight, _strumBarTexture);

                newLane.dropletSpawnPos = new Vector2(newLane._width - 10, (laneHeight * i) + newLane._height / 2);

                Lanes.Add(newLane);
                

                nextPos = new Vector2(nextPos.X, nextPos.Y + laneHeight);
            }

            Lanes[0].TriggerKey = Keys.E;
            Lanes[1].TriggerKey = Keys.F;
            Lanes[2].TriggerKey = Keys.V;
            GameComponents.AddRange(Lanes);
        }


        protected override void ImplementNoteConstruction()
        {
         
        }

        void ChangeZones()
        {
            Random range = new Random();

            foreach(TeleportZone zone in zones)
            {
                if (zone.IsDangerous) zone.ZoneFlash();

                zone.IsDangerous = false;


            }            
            int maxHarmfulZoneCount = zones.Count - numOffreeZones;

            if (maxHarmfulZoneCount <= 1)
            {
                _zoneWithPlayer.IsDangerous = true;
                

            }
            else
            {
                for (int i = 0; i < maxHarmfulZoneCount; i++)
                {
                    int randIndex = range.Next(0, zones.Count);
                    zones[randIndex].IsDangerous = true;
                }

                _zoneWithPlayer.IsDangerous = true;
            }





            foreach (TeleportZone zone in zones)
            {
                float timeToNext = 1f;
                if (nextZoneNote + 1 < _loadedLevel.NoteList.Count)
                {
                    timeToNext = _loadedLevel.NoteList[nextZoneNote + 1] - 0.27f - (float)_levelConductor.GetSongSeconds();
                }

                zone.RefreshZoneAnimation(timeToNext);
            }

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
                g.SpriteBatch.Draw(_strumBarTexture, new Rectangle(160, 0, 80, (int)STAGE.Y), Color.Azure);
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

                int distFromCorner = 220;

               TeleportZone zone1 =  new TeleportZone(g, new Vector2(STAGE.X / 2, (STAGE.Y + distFromCorner) / 2), Keys.Space);
               TeleportZone zone2 =  new TeleportZone(g, new Vector2( distFromCorner , distFromCorner), Keys.R);
                TeleportZone zone3 = new TeleportZone(g, new Vector2((STAGE.X - distFromCorner), (STAGE.Y - distFromCorner)), Keys.N);
                TeleportZone zone4 = new TeleportZone(g, new Vector2((distFromCorner), (STAGE.Y - distFromCorner)), Keys.V);
                TeleportZone zone5 = new TeleportZone(g, new Vector2((STAGE.X -distFromCorner) ,(distFromCorner)), Keys.I);
               
                zones.Add(zone1);
                zones.Add(zone2);
                zones.Add(zone3);
                zones.Add(zone4);
                zones.Add(zone5);

                _player = new TeleportPlayer(g, zone1.Position);

                MovePlayer(zone1);


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
              
             


            


            base.Update(gameTime);


        }
    }
}
