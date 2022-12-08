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
    public class TestLevelScene : GameScene
    {
        //Take it to abstract base Level
        public Queue<VisualizedNote> SpawnedDroplets { get; set; }


        List<DropletLane> Lanes;


        KeyboardState _oldKeyboardState;
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
        

        //Test
        SpriteFont _font;

        LevelFileHandler _levelFileHandler;
        ScoreManager scoreManager;
        
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

            _font = g.Content.Load<SpriteFont>("Fonts/regularFont");
            


            scoreManager = new ScoreManager();
            _levelConductor = new Conductor(g, _levelName, _song, bpm);

            this.GameComponents.Add(_levelConductor);

            SpawnedDroplets = new Queue<VisualizedNote>();

            //For each loaded beat level, read the floats and generate new points for it
         
            BeatLevel loaded = LoadBeatLevel();
            foreach (float dropTime in loaded.NoteList )
            {
                DropletLane laneForNewDrop = GetRandomLane();


                Vector2 spawnpoint = new Vector2(laneForNewDrop.dropletSpawnPos.X-10,laneForNewDrop.dropletSpawnPos.Y);
                
                Droplet drop = new Droplet(g, dropTime,new Vector2(spawnpoint.X,spawnpoint.Y), new Vector2(spawnpoint.X,spawnpoint.Y + SharedVars.STAGE.Y - 220) ,_levelConductor,this, laneForNewDrop);
                
                AssignEventHandlers(drop);
                

           
                this.GameComponents.Add(drop);
            }


        }
        public override void Initialize()
        {
            base.Initialize();

          
        }

        public override void Draw(GameTime gameTime)
        {

            g.SpriteBatch.Begin();
            g.SpriteBatch.DrawString(_font, scoreManager.CurentScore.ToString(), new Vector2(280, 30), Color.White);
            g.SpriteBatch.End();

            base.Draw(gameTime);
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

            Lanes[0].TriggerKey = Keys.F;
            Lanes[1].TriggerKey = Keys.G;
            Lanes[2].TriggerKey = Keys.H;
            Lanes[3].TriggerKey = Keys.J;
        }


        void InitializeLevelBeats()
        {

        }

        void AssignEventHandlers(VisualizedNote note)
        {
            note.OnTapped += () =>
            {
                scoreManager.CurentScore += (int)MathF.Floor(CalculateScore((Droplet)note));
            };
        }

        float CalculateScore(Droplet drop)
        {
            float finalscore = 200;

            finalscore -= (MathF.Abs((float)drop._targetPos.Y - (float)drop._position.Y));

            if(finalscore < 210 && finalscore > 195)
            {
                return finalscore * 3;
            }


            if(finalscore <= 0) finalscore = 0;

            return finalscore * 1.5f;
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

        DropletLane GetRandomLane()
        {
            Random rand = new Random();
             DropletLane lane = Lanes[rand.Next(0, LANE_AMOUNT)];

            return lane;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            KeyboardState ks = Keyboard.GetState();

            if (ms.LeftButton == ButtonState.Pressed && _oldState.LeftButton == ButtonState.Released)
            {

                _levelConductor.PlayFromStart();
            } 

            //if key pressed check if correct key pressed
            //if yes then remove drop
            //only do this for top drop


            if(SpawnedDroplets.TryPeek(out var droplet))
            {

                //Set as active droplet

                if (droplet.Status == RhythmComponents.NoteStatus.NotSpawned)
                {
                    return;
                }

                if (droplet._position.Y > Helpers.SharedVars.STAGE.Y - 1)
                {
                    SpawnedDroplets.Dequeue();
                    return;
                }


                if (ks.IsKeyDown(droplet.GetAssignedKey()) && _oldKeyboardState.IsKeyUp(droplet.GetAssignedKey())){

                            droplet.ActivateNote();
                            SpawnedDroplets.Dequeue();

                      

                    }

                



            }



            _oldKeyboardState = ks;
            _oldState = ms;

            base.Update(gameTime);
        }

    }
}
