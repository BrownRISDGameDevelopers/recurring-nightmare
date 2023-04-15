using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameHandler _gameHandler;
    [SerializeField] private Transform _playerTransform;
    // The Enemy prefab that should be spawned.
    // [SerializeField] private GameObject _enemy;
    // How many seconds between spawns.
    [SerializeField] private float _spawnTime = 10f;
    [SerializeField] private bool _spawnOnStart = false;
    private float _timeToNextSpawn;
    // How many Enemies can be spawned at this point at a time.
    [SerializeField] private int _maxEnemies = 1;
    [SerializeField] private List<Vector3> enemy_positions;
    [SerializeField] private List<int> enemy_ID;
    [SerializeField] private List<GameObject> enemyPrefabList;
    [SerializeField] private bool hasSpawned = false;

    void Start()
    {
        // if (_spawnOnStart) SpawnEnemy();
        // else _timeToNextSpawn = _spawnTime;
    }
    
    void Update()
    {
        // If there are less Enemies than the max amount, spawn a new one.
        if (transform.childCount < _maxEnemies)
        {
            _timeToNextSpawn -= Time.deltaTime;
            if (_timeToNextSpawn <= 0)
            {
                // SpawnEnemy();
            }
        }
    }
    
    private void SpawnOneEnemy(int index)
    {
        Debug.Log("Enemy has been spawned.");
        // Make the Enemy as child of this EnemySpawner.
        int enemy_id = enemy_ID[index];
        GameObject enemy = Instantiate(enemyPrefabList[enemy_id]);
        enemy.transform.position = enemy_positions[index];
        enemy.transform.parent = transform;
        enemy.GetComponent<EnemyMovement>().InitializeEnemy(_playerTransform, _gameHandler);
        enemy.SetActive(true);
        _timeToNextSpawn = _spawnTime;
    }

    private void SpawnEnemies()
    {
        Debug.Log("spawner activated looping through enemy list");
        for (int i = 0; i < enemy_ID.Count; i++){
            SpawnOneEnemy(i);
        } 
        hasSpawned = true;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy spawner collision detected");
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.SendMessage("ApplyDamage", 10);
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        Debug.Log("Enemy spawner collision detected");
        if (!hasSpawned)
        {
            SpawnEnemies();
        }
        
    }

    
}
