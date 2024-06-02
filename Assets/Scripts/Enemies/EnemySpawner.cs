using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]List<EnemySpawn> possibleEnemies = new List<EnemySpawn>();
    List<GameObject> spawnedEnemies = new List<GameObject>();
    enum ActivateTypes
    {
        Start,
        TriggerEnter,
        None,
    }
    [SerializeField] ActivateTypes activateType = ActivateTypes.Start;
    enum SpawnTypes
    {
        All,
        Interval,
        Indefinite,
    }

    [SerializeField] float spawnRange = 5f;
    [SerializeField] SpawnTypes spawnType = SpawnTypes.All;
    [SerializeField] int maxEnemies = 5;
    [SerializeField] float spawnDelay = 0f;
    [SerializeField][Tooltip("Only used with Interval and Indefinite ActivateTypes")] float intervalAmount = 3f; 
    public void Activate()
    {
        StartCoroutine(ActivateCoroutine());
    }
    IEnumerator ActivateCoroutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        switch (spawnType)
        {
            case SpawnTypes.All:
                SpawnAllEnemies();
                break;
            case SpawnTypes.Interval:
                StartCoroutine(SpawnEnemiesAtInterval());
                break;
            case SpawnTypes.Indefinite:
                StartCoroutine(SpawnEnemiesIndefinitely());
                break;
        }
    }

    void SpawnAllEnemies()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    IEnumerator SpawnEnemiesAtInterval()
    {
        for (int i = 0; i < maxEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(intervalAmount);
        }
    }

    IEnumerator SpawnEnemiesIndefinitely()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalAmount);

            List<GameObject> enemiesToRemove = new List<GameObject>();
            foreach (GameObject spawnedEnemy in spawnedEnemies)
            {
                if (!spawnedEnemy.activeSelf)
                {
                    enemiesToRemove.Add(spawnedEnemy);
                }
            }
            foreach (GameObject enemyToRemove in enemiesToRemove)
            {
                spawnedEnemies.Remove(enemyToRemove);
            }


            if (spawnedEnemies.Count < maxEnemies)
            {
               SpawnEnemy();
                //StartCoroutine(PopEnemy(spawnedEnemy));
            }
        }
    }

    GameObject SpawnEnemy()
    {
        if (possibleEnemies.Count == 0) return null;

        float totalWeight = 0f;
        foreach (EnemySpawn enemySpawn in possibleEnemies)
        {
            totalWeight += enemySpawn.weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);

        foreach (EnemySpawn enemySpawn in possibleEnemies)
        {
            if (randomValue < enemySpawn.weight)
            {
                GameObject spawnedEnemy = Instantiate(enemySpawn.prefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-spawnRange, spawnRange), transform.position.y, transform.position.z + UnityEngine.Random.Range(-spawnRange, spawnRange)), Quaternion.identity);
                spawnedEnemies.Add((spawnedEnemy));
                if(spawnedEnemies.Count >= maxEnemies && spawnType != SpawnTypes.Indefinite)
                {
                    Destroy(gameObject);
                }
                return spawnedEnemy;
            }
            randomValue -= enemySpawn.weight;
        }
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "currentPlayer")
        {
            if(activateType == ActivateTypes.TriggerEnter)
            {
                Activate();
            }
        }
    
    }
    void Start()
    {
        if(activateType == ActivateTypes.Start)
        {
            Activate();
        }
    }
    [System.Serializable]
    class EnemySpawn
    {
        [SerializeField]public GameObject prefab;
        [SerializeField][Min(0f)] public float weight;
    }

}
