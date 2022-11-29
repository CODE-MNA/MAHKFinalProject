using MAHKFinalProject.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.Scenes
{
    public class AboutScene : GameScene
    {
        private SpriteBatch _spriteBatch;
        private Vector2 _position;
        private SpriteFont _headerFont;
        private SpriteFont _spriteFont;

        public AboutScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this._spriteBatch = g.SpriteBatch;
            this._position = new Vector2(SharedVars.STAGE.X / 7, SharedVars.STAGE.Y / 7);
            this._headerFont = g.Content.Load<SpriteFont>("Fonts/hilightFont");
            this._spriteFont = g.Content.Load<SpriteFont>("Fonts/regularFont");
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 initPos = _position;
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_headerFont, "About Us", initPos, Color.White);
            initPos.Y += _spriteFont.LineSpacing * 3;

            _spriteBatch.DrawString(_spriteFont, "Genius == Muhammad Noman Ahmed", initPos, Color.White);
            initPos.Y += _spriteFont.LineSpacing * 2;

            _spriteBatch.DrawString(_spriteFont, "(^_______^)? Hyunchul Kim", initPos, Color.White);
            initPos.Y += _spriteFont.LineSpacing * 2;

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
