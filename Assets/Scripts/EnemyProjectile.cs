using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectileObject;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform targetTransform;  // Transform of the target
    [SerializeField] private float shootingInterval = 3.0f;  // Interval between each projectile
    [SerializeField] private float projectileSpeed = 100.0f;

    private float _shootingTimer = 0f;  // When the enemy should shoot
    
    // Update is called once per frame
    void Update()
    {
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
        var position = enemyTransform.position;
        var projectile = Instantiate(projectileObject, position, Quaternion.identity);
        var direction = (targetTransform.position - position).normalized;
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed; //shoot the bullet
    }
}
