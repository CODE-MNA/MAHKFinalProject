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
        private Texture2D _texture;
        private float _cellWidth;
        private float _cellHeight;
        private int _frameIndex = 0;
        private float _delaySeconds = 0.5f;
        private float _elapsedSeconds = 0;
        private List<Rectangle> _frames = new List<Rectangle>();
        public Action OnAnimationStopped;

         bool _isPlaying = false;
        public bool IsAnimationPlaying { get => _isPlaying; }
        bool _isLooping = false;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="texture">whole texture</param>
        /// <param name="cellWidth">each frame's width</param>
        /// <param name="cellHeight">each frame's height</param>
        /// <param name="delay">a period until change to next frame </param>
        public AnimationTexture(Game game, Texture2D texture, float cellWidth, float cellHeight, float delay, bool playOnSpawn = true, bool loop = false) : base(game)
        {
            Game1 g = (Game1) game;
            _texture = texture;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            _delaySeconds = delay;

            _isLooping = loop;
            _isPlaying = playOnSpawn;

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

        // get texture that will use
        public Texture2D GetAnimatedTexture()
        {
            return _texture;
        }

        // get Current frame
        public Rectangle GetCurrentFrame()
        {
            if (_frameIndex < 0) _frameIndex = 0;
            return _frames[_frameIndex];
        }

        public void StartPlaying()
        {
            _isPlaying = true;
            _frameIndex = 0;
        }

        public void StopPlaying(bool stopOnLastFrame = false)
        {
            _isPlaying = false;

            if (!stopOnLastFrame)
            {
            _frameIndex = 0;

            }
            else
            {
                _frameIndex = _frames.Count - 1;
            }

        }

        public void SetDelay(float seconds)
        {
          _delaySeconds = seconds;
            _elapsedSeconds = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_isPlaying) return;

            // change frames
            if (_frameIndex >= 0)
            {
                _elapsedSeconds += (float)gameTime.ElapsedGameTime.TotalSeconds;

                
                
                if (_elapsedSeconds > _delaySeconds / _frames.Count)
                {
                    _frameIndex++;
                    _elapsedSeconds = 0;
                    if (_frameIndex == _frames.Count)
                    {

                        if (_isLooping)
                        {
                            _frameIndex = 0;
                      

                        }
                        else
                        {
                            _frameIndex = 0;
                            _isPlaying = false;
                            OnAnimationStopped?.Invoke();
                        }
                    }
                }
            }

            base.Update(gameTime);
        }
    }
}
