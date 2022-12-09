/*
 * Program ID: AnimationTexture
 *  
 * Purpose: to animate texture
 *             - frame size = cellWidth, cellHeight
 *          
 * Revision History:
 *      Hyunchul Kim, 2022.12.08: Created
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.DrawableComponents
{
    public class AnimationTexture : DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private float _cellWidth;
        private float _cellHeight;
        private int _frameIndex = 0;
        private int _delay = 10;
        private int _delayCount = 0;
        Vector2 _position;
        private List<Rectangle> _frames = new List<Rectangle>();

        public AnimationTexture(Game game, Texture2D texture, Vector2 position, float cellWidth, float cellHeight, int delay) : base(game)
        {
            Game1 g = (Game1) game;
            _spriteBatch = g.SpriteBatch;
            _texture = texture;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            _position = position;
            _delay = delay;

            int rows = texture.Height / (int)_cellHeight;
            int cols = texture.Width / (int) _cellWidth;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Rectangle rect = new Rectangle(j * (int)_cellWidth, i * (int)_cellHeight, (int)_cellWidth, (int)_cellHeight);
                    _frames.Add(rect);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            if (_frameIndex >= 0)
            {
                _spriteBatch.Draw(_texture, _position, _frames[_frameIndex], Color.White);
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // change frames
            if (_frameIndex >= 0)
            {
                _delayCount++;
                if (_delayCount > _delay)
                {
                    _frameIndex++;
                    _delayCount = 0;
                    if (_frameIndex == _frames.Count)
                    {
                        _frameIndex = -1;
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
