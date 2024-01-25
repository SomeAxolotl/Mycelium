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

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (enemiesSpawnedCount < enemiesSpawnedLimit)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
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
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

}
