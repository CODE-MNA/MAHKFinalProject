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
        Texture2D _texture;
        Vector2 _position;

        public Vector2 Position { get { return new Vector2(_position.X + _width/2,_position.Y + _height/2); } }

        int _height;
        int _width;

        public bool IsDangerous;
        public Keys ZoneKey { get; set; }


        public TeleportZone(Game game,Vector2 position, Keys K,int width, int height) : base(game)
        {
            g = (Game1)game;
            _texture = g.Content.Load<Texture2D>("Images/space");
            ZoneKey = K;
            _width = width;
            _height = height;

            _position = new Vector2(position.X - width/2, position.Y - height/2);
        }



        public override void Draw(GameTime gameTime)
        {

            
            g.SpriteBatch.Begin();

            g.SpriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, _width, _height), Color.AliceBlue * (IsDangerous ? 1 : 0.5f));

            g.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
