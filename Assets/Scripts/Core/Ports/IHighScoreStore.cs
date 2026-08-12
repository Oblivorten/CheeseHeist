namespace CheeseHeist.Core
{
    public interface IHighScoreStore
    {
        int Load();
        void Save(int score);
    }
}