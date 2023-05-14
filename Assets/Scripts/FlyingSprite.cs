using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool flip = false;

    [SerializeField] private NavMeshAgent agent;

    bool flipX = false;

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.x < 0) spriteRenderer.flipX = flipX = true;
        else if (agent.velocity.x == 0) spriteRenderer.flipX = flipX;
        else spriteRenderer.flipX = flipX = false;
        if (flip) spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}
