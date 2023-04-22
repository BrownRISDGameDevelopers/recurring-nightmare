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
        _timerText.text = "Time: " + (int)GameManager.RemainingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState == GameManager.RunningState.Running)
        {
            GameManager.RemainingTime -= Time.deltaTime;
            if (GameManager.RemainingTime > 0)
            {
                UpdateOverlay();
            }
            else
            {
                GameManager.EndGameAsWin();
            }
        }
        else if (GameManager.GameState == GameManager.RunningState.NotYetStarted && Input.anyKey)
        {
            GameManager.GameState = GameManager.RunningState.Running;
            startTextObj.SetActive(false);
        }
    }
}
