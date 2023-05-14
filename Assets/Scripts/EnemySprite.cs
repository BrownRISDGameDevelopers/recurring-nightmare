using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    [SerializeField] private Rigidbody2D enemyBody;
    [SerializeField] private SpriteRenderer spriteRenderer;

    bool flipX = false;

    // Update is called once per frame
    void Update()
    {
        if (enemyBody.velocity.x < 0) spriteRenderer.flipX = flipX = true;
        else if (enemyBody.velocity.x == 0) spriteRenderer.flipX = flipX;
        else spriteRenderer.flipX = flipX = false;
    }
}
