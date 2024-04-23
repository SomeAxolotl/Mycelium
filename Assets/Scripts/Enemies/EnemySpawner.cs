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
    [SerializeField][Tooltip("The number of enemies spawned before this spawner destructs")] private int enemiesSpawnedLimit = 20;
    [SerializeField][Tooltip("Vertical Spawn Range")] private float waterfallOffset = 2;
    [SerializeField][Tooltip("Spawn enemies vertically")] private bool spawnerIsVertical = false;
    [SerializeField][Tooltip("Delay the spawner's start time by the spawn interval")] private bool spawnDelay = false;
    private int enemiesSpawnedCount = 0;
    [HideInInspector] public List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField]private GameObject particleEffect;
    [SerializeField][Tooltip("Spawns the enemiesSpawnedLimit at once")] private bool spawnAll = false;
    private bool hasSpawned = false;
    [SerializeField][Tooltip("Spawns enemies on Start()")] private bool spawnOnStart = false;
    private int totalEnemyWeight = 0;

    [SerializeField] private bool spawnIndefinitely = false;
    [SerializeField] private int maxSpawnCount = 5;
    [SerializeField] private float indefiniteSpawnInterval = 4f;

    [SerializeField] private bool spawnWithInterval = false;

    private List<GameObject> weightedEnemyList = new List<GameObject>();
    

    void Start()
    {
        if (spawnIndefinitely)
        {
            StartCoroutine(SpawnEnemiesIndefinitely());
        }

        //Fills a list of potential enemy spawns with amounts based on their weight
        foreach (EnemySpawn enemySpawn in EnemyList)
        {
            totalEnemyWeight += enemySpawn.weight;
            for (int i = 0; i < enemySpawn.weight; i++)
                weightedEnemyList.Add(enemySpawn.EnemyPrefab);
        }

        if (!hasSpawned && spawnOnStart)
        {
            if (spawnAll)
            {
                SpawnAllEnemies();
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(SpawnEnemiesWithInterval());
            }
        }
    }

    IEnumerator SpawnEnemiesIndefinitely()
    {
        while (true)
        {
            yield return new WaitForSeconds(indefiniteSpawnInterval);

            int spawnerChildCount = transform.childCount;
            if (spawnerChildCount < maxSpawnCount + 2)
            {
                GameObject a = Instantiate(weightedEnemyList[UnityEngine.Random.Range(0, weightedEnemyList.Count - 1)], new Vector3(transform.position.x + UnityEngine.Random.Range(-spawnRange, spawnRange), transform.position.y, transform.position.z + UnityEngine.Random.Range(-spawnRange, spawnRange)), Quaternion.identity, transform);
                yield return null;
                a.transform.parent = this.transform;
            }
        }
    }

    IEnumerator SpawnEnemiesWithInterval()
    {
        while (enemiesSpawnedCount < enemiesSpawnedLimit)
        {
            if (spawnDelay == true)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnEnemy();
                enemiesSpawnedCount++;
            }
            else
            {
                SpawnEnemy();
                enemiesSpawnedCount++;
                //Debug.Log("Spawn count: " + enemiesSpawnedCount);
                yield return new WaitForSeconds(spawnInterval);
            }

        }

        hasSpawned = true;
        Destroy(gameObject);
    }
    IEnumerator SpawnAllWithInterval()
    {
        yield return new WaitForSeconds(spawnInterval);
        SpawnAllEnemies();
        Destroy(gameObject);
    }

    void SpawnEnemy()
    {
        if (spawnerIsVertical == true)
        {
            GameObject newEnemy = Instantiate(weightedEnemyList[UnityEngine.Random.Range(0, weightedEnemyList.Count - 1)], new Vector3(transform.position.x + waterfallOffset, transform.position.y, transform.position.z), Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
        }
        else
        {
            GameObject newEnemy = Instantiate(weightedEnemyList[UnityEngine.Random.Range(0, weightedEnemyList.Count - 1)], new Vector3(transform.position.x + UnityEngine.Random.Range(-spawnRange, spawnRange), transform.position.y, transform.position.z + UnityEngine.Random.Range(-spawnRange, spawnRange)), Quaternion.identity);
            spawnedEnemies.Add(newEnemy);
        }
        //Selects a random enemy from the weightedEnemyList, and spawns it

    }

    void SpawnAllEnemies()
    {
        for (int i = 0; i < enemiesSpawnedLimit; i++)
        {
            SpawnEnemy();
            Destroy(gameObject);
        }
        hasSpawned = true;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject player = GameObject.FindGameObjectWithTag("currentPlayer");
        if (other.gameObject == player)
        {
            if (!hasSpawned)
            {
                if (spawnDelay && spawnAll)
                {
                    Debug.Log("Ran");
                    StartCoroutine(SpawnAllWithInterval());
                }
                else if (spawnAll && !spawnDelay)
                {
                    SpawnAllEnemies();
                }
                else if (spawnWithInterval)
                {
                    StartCoroutine(SpawnEnemiesWithInterval());
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

    [Range(1, 10)]
    [Tooltip("The spawn weight of an enemy. The higher the weight compared to other enemies, the more likely the enemy will spawn.")]
    public int weight = 1;
}