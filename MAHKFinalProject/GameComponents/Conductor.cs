﻿using MAHKFinalProject.LevelSerialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.GameComponents
{
    public class Conductor : GameComponent
    {
        string _stageName;
        public string StageName
        {
            get { return _stageName; }
            set { _stageName = value; }
        }
        Song _song;
        float _bpm;
        Game1 g;
     

        public Conductor(Game game, string stageName, Song song, float bpm) : base(game)
        {
            g = (Game1)game;
            StageName = stageName;
        
            _song = song;
            this._bpm = bpm;

            g.OnPause += ()=>this.PauseSong();
            g.OnUnPause += ()=>this.ContinueSong();

        }

       
        public void ContinueSong()
        {
            MediaPlayer.Resume();
        }
        public void PlayFromStart()
        {
            MediaPlayer.Play(_song);

        }
        public void PauseSong()
        {
            MediaPlayer.Pause();
        }
        internal float GetQuantizedBeat()
        {
            float shortBeat = GetCurrentBeat() * 4;
            return (float)shortBeat / 8f;

        }

        public float GetCurrentBeat()
        {
            //If bpm is 120, at 60 seconds, curr beat should be 120
            return (float)GetSongSeconds() * (_bpm / 60f);
        }
        public double GetSongSeconds()
        {
            return MediaPlayer.PlayPosition.TotalSeconds;
        }

        public double GetSecondsFromBeat(float beat)
        {
            return beat * 60 / _bpm;
        }

        public void FadeOutSong()
        {
            deltaVolume = -0.01f;

            _fading = true;
            FadingTriggered = true;
        }

        public void FadeInSong()
        {
            PlayFromStart();
            MediaPlayer.Volume = 0;
            deltaVolume = 0.003f;
            _fading = true;
            HasStarted = true;

        }


        public override void Update(GameTime gameTime)
        {

            if (_fading && !MediaPlayer.IsMuted)
            {
                MediaPlayer.Volume += deltaVolume;

                if(deltaVolume < 0)
                {
                    if(MediaPlayer.Volume < 0 )
                    {
                        MediaPlayer.Stop();
                        _fading =false;
                    }

                }
                else
                {
                    if (MediaPlayer.Volume > 0.8)
                    {
                      
                        _fading = false;
                    }


                }
            }

            base.Update(gameTime);
        }

        bool _fading = false;
        private float deltaVolume;

        public bool FadingTriggered { get; private set; }   
       public bool HasStarted { get; private set; }
    }
}
