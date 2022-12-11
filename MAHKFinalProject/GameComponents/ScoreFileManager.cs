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
        private const string filepath = @"Scores/score.dat";

        public void recordScore(int score)
        {
            try
            {
                // if there is no directory, create directory
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                }

                // write file with append mode
                using (StreamWriter sw = new StreamWriter(filepath, true))
                {
                    sw.WriteLine(score);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<int> getHighScores(int numberOfRanks)
        {
            try
            {
                // if there is no score file, it will return empty list
                if (File.Exists(filepath))
                {
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        string content = sr.ReadToEnd();

                        List<string> scoreTexts = new List<string>();
                        List<int> scores = new List<int>();
                        List<int> ranks = new List<int>();

                        if (!string.IsNullOrEmpty(content))
                        {
                            scoreTexts = content.Split("\n").ToList();
                            scoreTexts.RemoveAt(scoreTexts.Count - 1); // delete last empty list
                            scores = scoreTexts.Select(int.Parse).ToList();
                            ranks = scores.OrderByDescending(s => s).Take(numberOfRanks).ToList();
                        }

                        return ranks;
                    }
                }
                else
                {
                    return new List<int>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
