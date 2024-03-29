﻿using MAHKFinalProject.GameComponents;
using MAHKFinalProject.RhythmComponents;
using MAHKFinalProject.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using static MAHKFinalProject.GameComponents.Conductor;

namespace MAHKFinalProject.DrawableComponents
{
    public abstract class VisualizedNote : DrawableGameComponent  
    {
        protected Game1 g;


        public float HitBeat { get; set; }
        public NoteStatus Status { get; set; } = NoteStatus.NotSpawned;
        

        public Texture2D _texture;

        public Vector2 _position;

        public Action OnTapped { get; set; }

        private double _startSeconds;
        private double _endSeconds;
        protected float diffSeconds;

        protected Conductor _conductor;
        public float DELAY_BETWEEN_SPAWN_AND_HIT = 1f;

        SyncMode _syncMode;
        protected VisualizedNote(Game game,float hitBeat, Vector2 spawnPosition, Conductor conductor, BaseLevelScene LEVEL) : base(game)
        {
           
            HitBeat = hitBeat;
            Status = NoteStatus.NotSpawned;
            this.Visible = false;
            this.Enabled = true;
            _position = spawnPosition;
            _conductor = conductor;
            _endSeconds = _conductor.GetSecondsFromBeat(HitBeat);

            _syncMode = _conductor.Mode;

            if(_syncMode == SyncMode.Seconds)
            {
                //Convert to seconds
                DELAY_BETWEEN_SPAWN_AND_HIT = (DELAY_BETWEEN_SPAWN_AND_HIT * 60)/_conductor._bpm;
            }
        }

        #region Transitions
        public virtual void SpawnNote()
        {
            this.Visible = true;
            Status = NoteStatus.Spawned;
            
            _startSeconds = _conductor.GetSecondsFromBeat(HitBeat - DELAY_BETWEEN_SPAWN_AND_HIT);

            diffSeconds = (float)(_endSeconds - _startSeconds);

        }
        public virtual void ActivateNote()
        {
            Status = NoteStatus.Active;
            OnTapped?.Invoke();
        }
        public virtual void EndNote()
        {

            Status = NoteStatus.Ended;
            this.Enabled = false;
            this.Visible = false;
        }
        #endregion


        protected float GetInterpolationValue()
        {
            if(_syncMode == SyncMode.Seconds)
            {
                return (DELAY_BETWEEN_SPAWN_AND_HIT - (HitBeat - (float)_conductor.GetSongSeconds()) / DELAY_BETWEEN_SPAWN_AND_HIT);
            }

            return (DELAY_BETWEEN_SPAWN_AND_HIT - (HitBeat - _conductor.GetCurrentBeat()/2)) / DELAY_BETWEEN_SPAWN_AND_HIT;
        }


        public virtual void UpdateNotSpawned(GameTime gameTime)
        {
            if (_syncMode == SyncMode.Seconds)
            {

                if (MathF.Abs(HitBeat - (float)_conductor.GetSongSeconds()) < DELAY_BETWEEN_SPAWN_AND_HIT && _conductor.GetCurrentBeat() > 0)
                {
                    SpawnNote();
                }
            }
            else
            {
                if (MathF.Abs(HitBeat - _conductor.GetCurrentBeat() / 2) < DELAY_BETWEEN_SPAWN_AND_HIT && _conductor.GetCurrentBeat() > 0)
                {

                    SpawnNote();

                }

            }

        }
        public abstract void UpdateSpawned(GameTime gameTime);
        public abstract void UpdateActive(GameTime gameTime);

        public abstract void UpdateEnded(GameTime gameTime);


        public abstract Keys GetAssignedKey();


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

        public virtual float CalculateScore()
        {
            float diff;

            float range = 0.0525f;
            if (_syncMode == SyncMode.Beats)
            {

             diff = MathF.Abs(HitBeat - _conductor.GetCurrentBeat() / 2f);
            }
            else
            {
                diff = MathF.Abs((float)(HitBeat - _conductor.GetSongSeconds()));
                range = 0.036f;
            }
            float score = 800;
            if(diff < range){

            }
            else
            {
                score -= diff * 1400;
            }

        

            if(score <= 0 ) score = 0;
            return score;
        }
    }
}
