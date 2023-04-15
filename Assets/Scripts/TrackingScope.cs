using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingScope : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private GameObject tracker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            Debug.Log("start track");
            tracker.SendMessage("startTrack");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == targetTag)
        {
            Debug.Log("stop track");
            tracker.SendMessage("stopTrack");
        }
    }
}
