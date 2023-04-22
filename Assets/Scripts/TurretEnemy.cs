using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretEnemy : MonoBehaviour
{
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private Transform targetTransform; // Transform of the target
    [SerializeField] private float shootingInterval = 1.0f; // Interval between each projectile
    [SerializeField] private float projectileSpeed = 10.0f;
    [SerializeField] private float range = 20f;

    private const int PoolSize = 10;
    private readonly List<GameObject> _projectilePool = new List<GameObject>();

    private float _shootingTimer = 0f; // When the enemy should shoot
    private Vector2 _targetDirection;

    private void Start()
    {
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
        _targetDirection = (targetTransform.position - position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(position, _targetDirection);

        if (hit.collider == null || !hit.collider.CompareTag("Player"))
        {
            return false;
        }

        return Vector2.Distance(hit.point, position) < range;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.GameState != GameHandler.RunningState.Running) return;

        _shootingTimer += Time.deltaTime;
        if (_shootingTimer < shootingInterval || !HasLineOfSight()) return;

        Shoot();
        _shootingTimer = 0f;
    }

    private void Shoot()
    {
        // Create projectile game object
        var projectile = _projectilePool.Find(projectile => !projectile.activeInHierarchy);
        var turretPosition = transform.position;
        
        projectile.transform.position = turretPosition;
        projectile.transform.right = targetTransform.position - turretPosition;
        projectile.SetActive(true);
        projectile.GetComponent<Rigidbody2D>().velocity = _targetDirection * projectileSpeed;
    }
}