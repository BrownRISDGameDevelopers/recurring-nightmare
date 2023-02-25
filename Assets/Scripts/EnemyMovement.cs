using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float speed = 0.05f;
    private bool isColliding = false;
    public float movementBond = 15;

    private bool isMoving;

    private void Start() 
    {
        isMoving = true;
    }

    private void Update() 
    {
        
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer() 
    {
        Vector3 movementDirection = (playerTransform.position - enemyTransform.position);
        movementDirection.y = 0;
        if (Math.Abs(movementDirection.x) < 0.5 * enemyTransform.localScale.x + 0.5 * playerTransform.localScale.x + 0.1) {
            movementDirection.x = 0;
        }
        else
        {
            movementDirection.Normalize();
        }
        enemyTransform.position += speed * movementDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        isMoving = false;
    }

 
}
