using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlying : MonoBehaviour
{
    Vector3 basePosition;
    NavMeshAgent agent;
    GameObject target = null;
    bool track = true;

    // Start is called before the first frame update
    void Start()
    {
        basePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameHandler.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameHandler.GameState != GameHandler.RunningState.Running) return;

        if (track && target != null)
        {
            // Pathfinding
            agent.SetDestination(target.transform.position);
        }
        else if(target == null)
        {
            target = GameHandler.Player;
        }
    }

    void startTrack()
    {
        Debug.Log("start track called");
        track = true;
    }

    void stopTrack()
    {
        Debug.Log("stop track called");
        track = false;
        agent.SetDestination(basePosition);  // Heading to base
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            track = false;
            agent.SetDestination(transform.position);  // Heading to base
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
