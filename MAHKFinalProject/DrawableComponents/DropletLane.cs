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
        Texture2D _flashTexture;
        Texture2D _failTexture;

        public Queue<Droplet> _droplets;

        //Temp
        KeyboardState _oldKbState;

        public bool flashing;

        //Possible Common Prroperties
        private Game1 g;
        private float alpha = 0f;
        private float _maxAlpha = 0.6f;
      
        private float fontAlpha;

        public Keys TriggerKey { get; set; }
        
        public DropletLane(Game game,Vector2 spawnCorner,int width,int height, Texture2D texture) : base(game)
        {
            _droplets = new Queue<Droplet>();

            g = (Game1)game;
            _flashTexture = g.Content.Load<Texture2D>("greenFlash");
            _failTexture = g.Content.Load<Texture2D>("yellowFlash");
            _spawnCorner = spawnCorner;
            _width = width;
            _height = height;


            _texture = texture;
            _laneRect = new Rectangle((int)spawnCorner.X, (int)spawnCorner.Y, width, height);

            dropletSpawnPos = new Vector2(spawnCorner.X + width/2, spawnCorner.Y + 10);
            ImpactPos = new Vector2(dropletSpawnPos.X , dropletSpawnPos.Y + height - 20);

            alpha = 0f;


        }

        public override void Draw(GameTime gameTime)
        {
            
            g.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Additive);
            
            g.SpriteBatch.Draw(_texture, _laneRect,new Color(Color.AliceBlue,alpha));

        
                g.SpriteBatch.DrawString(g.GlobalFont, this.TriggerKey.ToString(),_laneRect.Center.ToVector2(), Color.LightGoldenrodYellow * fontAlpha);
           

            g.SpriteBatch.End();
            base.Draw(gameTime);
        }



        public override void Update(GameTime gameTime)
        {
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.Tab))
            {
                if (fontAlpha <= 1)
                {
                    fontAlpha += 0.125f;
                }
                else
                {
                    fontAlpha = 1;
                  
                }
            }
            else
            {
                if (fontAlpha > 0)
                {
                    fontAlpha -= 0.15f;

                }
                else
                {
                    fontAlpha = 0;
                }
            }


            if (flashing)
            {
               if(alpha <= _maxAlpha)
                {
                    alpha += 0.125f;
                }
                else
                {
                    alpha = 0.6f;
                    flashing = false;
                }
            }
            else
            {
                if(alpha > 0)
                {
                    alpha -= 0.08f;

                }
                else
                {
                    alpha = 0;
                }
            }
          
           


            _oldKbState = ks;
            base.Update(gameTime);
        }

        //Change to enum
        public void FlashLane(bool success)
        {
            if (success)
            {
                _maxAlpha = 0.85f;
                _texture = _flashTexture;

            }
            else 
            {
                _maxAlpha = 0.6f;
                _texture = _failTexture;
            }
            flashing = true;
        }
    }
}
