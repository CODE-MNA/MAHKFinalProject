using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAHKFinalProject.GameComponents
{
    public class ScoreFileManager
    {
        private const string filepath = "Scores/score.dat";

        public void recordScore(int score)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine(score);
            }
        }

        public List<int> getHighScores(int numberOfRanks)
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                string content = sr.ReadToEnd();

                List<string> scoreTexts = new List<string>();
                List<int> scores = new List<int>();
                List<int> ranks = new List<int>();

                if (!string.IsNullOrEmpty(content)) {
                    scoreTexts = content.Split("\n").ToList();
                    scores = scoreTexts.Select(int.Parse).ToList();
                    ranks = scores.OrderByDescending(s => s).Take(numberOfRanks).ToList();
                }

                return ranks;
            }
        }
    }
}
