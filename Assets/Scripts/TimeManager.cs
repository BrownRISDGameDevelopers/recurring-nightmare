using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private GameObject timerTextObj;
    [SerializeField] private GameObject startTextObj;
    [SerializeField] private GameObject endTextObj;
    
    private Text _timerText;
    private Text _startText;
    private Text _endText;
    // Start is called before the first frame update
    void Start()
    {
        _timerText = timerTextObj.GetComponent<Text>();
        _startText = startTextObj.GetComponent<Text>();
        _endText = endTextObj.GetComponent<Text>();
        UpdateOverlay();
    }
    
    void UpdateOverlay()
    {
        _timerText.text = "Time: " + (int)GameHandler.RemainingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.GameState == GameHandler.RunningState.Running)
        {
            GameHandler.RemainingTime -= Time.deltaTime;
            if (GameHandler.RemainingTime > 0)
            {
                UpdateOverlay();
            }
            else
            {
                GameHandler.EndGameAsWin();
            }
        }
        else if (GameHandler.GameState == GameHandler.RunningState.NotYetStarted && Input.anyKey)
        {
            GameHandler.GameState = GameHandler.RunningState.Running;
            startTextObj.SetActive(false);
        }
    }
}
