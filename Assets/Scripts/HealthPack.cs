using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float _healAmount = 5f;
    
    void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            // I thought this would ignore the physics collision so there wouldn't be
            // a stutter when the player picked up the HealthPack, but it only ignores future collisions.
            // Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            collision.gameObject.GetComponent<PlayerHealth>().HealPlayer(_healAmount);
            // Destroy the HealPack after the player gets it.
            Destroy(gameObject);
        }
    }
    
    /*
    // To make the player not collide physically with the HealthPack,
    // use this function instead of OnCollisionStay2D. Put the script instead on a child with a
    // BoxCollider2D set to trigger. Then set the parent's layer to one that doesn't collide with the player.
    void OnTriggerEnter2D(Collider2D collided)
    {
        Debug.Log("Collided with HealthPack!");
        if (collided.gameObject.tag == "Player")
        {
            collided.gameObject.GetComponent<PlayerHealth>().HealPlayer(_healAmount);
            // Destroy the HealPack after the player gets it.
            //Destroy(gameObject);
            Destroy(transform.parent.gameObject);
        }
    }
    */
}
