using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject timerTextObj;
    [SerializeField] private GameObject startTextObj;
    [SerializeField] private GameObject endTextObj;
    [SerializeField] private float remainingTime = 45.0F;
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
        _timerText = timerTextObj.GetComponent<Text>();
        _startText = startTextObj.GetComponent<Text>();
        _endText = endTextObj.GetComponent<Text>();
        UpdateOverlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState == RunningState.Running)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime > 0)
            {
                UpdateOverlay();
            }
            else
            {
                EndGame("Round Over");
            }
        }
        else if (GameState == RunningState.NotYetStarted && Input.anyKey)
        {
            GameState = RunningState.Running;
            startTextObj.SetActive(false);
        }
    }

    void UpdateOverlay()
    {
        _timerText.text = "Time: " + (int)remainingTime;
    }

    public void EndGame(string msg)
    {
        GameState = RunningState.GameOver;
        _endText.text = msg;
        endTextObj.SetActive(true);
    }
}
