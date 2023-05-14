using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class EnemyMovement : GroundDetectionEntity
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxVelocity;
    [SerializeField] private float maxIdleVelocity;
    [SerializeField] private float collisionTimeThreshold;
    // [SerializeField] private float speed = 0.05f;
    [FormerlySerializedAs("damage")] [SerializeField] private float contactDamage = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSwitcher idleAudioSwitcher;
    [SerializeField] private AudioSource alertedAudioSource;
    [SerializeField] private AudioSource agitatedAudioSource;
    
    private Rigidbody2D _enemyBody;
    private bool _isOnGround;
    private Transform _playerTransform;

    private bool _isFollowing = false;
    private Vector2 _movingDirection = Vector2.right;

    protected override void Awake() 
    {
        _enemyBody = GetComponent<Rigidbody2D>();
        _playerTransform = GameManager.Player.transform;
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (GameManager.GameState != GameManager.RunningState.Running) return;

        // (_isOnGround, _) = CheckOnGround();

        Collider2D enemyGround = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, GroundMask).collider;
        Collider2D playerGround = Physics2D.Raycast(_playerTransform.position, Vector2.down, Mathf.Infinity, GroundMask).collider;
        RaycastHit2D playerRaycast = Physics2D.Raycast(transform.position, (_playerTransform.position - transform.position).normalized, Mathf.Infinity, GroundMask);
        RaycastHit2D horizontalGroundeRaycast;
        RaycastHit2D verticalRaycast = Physics2D.Raycast(transform.position + new Vector3(_movingDirection.x * boxCollider.size.x / 2, 0, 0), Vector2.down, Mathf.Infinity, GroundMask);

        if (!_isFollowing
            && Mathf.Sign((_playerTransform.position - transform.position).x) * Mathf.Sign(_movingDirection.x) > 0  // Same direction
            && enemyGround == playerGround && Mathf.Abs(_playerTransform.position.y - transform.position.y) < 1  // Same platform
            && (playerRaycast.collider == null || playerRaycast.distance > Vector3.Distance(_playerTransform.position, transform.position))  // No obstacle
            && (verticalRaycast.collider != null && verticalRaycast.distance <= boxCollider.size.y / 2 + 0.1))
        {
            FollowPlayer();
        }
        else if (_isFollowing)
        {
            if ((playerRaycast.collider != null && playerRaycast.distance < Vector3.Distance(_playerTransform.position, transform.position))  // Obstacle
                || (verticalRaycast.collider == null || verticalRaycast.distance > boxCollider.size.y / 2 + 0.1)  // Fall
                )
            {
                if (_isFollowing)
                {
                    agitatedAudioSource.Stop();
                    idleAudioSwitcher.Play();
                }
                _isFollowing = false;
            }
            else FollowPlayer();
        }
        else
        {
            horizontalGroundeRaycast = Physics2D.Raycast(transform.position, _movingDirection, Mathf.Infinity, GroundMask);
            if ((horizontalGroundeRaycast.collider != null && horizontalGroundeRaycast.distance < boxCollider.size.y / 2 + 0.1) 
                || (verticalRaycast.collider == null || verticalRaycast.distance > boxCollider.size.y / 2 + 0.1))
            {
                _movingDirection = -_movingDirection;
                _enemyBody.velocity = Vector2.zero;
            }
            _enemyBody.AddForce(_movingDirection * force, ForceMode2D.Impulse);
            if (_isFollowing)
            {
                agitatedAudioSource.Stop();
                idleAudioSwitcher.Play();
            }
            _isFollowing = false;
        }

        ClampVelocity();
        // if (_isOnGround) AvoidObstacle();
    }

    private void FollowPlayer()
    {
        if(!_isFollowing)
        {
            idleAudioSwitcher.Stop();
            alertedAudioSource.PlayOneShot(alertedAudioSource.clip);
            agitatedAudioSource.PlayScheduled(AudioSettings.dspTime + 1);
        }
        
        float delX = _playerTransform.position.x - transform.position.x;
        _movingDirection = Mathf.Sign(delX) * Vector2.right;
        _enemyBody.AddForce(_movingDirection * force, ForceMode2D.Impulse);
        _isFollowing = true;
    }

    private void ClampVelocity()
    {
        Vector3 v = _enemyBody.velocity;
        if(_isFollowing) v.x = Mathf.Clamp(v.x, -maxVelocity, maxVelocity);
        else v.x = Mathf.Clamp(v.x, -maxIdleVelocity, maxIdleVelocity);
        _enemyBody.velocity = v;
    }

    private void Jump()
    {
        _enemyBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void AvoidObstacle()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _enemyBody.velocity.normalized);
        if (!hit || hit.transform.CompareTag("Player")) return;


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
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(contactDamage, true);
        }
    }
}
