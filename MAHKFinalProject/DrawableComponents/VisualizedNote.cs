using MAHKFinalProject.GameComponents;
using MAHKFinalProject.RhythmComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MAHKFinalProject.DrawableComponents
{
    public abstract class VisualizedNote : DrawableGameComponent  
    {
        protected Game1 g;

        public float HitBeat { get; set; }
        public NoteStatus Status { get; set; } = NoteStatus.NotSpawned;

        public Texture2D _texture;

        public Vector2 _position;

        protected Conductor _conductor;
        private readonly float PRE_DELAY = 0.125f;

        protected VisualizedNote(Game game,float hitBeat, Vector2 spawnPosition, Conductor conductor) : base(game)
        {
           
            HitBeat = hitBeat;
            Status = NoteStatus.NotSpawned;
            this.Visible = false;
            this.Enabled = true;
            _position = spawnPosition;
            _conductor = conductor;
            
        }

        #region Transitions
        public void SpawnNote()
        {
            this.Visible = true;
            Status = NoteStatus.Spawned;
        }
        public  void ActivateNote()
        {
            Status = NoteStatus.Active;
        }
        public  void EndNote()
        {

            Status = NoteStatus.Ended;
        }
        #endregion





        public virtual void UpdateNotSpawned(GameTime gameTime)
        {
          

            if(HitBeat == _conductor.GetQuantizedBeat() + PRE_DELAY)
            {
                SpawnNote();
                
            }

        }
        public abstract void UpdateSpawned(GameTime gameTime);
        public abstract void UpdateActive(GameTime gameTime);

        public abstract void UpdateEnded(GameTime gameTime);    



        public override void Update(GameTime gameTime)
        {

            switch (Status)
            {
                case NoteStatus.NotSpawned:
                    UpdateNotSpawned(gameTime);
                    break;
                case NoteStatus.Spawned:
                    UpdateSpawned(gameTime);
                    break;
                case NoteStatus.Active:
                    UpdateActive(gameTime);
                    break;
                case NoteStatus.Ended:
                    UpdateEnded((GameTime) gameTime);
                    break;
                default:
                    break;
            }


            base.Update(gameTime);
        }
    }
}
