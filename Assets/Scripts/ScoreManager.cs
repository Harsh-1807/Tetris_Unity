using UnityEngine;

public static class ScoreManager
{
    public static int Score { get; private set; }
    public static int Level { get; private set; }
    public static int LinesCleared { get; private set; }

    static ScoreManager()
    {
        ResetScore();
    }

    public static void AddScore(int lines)
    {
        LinesCleared += lines;
        int pointsGained = 0;
        switch (lines)
        {
            case 1:
                pointsGained = 100 * Level;
                break;
            case 2:
                pointsGained = 300 * Level;
                break;
            case 3:
                pointsGained = 500 * Level;
                break;
            case 4:
                pointsGained = 800 * Level;
                break;
        }
        Score += pointsGained;

        if (LinesCleared / 10 > Level - 1)
        {
            LevelUp();
        }

        Debug.Log($"Score updated: {Score}, Lines Cleared: {LinesCleared}, Level: {Level}");
    }

    public static void LevelUp()
    {
        Level++;
        Debug.Log($"Level Up! New Level: {Level}");
    }

    public static void ResetScore()
    {
        Score = 0;
        Level = 1;
        LinesCleared = 0;
    }
}