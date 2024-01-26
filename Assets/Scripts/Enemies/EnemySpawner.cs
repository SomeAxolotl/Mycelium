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
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    public bool spawnAll = false;
    private void Start()
    {
        if (spawnAll)
        {
            SpawnAllEnemies();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(SpawnEnemies());
        }
        
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawnedCount < enemiesSpawnedLimit)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (spawnedEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
                enemiesSpawnedCount++;

                if(enemiesSpawnedCount >= enemiesSpawnedLimit)
                {
                    Destroy(gameObject);
                }
            }
        }
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
    }

}
