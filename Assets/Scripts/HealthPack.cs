using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthPack : MonoBehaviour
{
    [SerializeField] private float healAmount = 5f;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private Sprite[] sprites = new Sprite[3];

    private bool _used = false;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    void Start()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _collider = gameObject.GetComponent<Collider2D>();
        StartCoroutine(SpawnHealthPack());
    }

    private void Activate()
    {
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        _used = false;
    }

    private void Deactivate()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
    }

    private IEnumerator SpawnHealthPack()
    {
        Activate();
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        yield return new WaitUntil(() => _used);

        Deactivate();
        
        yield return new WaitForSeconds(spawnInterval);
        
        StartCoroutine(SpawnHealthPack());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Don't disable the healthpack if the player is already at max health and cannot heal more
        _used = other.gameObject.CompareTag("Player") &&
                other.gameObject.GetComponent<Inventory>().Store(healAmount, _spriteRenderer.sprite);
    }
}
