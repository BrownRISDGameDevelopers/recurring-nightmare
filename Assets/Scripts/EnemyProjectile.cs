using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public GameObject Projectile;

    [SerializeField] private Transform enemyTransform;
    [SerializeField] private Transform targetTransform;  // Transform of the target
    [SerializeField] private float shootingSpeed = 3.0f;  // Interval between each projectile
    [SerializeField] private float shootingPower = 100.0f;

    private float shootingTime;  // When the enemy should shoot

    // Start is called before the first frame update
    void Start()
    {
        shootingTime = Time.time + shootingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        // Get shooting time
        if(Time.time > shootingTime)
        {
            shootingTime = Time.time + shootingSpeed;
            // Create projectile game object
            Vector2 pos = new Vector2(enemyTransform.position.x, enemyTransform.position.y);
            GameObject projectile = Instantiate(Projectile, pos, Quaternion.identity);
            // Get the moving direction of the projectile
            Vector2 direction = ((Vector2)targetTransform.position - pos).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * shootingPower; //shoot the bullet
        }
    }
}
