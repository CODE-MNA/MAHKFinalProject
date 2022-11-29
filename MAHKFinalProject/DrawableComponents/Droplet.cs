using MAHKFinalProject.RhythmComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.DrawableComponents
{


    public class Droplet : VisualizedNote
    {
        Vector2 _velocity;
        Vector2 _scale;

        public override void Initialize()
        {

            base.Initialize();
        }
        protected override void LoadContent()
        {

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();
            g.SpriteBatch.Draw(_texture,
                new Rectangle((int)_position.X,(int)_position.Y,_texture.Width, _texture.Height),Color.AliceBlue);
            g.SpriteBatch.End();

            base.Draw(gameTime);    
        }
        public Droplet(Game game, float hitBeat, Vector2 position, Texture2D texture) : base(game, hitBeat ,position ,texture)
        {
            g = (Game1)game;
            
            
        }

        public override void UpdateActive(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEnded(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void UpdateNotSpawned(GameTime gameTime)
        {
            //If Conductor is on the beat with the hitbeat, change state to spawn
        }

        public override void UpdateSpawned(GameTime gameTime)
        {

            //Instead of using update, use conductors update time
            _position = new Vector2(_position.X,_position.Y + 10);


        }
    }
}
