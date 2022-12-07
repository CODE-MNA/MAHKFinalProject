using MAHKFinalProject.Helpers;
using MAHKFinalProject.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MAHKFinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        TestLevelScene testScene;


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


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            // TODO: Add your initialization logic here
            SharedVars.STAGE = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

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

            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            int index = _mainMenuScene.MenuComponent.SelectedIndex;
            if (_mainMenuScene.Enabled)
            {
                if(index == 5 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();

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
                    _aboutScene.Show();
                }
                else if (index == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.Hide();
                    _rankingScene.Show();
                }
            }
            else if (ks.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Stop();
                hideAllScenes();
                _mainMenuScene.Show();
            }

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