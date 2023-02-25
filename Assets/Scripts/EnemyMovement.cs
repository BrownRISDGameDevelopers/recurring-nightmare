using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform enemyTransform;

    private bool isMoving;

    private void Start() 
    {
        isMoving = true;
    }

    private void Update() 
    {
        if (isMoving) 
        {
            enemyTransform.position += Vector3.left * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.collider.name);
        isMoving = false;
    }

 
}
