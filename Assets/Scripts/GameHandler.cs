using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public static class GameHandler
{
    private const float TotalTime = 30f;
    public static float RemainingTime = TotalTime;
    public static readonly GameObject Player = GameObject.FindGameObjectWithTag("Player");

    public enum RunningState
    {
        NotYetStarted = 0,
        Running = 1,
        GameOver = -1,
    }
    public static RunningState GameState = RunningState.NotYetStarted;
    
    public static void EndGameAsDefeat()
    {
        GameState = RunningState.GameOver;
    }

    public static void EndGameAsWin()
    {
        GameState = RunningState.GameOver;
        SceneManager.LoadScene("SeongHeonScene");
        GameState = RunningState.Running;
        RemainingTime = TotalTime;
    }
}
