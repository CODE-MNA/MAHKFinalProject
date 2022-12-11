using MAHKFinalProject.Helpers;
using MAHKFinalProject.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace MAHKFinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        TestLevelScene testScene;
        public bool Paused;
        public Action OnPause;
        public Action OnUnPause;

        // MainMenu Scene
        private MainMenuScene _mainMenuScene;
        // Level select Scene
        private LevelSelectScene _levelSelectScene;
        // Help Scene
        private HelpScene _helpScene;
        // About Scene
        private AboutScene _aboutScene;
        // Ranking Scene
        private RankingScene _rankingScene;
        private KeyboardState _oldKeyboardState;

        public SpriteFont GlobalFont { get; set; }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;

            _graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        private void hideAllScenes()
        {
            foreach (GameScene item in Components)
            {
                item.Hide();
            }
        }

        protected override void Initialize()
        {
         
            

            SharedVars.STAGE = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GlobalFont = this.Content.Load<SpriteFont>("Fonts/regularFont");

            // TODO: use this.Content to load your game content here
            // load scenes
            _mainMenuScene = new MainMenuScene(this);
            _levelSelectScene = new LevelSelectScene(this);
            testScene = new TestLevelScene(this);
            _helpScene = new HelpScene(this);
            _aboutScene = new AboutScene(this);
            _rankingScene = new RankingScene(this);

            // hide all scenes
            hideAllScenes();

            // show main menu
            _mainMenuScene.Show();
        }

        protected override void Update(GameTime gameTime)
        {
            
            KeyboardState ks = Keyboard.GetState();

            if (ks.IsKeyDown(Keys.P) && _oldKeyboardState.IsKeyUp(Keys.P))
            {
                Paused = !Paused;

                if (Paused)
                {
                    OnPause?.Invoke();

                    _oldKeyboardState = ks;
                
                    return;
                }
                else
                {
                    OnUnPause?.Invoke();
                    _oldKeyboardState = ks;
                  
                    return;
                }
            }

        
            


            int index = _mainMenuScene.MenuComponent.SelectedIndex;
            if (_mainMenuScene.Enabled)
            {
                if(index == 5 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();
                    // reset game
                    testScene = new TestLevelScene(this);
                    testScene.Show();

                }else if (index == 4 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAllScenes();
                    Exit();
                }
                else if (index == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();
                    _levelSelectScene.Show();
                }
                else if (index == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();
                    _helpScene.Show();
                }
                else if (index == 2 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();
                    _aboutScene = new AboutScene(this);
                    _aboutScene.Show();
                }
                else if (index == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();
                    _rankingScene = new RankingScene(this);
                    _rankingScene.Show();
                }
            }
            else if (ks.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                hideAllScenes();
                _mainMenuScene.Show();
            }

            _oldKeyboardState = ks;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}