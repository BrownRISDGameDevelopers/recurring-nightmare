using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : GroundDetectionEntity
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float collisionTimeThreshold;
    // [SerializeField] private float speed = 0.05f;
    
    private Rigidbody2D _enemyBody;
    private bool _isOnGround;

    protected override void Awake() 
    {
        _enemyBody = GetComponent<Rigidbody2D>();
        base.Awake();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
        ClampVelocity();

        (_isOnGround, _) = CheckOnGround();
        if (_isOnGround) AvoidObstacle();
    }

    private void FollowPlayer() 
    {
        float delX = (playerTransform.position.x - transform.position.x);
        Vector2 movementDirection = Mathf.Sign(delX) * Vector2.right;
        _enemyBody.AddForce(movementDirection * force, ForceMode2D.Impulse);
    }

    private void ClampVelocity()
    {
        Vector3 v = _enemyBody.velocity;
        v.x = Mathf.Clamp(v.x, -maxVelocity, maxVelocity);
        _enemyBody.velocity = v;
    }

    private void Jump()
    {
        _enemyBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void AvoidObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _enemyBody.velocity.normalized);
        if (!hit) return;
        
        // Handle the case where the enemy is stuck at the obstacle.
        // Add some horizontal momentum after jumping.
        if (Mathf.Abs(_enemyBody.velocity.x) < 0.12f && hit.distance < 1.0f)
        {
            Jump();
            _enemyBody.AddForce(Vector2.right * (Mathf.Sign(_enemyBody.velocity.x) * jumpForce));
        }
        // If enemy is moving, jump when it expects to hit a obstacle shortly.
        else
        {
            float expectedCollisionTime = hit.distance / Mathf.Abs(_enemyBody.velocity.x);
            if (expectedCollisionTime < collisionTimeThreshold) 
            {
                Jump();
            }
        }
    }
}
