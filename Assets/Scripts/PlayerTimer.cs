using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerTimer : MonoBehaviour
{
    [SerializeField] private float remainingTime = 45.0F;
    
    private bool _timerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _timerActive = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_timerActive) return;
        
        if (remainingTime > 0) 
        {
            remainingTime -= Time.deltaTime;
            DisplayTime();
        } 
        else 
        {
            // Player won
            remainingTime = 0;
            _timerActive = false;
            DisplayTime();
            PlayerWin();
        }
    }

    private void DisplayTime() 
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime % 60);
        string text = $"{minutes:00}:{seconds:00}";
        // Debug.Log(text);
    }

    void PlayerWin()
    {
        Debug.Log("Player Won!");
    }
}
