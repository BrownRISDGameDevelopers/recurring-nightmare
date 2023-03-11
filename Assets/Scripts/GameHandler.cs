using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject timerTextObj;
    [SerializeField] private GameObject startTextObj;
    [SerializeField] private GameObject endTextObj;
    [SerializeField] private float remainingTime = 45.0F;
    private bool _timerActive = false;
    private bool _isRunning = false;
    private bool _gameStarted = false;

    private Text _timerText;
    private Text _startText;
    private Text _endText;

    // Start is called before the first frame update
    void Start()
    {
        _timerActive = true;
        _timerText = timerTextObj.GetComponent<Text>();
        _startText = startTextObj.GetComponent<Text>();
        _endText = endTextObj.GetComponent<Text>();
        UpdateOverlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isRunning)
        {
            UpdateTimer();
            UpdateOverlay();
        } else if (!_gameStarted && Input.anyKey)
        {
            _isRunning = true;
            _gameStarted = true;
            startTextObj.SetActive(false);
        }
    }

    void UpdateTimer()
    {
        if (!_timerActive) return;
        
        if (remainingTime > 0) 
        {
            remainingTime -= Time.deltaTime;
        } 
        else 
        {
            // Player won
            remainingTime = 0;
            GameOver("Game Over");
        }
    }
    
    void UpdateOverlay()
    {
        _timerText.text = "Time: " + (int)remainingTime;
    }

    void GameStart()
    {
        _isRunning = true;
    }

    public void GameOver(string msg)
    {
        _timerActive = false;
        _isRunning = false;
        _endText.text = msg;
        endTextObj.SetActive(true);
    }

    public bool isRunning()
    {
        return _isRunning;
    }
}
