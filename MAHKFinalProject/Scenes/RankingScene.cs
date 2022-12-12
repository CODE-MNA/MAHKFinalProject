using MAHKFinalProject.GameComponents;
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
        private ScoreFileManager _level2ScoreFileManager;
        private List<int> level1Scores;
        private List<int> level2Scores;

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
            _scoreFileManager = new ScoreFileManager("WF_Endgame");
            _level2ScoreFileManager = new ScoreFileManager("WF_FinalBattle");
            level1Scores = _scoreFileManager.getHighScores(5);
            level2Scores = _level2ScoreFileManager.getHighScores(5);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 initPos = _position;
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            // background image
            _spriteBatch.Draw(_background, _backgroundRect, Color.White);

          

            if(level1Scores != null && level1Scores.Count > 0)
            {
                // set score - from the file
                
                
                _spriteBatch.DrawString(_headerFont, "Ranks For Level 1", initPos, Color.White);
                initPos.Y += _spriteFont.LineSpacing * 2f;

                foreach (var (item, index) in level1Scores.Select((item, index) => (item, index)))
                {
                    string msg = $"Rank {index + 1} - {item} pts";
                    _spriteBatch.DrawString(_spriteFont, msg, initPos, Color.White);
                    initPos.Y += _spriteFont.LineSpacing * 1.2f;
                }

            }
            else
            {
                _spriteBatch.DrawString(_spriteFont,"No Scores for Level 1.", initPos,Color.Crimson);
                initPos.Y += _spriteFont.LineSpacing * 1.2f;

            }

            initPos.Y += _spriteFont.LineSpacing * 2f;

            if (level2Scores != null && level2Scores.Count > 0)
            {
                _spriteBatch.DrawString(_headerFont, "Ranks For Level 2", initPos, Color.White);
                initPos.Y += _spriteFont.LineSpacing * 2f;



                // set score - from the file
                foreach (var (item, index) in level2Scores.Select((item, index) => (item, index)))
                {
                    string msg = $"Rank {index + 1} - {item} pts";
                    _spriteBatch.DrawString(_spriteFont, msg, initPos, Color.White);
                    initPos.Y += _spriteFont.LineSpacing * 1.2f;
                }
            }
            else
            {
                _spriteBatch.DrawString(_spriteFont, "No Scores for Level 2.", initPos, Color.Crimson);
                initPos.Y += _spriteFont.LineSpacing * 1.2f;

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
