using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float damage = 1f;

    private float _timer = 0f;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > lifetime)
        {
            DisableSelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("hit");
            other.gameObject.GetComponent<PlayerHealth>().Damage(damage);
            DisableSelf();
        }
    }

    private void DisableSelf()
    {
        _timer = 0;
        gameObject.SetActive(false);
    }
}
