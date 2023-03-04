using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D enemyBody;

    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float collisionTimeThreshold;
    // [SerializeField] private float speed = 0.05f;


    private bool isColliding = false;
    public float movementBond = 15;

    private bool isInAir;
    private bool isMoving;

    private void Start() 
    {
        enemyBody = GetComponent<Rigidbody2D>();
        isMoving = true;
    }

    private void Update() 
    {
        
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        ClampVelocity();
        if (!isInAir)
        {
            AvoidObstacle();
        }
    }

    private void FollowPlayer() 
    {
        float delX = (playerTransform.position.x - enemyTransform.position.x);
        Vector2 movementDirection = Mathf.Sign(delX) * Vector2.right;
        enemyBody.AddForce(movementDirection * force, ForceMode2D.Impulse);
    }

    private void ClampVelocity()
    {
        Vector3 v = enemyBody.velocity;
        v.x = Mathf.Clamp(v.x, -maxVelocity, maxVelocity);
        enemyBody.velocity = v;
    }

    private void Jump()
    {
        isInAir = true;
        enemyBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Debug.Log("JUMP!");
    }

    private void AvoidObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            enemyTransform.position, enemyBody.velocity.normalized);

        // avoid potential obstacle
        if (hit.collider != null)
        {
            // Handle the case where the enemy is stuck at the obstacle.
            // Add some horizontal momentum after jumping.
            if (Mathf.Abs(enemyBody.velocity.x) < 0.12f && hit.distance < 1.0f)
            {
                Jump();
                enemyBody.AddForce(Vector2.right * Mathf.Sign(enemyBody.velocity.x) * jumpForce);
            }
            // If enemy is moving, jump when it expects to hit a obstacle shortly.
            else
            {
                float expectedCollisionTime = hit.distance / Mathf.Abs(enemyBody.velocity.x);
                if (expectedCollisionTime < collisionTimeThreshold) 
                {
                    Jump();
                }
            }
        }

        // Debug.Log(enemyBody.velocity);
        // Debug.Log("collider:");
        // Debug.Log(hit.collider);
        // Debug.Log("distance:");
        // Debug.Log(hit.distance);
        // Debug.Log("point:");
        // Debug.Log(hit.point);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Ground")
        {
            isInAir = false;
        }
        Debug.Log(collision.collider.name);
        isMoving = false;
    }

 
}
