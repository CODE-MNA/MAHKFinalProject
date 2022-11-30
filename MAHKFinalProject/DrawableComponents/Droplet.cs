using MAHKFinalProject.GameComponents;
using MAHKFinalProject.RhythmComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.DrawableComponents
{


    public class Droplet : VisualizedNote
    {
        float _velocityFall;
        Vector2 _targetPos;


        double prevFrameSeconds;
        double currentFrameSeconds;
        double startSeconds;
        double endSeconds;
        float initY;

        float step;
        public override void Initialize()
        {
            
            base.Initialize();
        }
        protected override void LoadContent()
        {
         
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();
            g.SpriteBatch.Draw(_texture,
                new Rectangle((int)_position.X,(int)_position.Y,_texture.Width, _texture.Height),Color.AliceBlue);
            g.SpriteBatch.End();
            
            base.Draw(gameTime);    
        }
        public Droplet(Game game, float hitBeat, Vector2 position,Vector2 target, Conductor conductor) : base(game, hitBeat ,position , conductor)
        {
            g = (Game1)game;
            _texture = g.Content.Load<Texture2D>("droplet");
            _targetPos = target;
            Status = NoteStatus.NotSpawned;

            startSeconds = _conductor.GetSongSeconds();
            endSeconds = _conductor.GetSecondsFromBeat(HitBeat);
            initY = _position.Y;
           
        }

        public override void UpdateActive(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void UpdateEnded(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

      
        public override void UpdateSpawned(GameTime gameTime)
        {
            //Instead of using update, use conductors update time

            //60 FPS * 
            if (_targetPos.Y <= _position.Y)
            {
                step = 0;
                _position = _targetPos;
                this.Enabled = false;
            }
            else
            {
                float oldY = _position.Y;
                _position = new Vector2(_position.X,step + GetLerpY());
                step += _position.Y + oldY/10;
            }
            
            
        }
        float GetLerpY()
        {
            currentFrameSeconds = _conductor.GetSongSeconds();

            float b1 = (float)currentFrameSeconds-(float)startSeconds ;

            float b2 = _targetPos.Y - initY;

            float b3 = (float)endSeconds-(float)startSeconds;
            float answer = ((b1 * b2) / b3) + initY;
            return answer /100 ;
        }


    }
}
