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

        protected VisualizedNote(Game game,float hitBeat, Vector2 spawnPosition, Texture2D texture) : base(game)
        {
            _texture = texture;
            HitBeat = hitBeat;
            Status = NoteStatus.NotSpawned;
            this.Visible = false;
            _position = spawnPosition;
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





        public abstract void UpdateNotSpawned(GameTime gameTime);
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
