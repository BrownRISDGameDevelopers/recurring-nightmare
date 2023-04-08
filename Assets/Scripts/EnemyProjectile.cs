using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private GameHandler gameHandler;
    [SerializeField] private Transform targetTransform;  // Transform of the target
    [SerializeField] private float shootingInterval = 3.0f;  // Interval between each projectile
    [SerializeField] private float projectileSpeed = 100.0f;

    private float _shootingTimer = 0f;  // When the enemy should shoot

    // Update is called once per frame
    void Update()
    {
        if (gameHandler.GameState != GameHandler.RunningState.Running) return;
        
        _shootingTimer += Time.deltaTime;
        
        if (_shootingTimer > shootingInterval)
        {
            Shoot();
            _shootingTimer = 0f;
        }
    }

    private void Shoot()
    {
        // Create projectile game object
        var position = gameObject.transform.position;
        var direction = (targetTransform.position - position).normalized;
        var projectile = Instantiate(projectileObject, position, Quaternion.LookRotation(Vector3.forward, direction));
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed; //shoot the bullet
    }
}
