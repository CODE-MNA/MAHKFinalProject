using MAHKFinalProject.DrawableComponents;
using MAHKFinalProject.GameComponents;
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
    internal class TestLevelScene : GameScene
    {

        List<Droplet> droplets;
        List<DropletLane> Lanes;
        MouseState _oldState;
        Conductor _levelConductor;
        
        Game1 g;
        Texture2D _dropTexture;
        Texture2D _laneTexture;

        Vector2 nextPos;
        float laneWidth;
        const int LANE_AMOUNT = 4;
        
        public TestLevelScene(Game game, Texture2D droptexture, Texture2D laneTexture) : base(game)
        {
            g = (Game1)game;
            _oldState = Mouse.GetState();


            _laneTexture = laneTexture;
            _dropTexture = droptexture;


            droplets = new List<Droplet>();

            InitializeLanes();
           
            //Todo get level info and make base map class with level name, loading level, bpm etc

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

            if(ms.RightButton == ButtonState.Pressed && _oldState.RightButton == ButtonState.Released)
            {
                Droplet drop = new Droplet(g, 6, GetRandomLaneSpawnpoint(),_dropTexture);
                this.GameComponents.Add(drop);
                
                drop.SpawnNote();

            } 


            _oldState = ms;

            base.Update(gameTime);
        }

    }
}
