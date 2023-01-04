using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.Scenes
{

    public class MainMenuScene : GameScene
    {
        private SpriteBatch _spriteBatch;
        private MenuComponent _menuComponent;
        private string[] _menuItems = { "Start - Level 1", "Start - Level 2","Help", "About", "Leader Board", "Exit"  };

        public MenuComponent MenuComponent { get => _menuComponent; }

        public MainMenuScene(Game game) : base(game)
        {
            Game1 g = (Game1)game;
            this._spriteBatch = g.SpriteBatch;
            SpriteFont font1 = g.Content.Load<SpriteFont>("Fonts/hilightFont");
            SpriteFont font2 = g.Content.Load<SpriteFont>("Fonts/regularFont");
            _menuComponent = new MenuComponent(g, this._spriteBatch, font1, font2, 0, _menuItems);
            this.GameComponents.Add(_menuComponent);
        }

        

    }
}
