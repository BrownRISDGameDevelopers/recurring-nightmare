using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEnvCollision : MonoBehaviour
{
    public GameObject enemy;
    private Rigidbody2D _enemyRigidBody;
    private Transform _enemyTransform;
    private Vector3 speed = new Vector3(5, 0, 0);

    void OnCollisionEnter2D(Collision2D collision){
        // Vector2 direction = collision.GetContact(0).normal;
        // if(direction.x == 1 || direction.x == -1) {
        //     _enemyRigidBody.velocity = new Vector2(-_enemyRigidBody.velocity.x, _enemyRigidBody.velocity.y);
        // } 
        // if(direction.y == 1 || direction.y == -1) {
        //     _enemyRigidBody.velocity = new Vector2(_enemyRigidBody.velocity.x, -_enemyRigidBody.velocity.y);
        // }
        // _enemyRigidBody.velocity = new Vector2(-_enemyRigidBody.velocity.x, _enemyRigidBody.velocity.y);
        speed.x = -speed.x;
    }
    
    void Start()
    {

    }

    void Awake()
    {
        _enemyRigidBody = enemy.GetComponent<Rigidbody2D>();
        _enemyTransform = enemy.GetComponent<Transform>();
    }

    void Update()
    {
        _enemyTransform.position += speed * Time.deltaTime;
    }
}
