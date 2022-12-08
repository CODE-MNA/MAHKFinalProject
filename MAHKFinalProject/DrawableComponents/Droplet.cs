using MAHKFinalProject.GameComponents;
using MAHKFinalProject.RhythmComponents;
using MAHKFinalProject.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public Vector2 _targetPos;

        TestLevelScene _levelScene;

        // temp
        DropletLane _lane;

        
        
        public override Keys GetAssignedKey()
        {
            return _lane.TriggerKey;
        }
        

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
        public Droplet(Game game, float hitBeat, Vector2 position,Vector2 target, Conductor conductor, TestLevelScene level, DropletLane lane) : base(game, hitBeat ,position , conductor, level)
        {
            g = (Game1)game;
            _texture = g.Content.Load<Texture2D>("droplet");
            _targetPos = target;
            Status = NoteStatus.NotSpawned;


            _levelScene = level;
            _lane = lane;
          

            

        }


        public override void SpawnNote()
        {
            base.SpawnNote();
            _velocityFall = (_targetPos.Y - _position.Y) / diffSeconds;

            //Enque
            _levelScene.SpawnedDroplets.Enqueue(this);
        }


        public override void UpdateActive(GameTime gameTime)
        {

            //DeQueue
            
        
            //play animation


            //Score

        }

        public override void UpdateEnded(GameTime gameTime)
        {
            // after
          
        }

      
        public override void UpdateSpawned(GameTime gameTime)
        {
            //Instead of using update, use conductors update time

           
                _position = new Vector2(_position.X, _position.Y + _velocityFall / 193);
         

         

        }
        //float GetLerpY()
        //{
        //    currentFrameSeconds = _conductor.GetSongSeconds();

        //    float b1 = (float)currentFrameSeconds-(float)startSeconds ;

        //    float b2 = _targetPos.Y - initY;

        //    float b3 = (float)endSeconds-(float)startSeconds;
        //    float answer = ((b1 * b2) / b3) + initY;
        //    return answer;
        //}


    }
}
