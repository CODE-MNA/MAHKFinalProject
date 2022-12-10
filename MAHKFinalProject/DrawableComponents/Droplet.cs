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
        public DropletLane _lane;

        //Animation anim;

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




            g.SpriteBatch.Draw(_texture, new Rectangle((int)_position.X,(int)_position.Y,_texture.Width,_texture.Height), Color.AntiqueWhite);

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
          
            
            // anim.OnAnimationStopped += ()=> EndNote();
            

        }


        public override void SpawnNote()
        {
            base.SpawnNote();
            _velocityFall = (_targetPos.Y - _position.Y) / diffSeconds;

            //Enque
            _levelScene.SpawnedNotes.Enqueue(this);
        }


        public override void UpdateActive(GameTime gameTime)
        {

            //play animation
            
        }

        public override void UpdateEnded(GameTime gameTime)
        {
            // after
          
        }

      
        public override void UpdateSpawned(GameTime gameTime)
        {
            //Instead of using update, use conductors update time

           
                _position = new Vector2(_position.X, _position.Y + _velocityFall / 220);
         

         

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

        

        public override float CalculateScore()
        {
            float finalscore = 200;

            finalscore -= (MathF.Abs((float)_targetPos.Y - (float)_position.Y));

            if (finalscore > 185 && _position.Y >= _targetPos.Y  || finalscore > 187&& _position.Y < _targetPos.Y)
            {
              
                return finalscore * 3.25f;
            }


            if (finalscore <= 0) finalscore = 0;

            return finalscore;
        }

    }
}
