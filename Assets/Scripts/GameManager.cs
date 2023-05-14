using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public static class GameManager
{
    [Header("Difficulty Settings")]
    private const float TotalTime = 5f;
    private const int NumNightsToWin = 1;
    
    private static readonly Vector3 DefaultPos = new(-7.3f, -15, -5);

    public enum RunningState
    {
        NotYetStarted = 0,
        Running = 1,
        GameOver = -1,
    }
    
    public enum SceneIndexTable
    {
        Title = 0,
        Instructions = 1,
        Credits = 2,
        Game = 3,
        Win = 4,
        Defeat = 5
    }

    [Header("Initial Values")]
    private static int _numNightsSurvived = 0;
    private static bool _isNight = false;
    public static float RemainingTime = TotalTime;
    public static RunningState GameState = RunningState.NotYetStarted;

    
    public static readonly GameObject Player = GameObject.FindGameObjectWithTag("Player");
    
    private static readonly GameObject[] Healthpacks = GameObject.FindGameObjectsWithTag("Healthpack");
    private static readonly GameObject[] EnemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    private static readonly GameObject InventoryContainer = GameObject.Find("InventoryContainer");
    private static readonly GameObject Canvas = GameObject.Find("Canvas");
    
    public static void GameStart()
    {
        GameState = RunningState.Running;
    }
    
    public static void EndGameAsDefeat()
    {
        GameState = RunningState.GameOver;
        SceneManager.LoadScene((int) SceneIndexTable.Defeat);
    }

    public static void EndRoundAsWin()
    {
        if (_isNight && ++_numNightsSurvived == NumNightsToWin)
        {
            GameState = RunningState.GameOver;
            SceneManager.LoadScene((int) SceneIndexTable.Win);
        }
        else
        {
            SwitchTimeOfDay(_isNight);
            _isNight = !_isNight;
        }
    }

    private static void SwitchTimeOfDay(bool isSwitchingToDay)
    {
        GameState = RunningState.GameOver;
        
        Player.transform.position = DefaultPos;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (isSwitchingToDay) MusicManager.Instance.PlayDaytimeMusic();
        else MusicManager.Instance.PlayNightmareMusic();
        
        InventoryContainer.GetComponent<InventoryContainer>().ChangeSprite(isSwitchingToDay);
        Canvas.GetComponent<TimeManager>().ChangeSprite(isSwitchingToDay);
        
        foreach (var pack in Healthpacks)
        {
            pack.SetActive(isSwitchingToDay);
        }

        foreach (var spawner in EnemySpawners)
        {
            spawner.SetActive(isSwitchingToDay);
        }
        
        GameState = RunningState.NotYetStarted;
        RemainingTime = TotalTime;
    }
}
