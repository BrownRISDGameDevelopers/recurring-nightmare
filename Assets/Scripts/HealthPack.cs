using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float healAmount = 5f;
    [SerializeField] private float spawnInterval = 10f;

    private bool _used = false;

    void Start()
    {
        StartCoroutine(SpawnHealthPack());
    }

    private IEnumerator SpawnHealthPack()
    {
        gameObject.SetActive(true);
        
        yield return new WaitUntil(() => _used);
        
        gameObject.SetActive(false);

        yield return new WaitForSeconds(spawnInterval);

        StartCoroutine(SpawnHealthPack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't disable the healthpack if the player is already at max health and cannot heal more
        _used = other.gameObject.CompareTag("Player") &&
                other.gameObject.GetComponent<PlayerHealth>().Heal(healAmount);
    }
}
