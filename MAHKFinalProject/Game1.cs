using MAHKFinalProject.Helpers;
using MAHKFinalProject.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MAHKFinalProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _dropletTexture;
        private Texture2D _laneTexture;

        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }

        TestLevelScene testScene;

        public SpriteBatch SpriteBatch { get => _spriteBatch; set => _spriteBatch = value; }

        // MainMenu Scene
        private MainMenuScene _mainMenuScene;
        // Level select Scene
        private LevelSelectScene _levelSelectScene;
        // Help Scene
        private HelpScene _helpScene;
        // About Scene
        private AboutScene _aboutScene;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        private void hideAllScenes()
        {
            foreach (GameScene item in Components)
            {
                item.hide();
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
            _dropletTexture = this.Content.Load<Texture2D>("droplet");
            _laneTexture = this.Content.Load<Texture2D>("dropletLane");

            testScene = new TestLevelScene(this,_dropletTexture,_laneTexture);
            testScene.Show();
            // TODO: use this.Content to load your game content here
            SharedVars.STAGE = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _mainMenuScene = new MainMenuScene(this);
            this.Components.Add(_mainMenuScene);

            _levelSelectScene = new LevelSelectScene(this);
            this.Components.Add(_levelSelectScene);

            _helpScene = new HelpScene(this);
            this.Components.Add(_helpScene);

            _aboutScene = new AboutScene(this);
            this.Components.Add(_aboutScene);

            hideAllScenes();
            _mainMenuScene.show();
        }

        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            int index = _mainMenuScene.MenuComponent.SelectedIndex;
            if (_mainMenuScene.Enabled)
            {
                if (index == 3 && ks.IsKeyDown(Keys.Enter))
                {
                    hideAllScenes();
                    Exit();
                }
                else if (index == 0 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.hide();
                    _levelSelectScene.show();
                }
                else if (index == 1 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.hide();
                    _helpScene.show();
                }
                else if (index == 2 && ks.IsKeyDown(Keys.Enter))
                {
                    _mainMenuScene.hide();
                    _aboutScene.show();
                }
            }
            else if (ks.IsKeyDown(Keys.Escape))
            {
                hideAllScenes();
                _mainMenuScene.show();
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