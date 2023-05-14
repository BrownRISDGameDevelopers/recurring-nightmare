using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioClip daytimeAudioClip;
    [SerializeField] private AudioClip nightmareAudioClip;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void PlayDaytimeMusic()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = daytimeAudioClip;
        audioSource.Play();
    }

    public void PlayNightmareMusic()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = nightmareAudioClip;
        audioSource.Play();
    }
}
