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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.Scenes
{
    internal class TestLevelScene : GameScene
    {
        //Take it to abstract base Level
        List<Droplet> droplets;
        
        List<DropletLane> Lanes;



        MouseState _oldState;
        Conductor _levelConductor;
        
        Game1 g;
       
        Texture2D _laneTexture;

        Vector2 nextPos;
        float laneWidth;
        const int LANE_AMOUNT = 4;


        string _levelName;
        int bpm;
        Song _song;

        LevelFileHandler _levelFileHandler;
        
        public TestLevelScene(Game game) : base(game)
        {
            g = (Game1)game;
            _oldState = Mouse.GetState();


            _laneTexture = g.Content.Load<Texture2D>("dropletLane");


            InitializeLanes();

            //Todo get level info and make base map class with level name, loading level, bpm etc
            _levelName = "Astronaut13";
            bpm = 69;
            _song = g.Content.Load<Song>("Songs/" + _levelName + "Song");
            _levelFileHandler = new LevelFileHandler(new RythmSerializer());


            _levelConductor = new Conductor(g, _levelName, _song, bpm);

            this.GameComponents.Add(_levelConductor);

            droplets = new List<Droplet>();

            //For each loaded beat level, read the floats and generate new points for it
         
            BeatLevel loaded = LoadBeatLevel();
            foreach (float dropTime in loaded.NoteList )
            {
                Vector2 spawnpoint = GetRandomLaneSpawnpoint();
                Droplet drop = new Droplet(g, dropTime,spawnpoint, new Vector2(spawnpoint.X,spawnpoint.Y + SharedVars.STAGE.Y - 70) ,_levelConductor);

                this.GameComponents.Add(drop);
            }


        }
        public override void Initialize()
        {
            base.Initialize();

          
        }

        void InitializeLanes()
        {
            nextPos = new Vector2(0, 0);

            Lanes = new List<DropletLane>();

            laneWidth = SharedVars.STAGE.X / LANE_AMOUNT;

            for (int i = 0; i < LANE_AMOUNT; i++)
            {

                DropletLane newLane = new DropletLane(g, nextPos, (int)laneWidth, (int)SharedVars.STAGE.Y, _laneTexture);

                Lanes.Add(newLane);
                GameComponents.Add(newLane);

                nextPos = new Vector2(nextPos.X + laneWidth, nextPos.Y);
            }
        }


        void InitializeLevelBeats()
        {

        }

        BeatLevel TEMPBEATLEVEL()
        {
            return new BeatLevel()
            {
                NoteList = new List<float>()
                {
                    8, 9, 10, 11, 12, 16
                }
            };
        }

        BeatLevel LoadBeatLevel()
        {
           return _levelFileHandler.LoadRythmFromFile(_levelName + ".rdat");

        }

        protected override void LoadContent()
        {

            base.LoadContent();
        }

        Vector2 GetRandomLaneSpawnpoint()
        {
            Random rand = new Random();
             DropletLane lane = Lanes[rand.Next(0, LANE_AMOUNT)];

            return lane.dropletSpawnPos;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

            if(ms.LeftButton == ButtonState.Pressed && _oldState.LeftButton == ButtonState.Released)
            {

                _levelConductor.PlayFromStart();
            } 


            _oldState = ms;

            base.Update(gameTime);
        }

    }
}
