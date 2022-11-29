﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.Scenes
{
    public class GameScene : DrawableGameComponent
    {

        public List<GameComponent> GameComponents { get; set; }

        // constructor
        public GameScene(Game game) : base(game)
        {
            GameComponents = new List<GameComponent>();
            hide();
        }

        // visible
        public virtual void show()
        {
            this.Visible = true;
            this.Enabled = true;
        }

        // hide
        public virtual void hide()
        {
            this.Visible = false;
            this.Enabled = false;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawableGameComponent component = null;
            foreach (GameComponent gc in GameComponents)
            {
                if (gc is DrawableGameComponent)
                {
                    component = (DrawableGameComponent)gc;
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

            foreach (GameComponent gc in GameComponents)
            {
                if (gc.Enabled)
                {
                    gc.Update(gameTime);
                }
            }
        }
    }
}
