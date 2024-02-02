using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditor.EditorTools;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawn> EnemyList;
    public float spawnInterval = 2f;
    [SerializeField][Tooltip("The radius around the EnemySpawner that the Enemy will randomly spawn in")][Min(0)] private float spawnRange = 0.4f;
    [SerializeField][Tooltip("Functions the same as enemiesSpawnedLimit???")] private int maxEnemies = 5;
    [SerializeField][Tooltip("The number of enemies spawned before this spawner destructs")] private int enemiesSpawnedLimit = 20;
    private int enemiesSpawnedCount = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField][Tooltip("Spawns the enemiesSpawnedLimit at once")] private bool spawnAll = false;
    private bool hasSpawned = false;
    [SerializeField][Tooltip("Spawns enemies on Start()")] private bool spawnOnStart = false;
    private int totalEnemyWeight = 0;
    
    private List<GameObject> weightedEnemyList = new List<GameObject>();
    void Start()
    {
        //Fills a list of potential enemy spawns with amounts based on their weight
        foreach(EnemySpawn enemySpawn in EnemyList)
        {
            totalEnemyWeight += enemySpawn.weight;
            for(int i=0; i<enemySpawn.weight; i++)
                weightedEnemyList.Add(enemySpawn.EnemyPrefab);
        }

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
        //Selects a random enemy from the weightedEnemyList, and spawns it
        GameObject newEnemy = Instantiate(weightedEnemyList[UnityEngine.Random.Range(0,weightedEnemyList.Count)], new Vector3(transform.position.x + UnityEngine.Random.Range(-spawnRange, spawnRange), transform.position.y, transform.position.z + UnityEngine.Random.Range(-spawnRange, spawnRange)), Quaternion.identity);
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

//Custom Class that holds all information about what enemy to spawn
[System.Serializable]
class EnemySpawn
{
    [Tooltip("The label of this EnemySpawn in the inspector.")]
    public string name = "Enemy";
    [Tooltip("The prefab of the enemy to spawn")]
    public GameObject EnemyPrefab = null;
    
    [Range(0,10)][Tooltip("The spawn weight of an enemy. The higher the weight compared to other enemies, the more likely the enemy will spawn.")]
    public int weight = 1;
} 