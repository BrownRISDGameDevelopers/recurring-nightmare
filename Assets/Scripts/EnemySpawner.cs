using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameHandler _gameHandler;
    [SerializeField] private Transform _playerTransform;
    // The Enemy prefab that should be spawned.
    [SerializeField] private GameObject _enemy;
    // How many seconds between spawns.
    [SerializeField] private float _spawnTime = 10f;
    [SerializeField] private bool _spawnOnStart = false;
    private float _timeToNextSpawn;
    // How many Enemies can be spawned at this point at a time.
    [SerializeField] private int _maxEnemies = 1;

    void Start()
    {
        if (_spawnOnStart) SpawnEnemy();
        else _timeToNextSpawn = _spawnTime;
    }
    
    void Update()
    {
        // If there are less Enemies than the max amount, spawn a new one.
        if (transform.childCount < _maxEnemies)
        {
            _timeToNextSpawn -= Time.deltaTime;
            if (_timeToNextSpawn <= 0)
            {
                SpawnEnemy();
            }
        }
    }
    
    private void SpawnEnemy()
    {
        Debug.Log("Enemy has been spawned.");
        // Make the Enemy as child of this EnemySpawner.
        GameObject enemy = Instantiate(_enemy);
        enemy.transform.position = transform.position;
        enemy.transform.parent = transform;
        enemy.GetComponent<EnemyMovement>().InitializeEnemy(_playerTransform, _gameHandler);
        enemy.SetActive(true);
        _timeToNextSpawn = _spawnTime;
    }
}
