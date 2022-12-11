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
using static MAHKFinalProject.Helpers.SharedVars;

namespace MAHKFinalProject.DrawableComponents
{


    public class Droplet : VisualizedNote
    {
        
        public Vector2 _targetPos;

        BaseLevelScene _levelScene;

        // temp
        public DropletLane _lane;

        AnimationTexture anim;

        float diffDist;
        Vector2 _initPos;

        public override Keys GetAssignedKey()
        {
            return _lane.TriggerKey;
        }
        

        public override void Initialize()
        {
            //Center + pos - origin
            
            
            base.Initialize();
        }
        protected override void LoadContent()
        {
         
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            g.SpriteBatch.Begin();

/*            g.SpriteBatch.Draw(_texture, new Rectangle((int)_position.X,(int)_position.Y,_texture.Width,_texture.Height), Color.AntiqueWhite);*/

            g.SpriteBatch.Draw(anim.GetAnimatedTexture(), _position, anim.GetCurrentFrame(), Color.AntiqueWhite,0,Vector2.Zero,scale:0.5f,SpriteEffects.None,0);

            g.SpriteBatch.End();
            
            base.Draw(gameTime);    
        }
        public Droplet(Game game, float hitBeat, Vector2 position,Vector2 target, Conductor conductor, BaseLevelScene level, DropletLane lane) : base(game, hitBeat ,position , conductor, level)
        {
            g = (Game1)game;
            _texture = g.Content.Load<Texture2D>("DestroyedSquare");
            _targetPos = target;
            Status = NoteStatus.NotSpawned;

            _levelScene = level;
            _lane = lane;

            // set animation class
            anim = new AnimationTexture(game, _texture, 64.0f, 64.0f, 1);
            // when the droplet is hitted, droplet is going to disappear after animating
            anim.OnAnimationStopped += () => base.EndNote(); 

        }


        public override void SpawnNote()
        {
            base.SpawnNote();
            diffDist = (_targetPos.Y - _position.Y);
           

            _initPos = _position;

         

            //Enque
            _levelScene.SpawnedNotes.Enqueue(this);
        }


        public override void UpdateActive(GameTime gameTime)
        {

            //DeQueue


            //play animation
            anim.Update(gameTime);

            //Score

        }

        public override void UpdateEnded(GameTime gameTime)
        {
            // after
          
        }

      
        public override void UpdateSpawned(GameTime gameTime)
        {

            if (g.Paused)
            {
                return;
            }

            //Dbefore - (HitBeat - getcurrentbeat)/ dbefore

            
            float interpolatedValue = GetInterpolationValue();


            //need to smooth further
            _position = Vector2.Lerp(_initPos, _targetPos, interpolatedValue);

            //0.174
            //tdiff
                //_position = new Vector2(_position.X, _position.Y + (diffDist*(float)gameTime.ElapsedGameTime.TotalSeconds/0.348f) );
        
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

        

        //public override float CalculateScore()
        //{
        //    float finalscore = 200;

        //    finalscore -= (MathF.Abs((float)_targetPos.Y - (float)_position.Y));

        //    Vector2 impPos = new Vector2((float)_position.X + anim.GetCurrentFrame().Width/2, (float)_position.Y + anim.GetCurrentFrame().Height/2);

        //    if (finalscore > 175 && impPos.Y >= _targetPos.Y  || finalscore > 179&& impPos.Y < _targetPos.Y)
        //    {
              
        //        return finalscore * 3f;
        //    }


        //    if (finalscore <= 0) finalscore = 0;

        //    return finalscore + (finalscore * 1.60f);
        //}

    }
}
