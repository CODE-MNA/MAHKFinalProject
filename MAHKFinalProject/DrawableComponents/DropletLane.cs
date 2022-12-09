using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.DrawableComponents
{
    public class DropletLane : DrawableGameComponent
    {
        public Vector2 dropletSpawnPos;

        public Vector2 ImpactPos;

        public Vector2 _spawnCorner;
        public int _width;
        public int _height;

        public Rectangle _laneRect;
        Texture2D _texture;

        public Queue<Droplet> _droplets;

        //Temp
        KeyboardState _oldKbState;



        //Possible Common Prroperties
        private Game1 g;
        public Keys TriggerKey { get; set; }
        
        public DropletLane(Game game,Vector2 spawnCorner,int width,int height, Texture2D texture) : base(game)
        {
            _droplets = new Queue<Droplet>();

            g = (Game1)game;
            _spawnCorner = spawnCorner;
            _width = width;
            _height = height;

            _texture = texture;
            _laneRect = new Rectangle((int)spawnCorner.X, (int)spawnCorner.Y, width, height);

            dropletSpawnPos = new Vector2(spawnCorner.X + width/2, spawnCorner.Y + 10);
            ImpactPos = new Vector2(dropletSpawnPos.X , dropletSpawnPos.Y + height - 20);

        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();
            //g.SpriteBatch.Draw(_texture, _laneRect, Color.AliceBlue);
            g.SpriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

        

            //if (ks.IsKeyDown(TriggerKey) && _oldKbState.IsKeyUp(TriggerKey) && _droplets.Count > 0)
            //{

                
            //    //Transition to active

            //    //Calculate score
            //    Droplet drop = _droplets.Dequeue();
            //    //TEMP 
            //    drop.Enabled = false;
            //    drop.Visible = false;

            //}

            //_oldKbState = ks;

            base.Update(gameTime);
        }
    }
}
