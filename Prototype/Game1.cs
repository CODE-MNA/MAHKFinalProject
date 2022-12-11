using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Prototype.Managers;
using System.Collections.Generic;
using Prototype.Helpers;
using Prototype.Components;
using System;

namespace Prototype
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SongStage _songStagePrototype;
        private List<Song> _songs;
        Texture2D _explosionTexture;
        NoteMaker _noteMaker;
        string _noteMakerStatus;
        SpriteFont _font;
        Song _song;
        Vector2 _center;
        double _currSeconds;
        private MouseState _oldState;
        private float _bpm;
        ExplosionAnimation _explosion;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 2048;
            _graphics.PreferredBackBufferHeight = 1024;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            SharedVars.STAGE = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _center = new Vector2(SharedVars.STAGE.X/ 2, SharedVars.STAGE.Y / 2);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = this.Content.Load<SpriteFont>("regularFont");
            _song = this.Content.Load<Song>("WF_Endgame");
             _explosionTexture = this.Content.Load<Texture2D>("explosion");
            _songs = new List<Song>();
            Random random = new Random();
            Vector2 pos = new Vector2(random.Next((int)SharedVars.STAGE.X - 200),random.Next((int)SharedVars.STAGE.Y - 200));
            _explosion = new ExplosionAnimation(this, _spriteBatch, _explosionTexture, pos, 0.01f);
            _songStagePrototype = new SongStage(this, _song,"testEG",_explosion);
            _noteMaker = new NoteMaker(this, _songStagePrototype);
            this.Components.Add(_songStagePrototype);
            this.Components.Add(_noteMaker);
            _noteMaker.Enabled = false;
            _noteMakerStatus = "Note Maker Disabled!";
            
        }

        protected override void Update(GameTime gameTime)
        {
            _currSeconds = _songStagePrototype.GetSongSeconds();
            _bpm = _songStagePrototype.GetCurrentBeat();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState ms = Mouse.GetState();

            if (InputHelpers.WasJustClicked(ms.LeftButton,_oldState.LeftButton))
            {
                _songStagePrototype.PlayFromStart();
            }

            if (InputHelpers.WasJustClicked(ms.RightButton, _oldState.RightButton))
            {
                if (_noteMaker.Enabled)
                {
                    _noteMaker.Enabled = false;
                    _noteMakerStatus = "Note Maker Disabled!";
                }
                else
                {

                    _noteMakerStatus = "Note Maker Enabled!";

                    _noteMaker.Enabled = true;
                }
            }
           
            _oldState = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, _currSeconds.ToString(), _center, Color.Black);
            _spriteBatch.DrawString(_font,"BPM : " + _bpm, _center + new Vector2(-60, _font.LineSpacing), Color.Black);
            _spriteBatch.DrawString(_font,_noteMakerStatus, _center + new Vector2(-500, _font.LineSpacing*3), Color.Black);
            _spriteBatch.DrawString(_font,"Now Playing : " + _song.Name, _center + new Vector2(-300,_font.LineSpacing * 2), Color.Black);
            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public ExplosionAnimation CreateNewExplosion(Vector2 pos,float delay)
        {
            return new ExplosionAnimation(this, _spriteBatch, _explosionTexture, pos, delay);
        }
    }
}