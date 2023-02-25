using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimer : MonoBehaviour
{
    [SerializeField] private float time_remaining = 10.0F;
    [SerializeField] private bool timer_is_running = false;

    // Start is called before the first frame update
    void Start()
    {
        timer_is_running = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (timer_is_running) {
            if (time_remaining > 0.0F) {
                time_remaining -= Time.deltaTime;
                display_time();
            } else {
                time_remaining = 0.0F;
                timer_is_running = false;
                display_time();
            }
        }
    }

    void display_time() {
        float minutes = Mathf.FloorToInt(time_remaining / 60);
        float seconds = Mathf.FloorToInt(time_remaining % 60);
        string text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // Debug.Log(text);
    }
}
