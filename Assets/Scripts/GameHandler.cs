using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject timerTextObj;
    [SerializeField] private GameObject startTextObj;
    [SerializeField] private GameObject endTextObj;
    [SerializeField] private float remainingTime = 45.0F;
    private bool _timerActive;
    public enum RunningState
    {
        NotYetStarted = 0,
        Running = 1,
        GameOver = -1,
    }
    public RunningState GameState { get; private set; } = RunningState.NotYetStarted;

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
        switch (GameState)
        {
            case RunningState.Running:
                UpdateTimer();
                UpdateOverlay();
                break;
            case RunningState.NotYetStarted when Input.anyKey:
                GameState = RunningState.Running;
                startTextObj.SetActive(false);
                break;
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

    public void GameOver(string msg)
    {
        GameState = RunningState.GameOver;
        _timerActive = false;
        _endText.text = msg;
        endTextObj.SetActive(true);
    }
}
