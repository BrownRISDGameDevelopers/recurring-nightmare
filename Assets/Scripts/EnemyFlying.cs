using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlying : MonoBehaviour
{
    Vector3 basePosition;
    NavMeshAgent agent;
    GameObject target = null;
    bool track = false;

    [SerializeField] private float contactDamage = 2f;

    [SerializeField] AudioSwitcher idleAudioSwitcher;
    [SerializeField] AudioSwitcher agitatedAudioSwitcher;
    [SerializeField] AudioSource alertedAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        basePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameManager.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState != GameManager.RunningState.Running) return;

        // Pathfinding
        if (track && target != null)
        {
            // Pathfinding
            agent.SetDestination(target.transform.position);
        }
        else if(target == null)
        {
            target = GameManager.Player;
        }
    }

    void startTrack()
    {
        // Debug.Log("start track called");
        track = true;
        idleAudioSwitcher.Stop();
        alertedAudioSource.PlayOneShot(alertedAudioSource.clip);
        agitatedAudioSwitcher.PlayScheduled(AudioSettings.dspTime + 1);
    }

    void stopTrack()
    {
        // Debug.Log("stop track called");
        track = false;
        agent.SetDestination(basePosition);  // Heading to base
        agitatedAudioSwitcher.Stop();
        idleAudioSwitcher.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            track = false;
            collision.gameObject.GetComponent<PlayerHealth>().Damage(contactDamage, true);
            agent.SetDestination(transform.position);  // Stop
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            track = true;
        }
    }
}
