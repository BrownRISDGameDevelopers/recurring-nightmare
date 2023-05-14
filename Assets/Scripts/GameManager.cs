using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


public static class GameManager
{
    [Header("Difficulty Settings")]
    private const float TotalTime = 20f;
    private const int NumNightsToWin = 3;
    
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
    
    // TODO: Eventually move to Events to let other scripts know of Night Day switches
    private static readonly GameObject[] Healthpacks = GameObject.FindGameObjectsWithTag("Healthpack");
    private static readonly GameObject[] EnemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    private static readonly GameObject InventoryContainer = GameObject.Find("InventoryContainer");
    private static readonly GameObject Canvas = GameObject.Find("Canvas");
    private static GameObject _nightObjects;
    private static GameObject _dayObjects;
    
    // GameObject.Find does not work on inactive objects. So, I make a constantly active wrapper around the actual thing.
    private static (GameObject, GameObject) GetNightDayAssets()
    {
        GameObject day = null; GameObject night = null;
        var container = GameObject.Find("Container").GetComponent<Transform>();
        var transforms = container.GetComponentsInChildren<Transform>(true);
        foreach (var tr in transforms)
        {
            var obj = tr.gameObject;
            if (obj.name == "Night")
            {
                night = obj;
            }
            else if (obj.name == "Day")
            {
                day = obj;
            }
        }
        Assert.IsNotNull(day);
        Assert.IsNotNull(night);
        return (night, day);
    }

    public static void GameStart()
    {
        (_nightObjects, _dayObjects) = GetNightDayAssets();
        GameState = RunningState.Running;
    }
    
    public static void EndGameAsDefeat()
    {
        GameState = RunningState.GameOver;
        MusicManager.Instance.Stop();
        SceneManager.LoadScene((int) SceneIndexTable.Defeat);
    }

    public static void EndRoundAsWin()
    {
        if (_isNight && ++_numNightsSurvived == NumNightsToWin)
        {
            GameState = RunningState.GameOver;
            MusicManager.Instance.Stop();
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
        
        if (isSwitchingToDay) MusicManager.Instance.PlayDaytimeMusic();
        else MusicManager.Instance.PlayNightmareMusic();
        
        ResetPlayerPos();
        ChangeSprites(isSwitchingToDay);
        PrepSpawners(isSwitchingToDay);
        
        GameState = RunningState.NotYetStarted;
        RemainingTime = TotalTime;
    }

    private static void ResetPlayerPos()
    {
        Player.transform.position = DefaultPos;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private static void ChangeSprites(bool isSwitchingToDay)
    {
        InventoryContainer.GetComponent<InventoryContainer>().ChangeSprite(isSwitchingToDay);
        Canvas.GetComponent<TimeManager>().ChangeSprite(isSwitchingToDay);

        // This is verbose, but necessary to ensure that two layers are not active at the same time.
        if (isSwitchingToDay)
        {
            _nightObjects.SetActive(false);
            _dayObjects.SetActive(true);
        }
        else
        {
            _dayObjects.SetActive(false);
            _nightObjects.SetActive(true);
        }
    }

    private static void PrepSpawners(bool isSwitchingToDay)
    {
        foreach (var pack in Healthpacks)
        {
            pack.SetActive(isSwitchingToDay);
        }

        foreach (var spawner in EnemySpawners)
        {
            spawner.SetActive(isSwitchingToDay);
        }
    }
}
