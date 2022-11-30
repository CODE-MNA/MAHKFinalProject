using MAHKFinalProject.LevelSerialization;
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

     

        public Conductor(Game game, string stageName, Song song, float bpm) : base(game)
        {
            StageName = stageName;
        
            _song = song;
            this._bpm = bpm;
        
        }

        

        public void PlayFromStart()
        {
            MediaPlayer.Play(_song);

        }

        internal float GetQuantizedBeat()
        {
            int shortBeat = (int)MathF.Round(GetCurrentBeat() * 4);
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
    }
}
