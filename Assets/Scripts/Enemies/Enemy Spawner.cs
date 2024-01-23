using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;
    private void Start()
    {
        // Start the coroutine for spawning enemies
        StartCoroutine(SpawnEnemies());
    }

    // Coroutine for spawning enemies at regular intervals
    IEnumerator SpawnEnemies()
    {
        // Infinite loop for continuous enemy spawning
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (GameObject.FindGameObjectsWithTag("Enemy").Length < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }
    // Method for spawning a new enemy
    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
