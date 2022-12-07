using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.DrawableComponents
{
    public class DropletLane : DrawableGameComponent
    {
        public Vector2 dropletSpawnPos;

        public Vector2 ImpactPos;

        public Vector2 _spawnCorner;
        public int _width;
        public int _height;

        public Rectangle _laneRect;
        Texture2D _texture;

        public Game1 g;
        
        public DropletLane(Game game,Vector2 spawnCorner,int width,int height, Texture2D texture) : base(game)
        {
            g = (Game1)game;
            _spawnCorner = spawnCorner;
            _width = width;
            _height = height;

            _texture = texture;
            _laneRect = new Rectangle((int)spawnCorner.X, (int)spawnCorner.Y, width, height);

            dropletSpawnPos = new Vector2(spawnCorner.X + width/2, spawnCorner.Y + 10);
            ImpactPos = new Vector2(dropletSpawnPos.X , dropletSpawnPos.Y + height - 20);
        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();
            g.SpriteBatch.Draw(_texture, _laneRect, Color.AliceBlue);
            g.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
