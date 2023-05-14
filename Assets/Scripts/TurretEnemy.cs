using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectileObject;
    
    [Header("Attack Behavior")]
    [SerializeField] private float shootingInterval = 1.0f; // Interval between each projectile
    [SerializeField] private float projectileSpeed = 10.0f;
    [FormerlySerializedAs("range")] [SerializeField] private float visionRange = 20f;
    [SerializeField] private float contactDamage = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSwitcher idleAudioSwitcher;
    [SerializeField] private AudioSource alertedAudioSource;
    [SerializeField] private AudioSource agitatedAudioSource;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private const int PoolSize = 10;
    private readonly List<GameObject> _projectilePool = new();

    private float _shootingTimer = 0f; // When the enemy should shoot
    private Vector2 _targetDirection;
    private Transform _targetTransform;

    bool _hasLineofSight = false;

    private void Start()
    {
        _targetTransform = GameManager.Player.transform;
        for (int i = 0; i < PoolSize; i++)
        {
            var tmp = Instantiate(projectileObject, gameObject.transform);
            tmp.SetActive(false);
            _projectilePool.Add(tmp);
        }
    }

    private bool HasLineOfSight()
    {
        var position = gameObject.transform.position;
        _targetDirection = (_targetTransform.position - position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(position, _targetDirection);

        if (hit.collider == null || !hit.collider.CompareTag("Player"))
        {
            return false;
        }

        return Vector2.Distance(hit.point, position) < visionRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameState != GameManager.RunningState.Running) return;

        bool previousHasLineOfSight = _hasLineofSight;
        _hasLineofSight = HasLineOfSight();
        if(previousHasLineOfSight && !_hasLineofSight)
        {
            animator.SetBool("isAttacking", false);
            agitatedAudioSource.Stop();
            idleAudioSwitcher.Play();
        }
        else if(!previousHasLineOfSight && _hasLineofSight)
        {
            animator.SetBool("isAttacking", true);
            idleAudioSwitcher.Stop();
            alertedAudioSource.PlayOneShot(alertedAudioSource.clip);
            agitatedAudioSource.PlayScheduled(AudioSettings.dspTime + 1);
        }

        _shootingTimer += Time.deltaTime;
        if (_shootingTimer < shootingInterval || !_hasLineofSight) return;

        Shoot();
        _shootingTimer = 0f;
    }

    private void Shoot()
    {
        // Create projectile game object
        var projectile = _projectilePool.Find(projectile => !projectile.activeInHierarchy);
        var turretPosition = transform.position;
        
        projectile.transform.position = turretPosition;
        projectile.transform.right = _targetTransform.position - turretPosition;
        projectile.SetActive(true);
        projectile.GetComponent<Rigidbody2D>().velocity = _targetDirection * projectileSpeed;
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(contactDamage, true);
        }
    }
}