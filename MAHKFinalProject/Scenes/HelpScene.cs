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
    public class HelpScene : GameScene
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private Vector2 _position;
        Game1 g;
        public HelpScene(Game game) : base(game)
        {
             g = (Game1)game;
            this._spriteBatch = g.SpriteBatch;
            this._texture = g.Content.Load<Texture2D>("Images/help");
            this._position = new Vector2(0, 0);

        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                g.authHandler.Logout();
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            // spriteBatch.Draw(texture, Vecotr2.Zero, Color.White);
            _spriteBatch.Draw(_texture, _position, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}
