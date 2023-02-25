using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform enemyTransform;

    private void Update()
    {
        enemyTransform.position += Vector3.left * Time.deltaTime;
    }
}
