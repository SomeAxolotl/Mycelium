using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int maxEnemies = 5;
    public int enemiesSpawnedLimit = 20;
    private int enemiesSpawnedCount = 0;
    public float proximity = 10f;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public bool spawnAll = false;
    private bool hasSpawned = false;
    [SerializeField] private bool spawnOnStart = false;
    private void Start()
    {
       if(!hasSpawned && spawnOnStart)
        {
            if(spawnAll)
            {
                SpawnAllEnemies();
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(SpawnEnemies());
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawnedCount < enemiesSpawnedLimit)
        {
            if (spawnedEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
                enemiesSpawnedCount++;

                if(enemiesSpawnedCount >= enemiesSpawnedLimit)
                {
                    Destroy(gameObject);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        hasSpawned = true;
        Destroy(gameObject);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy= Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        spawnedEnemies.Add(newEnemy);
    }

    void SpawnAllEnemies()
    {
        for (int i = 0; i < enemiesSpawnedLimit; i++)
        {
            SpawnEnemy();
        }
        hasSpawned = true;
    }

    bool PlayerNearby()
    {
        GameObject player = GameObject.FindGameObjectWithTag("currentPlayer");

        if(player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= proximity;
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject player = GameObject.FindGameObjectWithTag("currentPlayer");
        if(other.gameObject == player)
        {
            if(!hasSpawned)
            {
                if(spawnAll)
                {
                    SpawnAllEnemies();
                }
                else
                {
                    StartCoroutine(SpawnEnemies());
                }
            }            
        }
    }   
}
