using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public static class GameManager
{
    private static readonly Vector3 DefaultPos = new(-30, -8, -5);

    public enum RunningState
    {
        NotYetStarted = 0,
        Running = 1,
        GameOver = -1,
    }
    
    private const float TotalTime = 4f;
    private static bool _isNight = true;

    public static float RemainingTime = TotalTime;
    
    public static readonly GameObject Player = GameObject.FindGameObjectWithTag("Player");
    private static readonly GameObject[] Healthpacks = GameObject.FindGameObjectsWithTag("Healthpack");
    private static readonly GameObject[] EnemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    private static readonly GameObject InventoryContainer = GameObject.Find("InventoryContainer");

    public static RunningState GameState = RunningState.NotYetStarted;
    
    public static void EndGameAsDefeat()
    {
        GameState = RunningState.GameOver;
    }

    public static void EndRoundAsWin()
    {
        SwitchTimeOfDay(_isNight);
        _isNight = !_isNight;
    }

    private static void SwitchTimeOfDay(bool isSwitchingToDay)
    {
        GameState = RunningState.GameOver;
        
        Player.transform.position = DefaultPos;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if (isSwitchingToDay) MusicManager.Instance.PlayDaytimeMusic();
        else MusicManager.Instance.PlayNightmareMusic();
        
        InventoryContainer.GetComponent<InventoryContainer>().ChangeSprite(isSwitchingToDay);
        
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
