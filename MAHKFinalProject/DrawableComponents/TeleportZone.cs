using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAHKFinalProject.Helpers;
using Microsoft.Xna.Framework.Input;

namespace MAHKFinalProject.DrawableComponents
{
    public class TeleportZone : DrawableGameComponent
    {
        Game1 g;
        Texture2D _fullAnimationTexture;
       
        AnimationTexture _anim;
        Vector2 _position;

        public Vector2 Position { get { return new Vector2(_position.X + _WIDTH / 2, _position.Y + _HEIGHT / 2); } }

        const int _HEIGHT = 256;
        const int _WIDTH = 256;

        public bool IsDangerous = false;
        private bool alreadyPlayed;
        private bool flashing;
        private Texture2D _flashTexture;
        private float alpha;
        private float _maxAlpha = 1f;
        private float _minAlpha = 0.3f;

        public Keys ZoneKey { get; set; }

        

        public TeleportZone(Game game, Vector2 position, Keys K) : base(game)
        {
            g = (Game1)game;
            _fullAnimationTexture = g.Content.Load<Texture2D>("Animation/Zone");
            _flashTexture = g.Content.Load<Texture2D>("ZoneFlash");
            ZoneKey = K;


            _position = new Vector2(position.X - _WIDTH / 2, position.Y - _HEIGHT / 2);

            _anim = new AnimationTexture(g, _fullAnimationTexture, _WIDTH, _HEIGHT, 1, false, false);
            _anim.StopPlaying();
            _anim.OnAnimationStopped += () =>
            {
                if (alreadyPlayed)
                {
                    _anim.StopPlaying(true);
                }
            };
        }


        public void RefreshZoneAnimation(float newDelay)
            {
                 alreadyPlayed = false;

            if (newDelay < 0) newDelay = 0.006f;
                 _anim.SetDelay(newDelay);
            }

        public void ZoneFlash()
        {
            flashing = true;
            alpha = _minAlpha;
        }

        public override void Draw(GameTime gameTime)
        {
         
            g.SpriteBatch.Begin();

            Rectangle rect = _anim.GetCurrentFrame();
                g.SpriteBatch.Draw(_flashTexture, Position, rect, Color.AliceBlue * alpha,0f,new Vector2(rect.Location.X + rect.Width/2, rect.Location.Y + rect.Height/2),1.15f,SpriteEffects.None,0);
            
            
               g.SpriteBatch.Draw(_anim.GetAnimatedTexture(),_position,rect, Color.AliceBlue );

            g.SpriteBatch.DrawString(g.GlobalFont, ZoneKey.ToString(), new Vector2(Position.X, Position.Y - _HEIGHT/1.9f), Color.LightGoldenrodYellow);


            g.SpriteBatch.End();

            base.Draw(gameTime);
        }


        public override void Update(GameTime gameTime)
        {

            _anim.Update(gameTime);

            if (flashing)
            {
                if (alpha <= _maxAlpha)
                {
                    alpha += 0.1f;
                }
                else
                {
                    alpha = _maxAlpha;
                    flashing = false;
                    _anim.StopPlaying(false);

                }
                base.Update(gameTime);
                return;
            }
            else
            {
                if (alpha > _minAlpha)
                {
                    alpha -= 0.09f;
                    base.Update(gameTime);

                    return;

                }
                else
                {
                    alpha = 0;
                }
            }

            if (IsDangerous)
            {
                if (alreadyPlayed == false)
                {

                _anim.StartPlaying();
                    alreadyPlayed = true;
                }
               


            }
            else
            {
                alreadyPlayed = false;
                _anim.StopPlaying(false);
            }

          
            base.Update(gameTime);
        }
    }
}
