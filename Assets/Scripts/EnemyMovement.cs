using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;

public class EnemyMovement : GroundDetectionEntity
{
    [SerializeField] private float force;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxVelocity;
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

    protected override void Awake() 
    {
        _enemyBody = GetComponent<Rigidbody2D>();
        _playerTransform = GameManager.Player.transform;
        base.Awake();
    }

    private void FixedUpdate()
    {
        if (GameManager.GameState != GameManager.RunningState.Running) return;
        
        FollowPlayer();
        ClampVelocity();

        (_isOnGround, _) = CheckOnGround();
        if (_isOnGround) AvoidObstacle();
    }

    private void FollowPlayer()
    {
        if ((_playerTransform.position - transform.position).magnitude > 7)
        {
            if(_isFollowing)
            {
                agitatedAudioSource.Stop();
                idleAudioSwitcher.Play();
            }
            _isFollowing = false;
            return;
        }

        if(!_isFollowing)
        {
            idleAudioSwitcher.Stop();
            alertedAudioSource.PlayOneShot(alertedAudioSource.clip);
            agitatedAudioSource.PlayScheduled(AudioSettings.dspTime + 1);
        }
        
        float delX = _playerTransform.position.x - transform.position.x;
        Vector2 movementDirection = Mathf.Sign(delX) * Vector2.right;
        _enemyBody.AddForce(movementDirection * force, ForceMode2D.Impulse);
        _isFollowing = true;
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
