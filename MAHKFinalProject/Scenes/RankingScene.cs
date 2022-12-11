﻿using MAHKFinalProject.GameComponents;
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
    public class RankingScene : GameScene
    {
        private SpriteBatch _spriteBatch;
        private Vector2 _position;
        private SpriteFont _headerFont;
        private SpriteFont _spriteFont;
        private Vector2 _velocity;
        private Texture2D _background;
        private Rectangle _backgroundRect;
        private ScoreFileManager _scoreFileManager;
        private List<int> scores;

        public RankingScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this._spriteBatch = g.SpriteBatch;
            this._position = new Vector2(SharedVars.STAGE.X / 3, SharedVars.STAGE.Y);
            this._headerFont = g.Content.Load<SpriteFont>("Fonts/hilightFont");
            this._spriteFont = g.Content.Load<SpriteFont>("Fonts/regularFont");
            this._background = g.Content.Load<Texture2D>("Images/space");
            this._velocity = new Vector2(0, 2.0f);
            this._backgroundRect = new Rectangle(0, 0, (int) SharedVars.STAGE.X, (int) SharedVars.STAGE.Y);

            // get scores
            _scoreFileManager = new ScoreFileManager();
            scores = _scoreFileManager.getHighScores(5);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 initPos = _position;
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // background image
            _spriteBatch.Draw(_background, _backgroundRect, Color.White);

            // text area
            _spriteBatch.DrawString(_headerFont, "Leader Board", initPos, Color.White);
            initPos.Y += _spriteFont.LineSpacing * 3;

            // set score - from the file
            foreach (var (item, index) in scores.Select((item, index) => (item, index)))
            {
                string msg = $"Rank {index+1} - {item} pts";
                _spriteBatch.DrawString(_spriteFont, msg, initPos, Color.White);
                initPos.Y += _spriteFont.LineSpacing * 2;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {

            // if it reaches top position, it will be stop
            if (_position.Y > SharedVars.STAGE.Y / 7)
            {
                // scrolling to the top
                _position = _position - _velocity;
            }

            base.Update(gameTime);
        }
    }
}
