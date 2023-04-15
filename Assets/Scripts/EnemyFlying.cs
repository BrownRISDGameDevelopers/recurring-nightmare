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

        if (target != null)
        {
            var agentDrift = 0.0001f; // minimal
            var driftPos = target.transform.position + (Vector3)(agentDrift * Random.insideUnitCircle);
            agent.SetDestination(driftPos);
        }
        else
        {
            target = GameObject.FindGameObjectWithTag(targetTag);
        }
    }
}
