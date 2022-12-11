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
using MAHKFinalProject.Helpers;
namespace MAHKFinalProject.Scenes
{
    public class TestLevelScene : BaseLevelScene
    {
        List<DropletLane> Lanes;
        Texture2D _laneTexture;
        float laneWidth;
        const int LANE_AMOUNT = 4;
        float hitYLine;
        //Test
        string dashes;

        //Decoration
        List<Rectangle> pulses;
  
        public TestLevelScene(Game game) : base(game,"WF_Endgame",123)
        {
            
            _laneTexture = g.Content.Load<Texture2D>("dropletLane");
            InitializeLanes();
            pulses = new List<Rectangle>();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Rectangle rect = new Rectangle((int)(i * SharedVars.STAGE.X / 10), (int)(j * SharedVars.STAGE.Y / 10), (int)(SharedVars.STAGE.X / 10), (int)(SharedVars.STAGE.Y / 10));
                    pulses.Add(rect);   

                }

            }







          

            //Droplet Code
            foreach (float dropTime in _loadedLevel.NoteList )
            {
                DropletLane laneForNewDrop = GetRandomLane();


                Vector2 spawnpoint = new Vector2(laneForNewDrop.dropletSpawnPos.X-10,laneForNewDrop.dropletSpawnPos.Y);

                hitYLine = spawnpoint.Y + SharedVars.STAGE.Y - 160;
                Droplet drop = new Droplet(g, dropTime,new Vector2(spawnpoint.X,spawnpoint.Y), new Vector2(spawnpoint.X,hitYLine) ,_levelConductor,this, laneForNewDrop);
                
                AssignEventHandlers(drop);
                

           
                this.GameComponents.Add(drop);
            }
            int dashNumber = (int)SharedVars.STAGE.X / (int)_font.MeasureString("----").X;


            for (int i = 0; i <= dashNumber; i++)
            {
                dashes += "----";
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
            g.SpriteBatch.DrawString(_font, dashes  ,new Vector2(0,hitYLine),Color.Red);
            g.SpriteBatch.DrawString(_font, dashes, new Vector2(0,hitYLine - 40),Color.Blue);
            g.SpriteBatch.End();

            base.Draw(gameTime);
        }

        void InitializeLanes()
        {
           Vector2 nextPos = new Vector2(0, 0);

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


       
        void AssignEventHandlers(VisualizedNote note)
        {
            int perfectHitPoints = 800;
            note.OnTapped += () =>
            {
                float tapScore = note.CalculateScore();


                if(tapScore >= perfectHitPoints)
                {
                    tapScore += tapScore + (25 * scoreManager.CurrentCombo);
                    scoreManager.CurrentCombo = scoreManager.CurrentCombo + 1;
                }
                else
                {
                    scoreManager.CurrentCombo = 0;
                }

                scoreManager.CurentScore += (int)MathF.Floor(tapScore);

                Droplet drop  = (Droplet)note;


                if(tapScore >= perfectHitPoints)
                {
                    drop._lane.FlashLane(true);

                }
                else
                {
                    drop._lane.FlashLane(false);
                }
                
            };
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
          

            base.Update(gameTime);
        }

    }
}
