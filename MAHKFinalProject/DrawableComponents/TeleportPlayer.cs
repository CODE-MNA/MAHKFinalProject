using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.DrawableComponents
{
    public class TeleportPlayer : DrawableGameComponent
    {
        Game1 g;
        Vector2 _position;
        AnimationTexture _anim;

        Texture _texture;

        public int Width
        {
            get
            {
                return _anim.GetCurrentFrame().Width;
            }
        }
        public int Height
        {
            get { return _anim.GetCurrentFrame().Height; }
        }

        public TeleportPlayer(Game game, Vector2 position) : base(game)
        {
            g = (Game1)game;    
            _position = position;

            Texture2D spriteSheet = g.Content.Load<Texture2D>("DestroyedSquare");
            _anim = new AnimationTexture(game, spriteSheet, 64, 64, 1, false);
        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();
            g.SpriteBatch.Draw(_anim.GetAnimatedTexture(), _position, _anim.GetCurrentFrame(), Color.AntiqueWhite);

            //g.SpriteBatch.DrawString(g.GlobalFont,"cnsdiuvs",_position,Color.Aquamarine);
            g.SpriteBatch.End();
            base.Draw(gameTime);    
        }

        public override void Update(GameTime gameTime)
        {
            _anim.Update(gameTime);
            base.Update(gameTime);
        }
        public void SetPosition(Vector2 pos)
        {
            _anim.StartPlaying();
            this._position = pos;
        }
    }
}
