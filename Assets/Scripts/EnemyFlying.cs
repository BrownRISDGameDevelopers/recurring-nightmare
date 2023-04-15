using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFlying : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private GameHandler gameHandler;
    NavMeshAgent agent;
    GameObject target = null;
    bool follow = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.GameState != GameHandler.RunningState.Running) return;

        if (follow && target != null)
        {
            // Pathfinding
            agent.SetDestination(target.transform.position);
        }
        else if(target == null)
        {
            target = GameObject.FindGameObjectWithTag(targetTag);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the enemy collided with target, stop where it is
        if (collision.gameObject.tag == targetTag)
        {
            follow = false;
            agent.SetDestination(transform.position);  // Stop here
            agent.isStopped = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the enemy and the target are no longer collided, start pathfinding
        if (collision.gameObject.tag == targetTag)
        {
            follow = true;
            agent.isStopped = false;
        }
    }
}
