using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitcher : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> clips;
    [SerializeField] bool loop = false;
    [SerializeField] bool playOnAwake = false;

    private bool playing = false;
    private bool scheduled = false;
    private double scheduledTime;

    private void Awake()
    {
        if (playOnAwake) playing = true;
        else audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if(scheduled && scheduledTime < AudioSettings.dspTime)
        {
            playing = true;
            scheduled = false;
        }

        if (playing && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Count)]);
            if (!loop) playing = false;
        }
    }

    public void Play()
    {
        playing = true;
    }

    public void Stop()
    {
        playing = false;
        audioSource.Stop();
    }

    public void PlayScheduled(double time)
    {
        scheduled = true;
        scheduledTime = time;
    }
}
