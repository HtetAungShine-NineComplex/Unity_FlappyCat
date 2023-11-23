using System;
using UnityEngine;

public static class HighScoreSystem
{
    public static void Start()
    {
        //ResetHighScore();
        Bird.GetInstance.OnDied += Bird_OnDied;

    }

    private static void Bird_OnDied(object sender, EventArgs e)
    {
        IsNewHighScore(Level.GetInstance.GetPipePassNum());
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool IsNewHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
}
