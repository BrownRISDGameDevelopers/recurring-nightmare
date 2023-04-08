using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    // The HealthPack prefab that should be spawned.
    [SerializeField] private GameObject _healthPack;
    // How many seconds between HealthPack spawns.
    [SerializeField] private float _spawnTime = 10f;
    private float _timeToNextSpawn;
    // How many HealthPacks can be on the map at a time in this spawn location.
    // We might just want to have this always be 1.
    [SerializeField] private int _maxHealthPacks = 1;
    
    void Start()
    {
        SpawnHealthPack();
    }
    
    void Update()
    {
        // If there are less HealthPacks than the max amount, spawn a new one.
        if (transform.childCount < _maxHealthPacks)
        {
            _timeToNextSpawn -= Time.deltaTime;
            if (_timeToNextSpawn <= 0)
            {
                SpawnHealthPack();
            }
        }
    }
    
    private void SpawnHealthPack()
    {
        Debug.Log("Health pack has been spawned.");
        // Make the HealthPack as child of this HealthPackSpawner.
        GameObject healthPack = Instantiate(_healthPack);
        healthPack.transform.position = transform.position;
        healthPack.transform.parent = transform;
        _timeToNextSpawn = _spawnTime;
    }
}