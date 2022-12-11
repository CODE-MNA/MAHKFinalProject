using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototype.Components
{
    public class ExplosionAnimation : DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        Texture2D _texture;
        Vector2 _position;
        Vector2 _oneImageDimension;
        List<Rectangle> frames;
        public Vector2 Position { get=>_position; set=>_position = value; }
        float originalDelay;
        float delay;
        int frameIndex = -1;
        const int ROWS = 5;
        const int COLS = 5;
        public ExplosionAnimation(Game game, SpriteBatch spriteBatch, Texture2D texture, Vector2 position, float framesForDelay) : base(game)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
            _oneImageDimension = new Vector2(_texture.Width / COLS, _texture.Height / ROWS);
            _position = new Vector2(position.X - _oneImageDimension.X / 2, position.Y - _oneImageDimension.Y / 2);
            originalDelay = framesForDelay;
            delay = 0;
            frames = new List<Rectangle>();
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLS; j++)
                {
                    Rectangle currentRect = new Rectangle((int)(j * _oneImageDimension.X), (int)(i * _oneImageDimension.Y), (int)_oneImageDimension.X, (int)_oneImageDimension.Y);
                    frames.Add(currentRect);
                }
            }


        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if (frameIndex >= 0 && frameIndex < frames.Count)
            {
                _spriteBatch.Draw(_texture, _position, frames[frameIndex], Color.AntiqueWhite);

            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (delay >= originalDelay)
            {
                delay = 0;
                frameIndex++;
            }
            else
            {
                delay++;
            }


            if (frameIndex >= frames.Count)
            {
                this.Enabled = false;
              this.Visible = false;
            }

            base.Update(gameTime);
        }
    }
}
