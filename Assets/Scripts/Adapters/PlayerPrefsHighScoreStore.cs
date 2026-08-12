using UnityEngine;
using CheeseHeist.Core;

namespace CheeseHeist.Adapters
{
    public class PlayerPrefsHighScoreStore : IHighScoreStore
    {
        private const string Key = "CheeseHeist.HighScore";

        public int Load() => PlayerPrefs.GetInt(Key, 0);

        public void Save(int score)
        {
            PlayerPrefs.SetInt(Key, score);
            PlayerPrefs.Save();
        }
    }
}