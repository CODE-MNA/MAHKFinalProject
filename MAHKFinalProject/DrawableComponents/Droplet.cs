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

            g.SpriteBatch.Draw(anim.GetAnimatedTexture(), _position, anim.GetCurrentFrame(), Color.AntiqueWhite,0,Vector2.Zero,scale:0.7f,SpriteEffects.None,0);

            g.SpriteBatch.End();
            
            base.Draw(gameTime);    
        }
        public Droplet(Game game, float hitBeat, Vector2 position,Vector2 target, Conductor conductor, BaseLevelScene level, DropletLane lane) : base(game, hitBeat ,position , conductor, level)
        {
            g = (Game1)game;
            _texture = g.Content.Load<Texture2D>("Animation/DestroyedSquare");
            _targetPos = target;
            Status = NoteStatus.NotSpawned;

            _levelScene = level;
            _lane = lane;

            // set animation class
            anim = new AnimationTexture(game, _texture, 64.0f, 64.0f, 0.3f);
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
     
          
        }

      
        public override void UpdateSpawned(GameTime gameTime)
        {

            if (g.Paused)
            {
                return;
            }

          
            
            float interpolatedValue = GetInterpolationValue();


            _position = Vector2.Lerp(_initPos, _targetPos, interpolatedValue);

           
        
        }
    

    }
}
