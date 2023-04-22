using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public static class GameHandler
{
    public enum RunningState
    {
        NotYetStarted = 0,
        Running = 1,
        GameOver = -1,
    }
    
    private const float TotalTime = 30f;
    private static bool _isNight = true;

    public static float RemainingTime = TotalTime;
    public static readonly GameObject Player = GameObject.FindGameObjectWithTag("Player");
    public static RunningState GameState = RunningState.NotYetStarted;
    
    public static void EndGameAsDefeat()
    {
        GameState = RunningState.GameOver;
    }

    public static void EndGameAsWin()
    {
        GameState = RunningState.GameOver;

        SceneManager.LoadScene(_isNight ? "SeongHeonScene" : "PlayTestScene");
        _isNight = !_isNight;

        GameState = RunningState.NotYetStarted;
        RemainingTime = TotalTime;
    }
}
