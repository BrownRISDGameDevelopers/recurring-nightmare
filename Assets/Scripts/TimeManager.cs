using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerBackground;
    [SerializeField] private GameObject startTextObj;
    [SerializeField] private Sprite dayBackground;
    [SerializeField] private Sprite nightBackground;
    
    void Start()
    {
        UpdateOverlay();
    }
    
    void UpdateOverlay()
    {
        timerText.text = Mathf.RoundToInt(GameManager.RemainingTime).ToString();
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
                GameManager.EndRoundAsWin();
            }
        }
        else if (GameManager.GameState == GameManager.RunningState.NotYetStarted && Input.anyKey)
        {
            startTextObj.SetActive(false);
            GameManager.GameStart();
        }
    }

    public void ChangeSprite(bool isSwitchingToDay)
    {
        timerBackground.sprite = isSwitchingToDay ? dayBackground : nightBackground;
    }
}
