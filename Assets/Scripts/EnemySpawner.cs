using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemySpawner : MonoBehaviour
{
    // The Enemy prefab that should be spawned.
    // [SerializeField] private GameObject _enemy;
    // How many seconds between spawns.
    [FormerlySerializedAs("_spawnTime")] [SerializeField] private float spawnTime = 10f;
    private float _timeToNextSpawn;
    // How many Enemies can be spawned at this point at a time.
    [FormerlySerializedAs("_maxEnemies")] [SerializeField] private int maxEnemies = 1;
    [FormerlySerializedAs("enemy_positions")] [SerializeField] private List<Vector3> enemyPositions;
    [FormerlySerializedAs("enemy_ID")] [SerializeField] private List<int> enemyID;
    [SerializeField] private List<GameObject> enemyPrefabList;
    [SerializeField] private bool hasSpawned = false;

    void Update()
    {
        // If there are less Enemies than the max amount, spawn a new one.
        if (transform.childCount < maxEnemies)
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
        int enemyID = this.enemyID[index];
        GameObject enemy = Instantiate(enemyPrefabList[enemyID], transform, true);
        enemy.transform.position = enemyPositions[index];
        enemy.SetActive(true);
        _timeToNextSpawn = spawnTime;
    }

    private void SpawnEnemies()
    {
        Debug.Log("spawner activated looping through enemy list");
        for (int i = 0; i < enemyID.Count; i++){
            SpawnOneEnemy(i);
        } 
        hasSpawned = true;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy spawner collision detected");
        if (collision.gameObject.CompareTag("Enemy"))
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
