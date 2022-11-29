using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.Scenes
{
    public class GameScene : DrawableGameComponent
    {
        protected List<GameComponent> GameComponents = new List<GameComponent>();
        public GameScene(Game game) : base(game)
        {
            game.Components.Add(this);
            Hide();
        }

        public void Hide()
        {
            Visible = false;
            Enabled = false;

        }
        public void Show()
        {
            Visible = true;
            Enabled = true;

        }

        public override void Initialize()
        {
            base.LoadContent();
            foreach (var item in GameComponents)
            {
                if (item is DrawableGameComponent)
                {
                    item.Initialize();
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (var item in GameComponents)
            {
                if (item is DrawableGameComponent)
                {
                    DrawableGameComponent component = (DrawableGameComponent)item;
                    if (component.Visible)
                    {

                        component.Draw(gameTime);
                    }
                }
            }


        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
            foreach (var item in GameComponents)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }


        }
    }
}
