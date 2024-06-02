using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public enum SpawnTypes
    {
        All,
        Interval,
        Indefinite,
    }
    [SerializeField] SpawnTypes spawnType = SpawnTypes.All;

    [Header("General Settings")]
    [SerializeField][Tooltip("How far from the center of the spawner enemies will poop out")] float spawnRadius = 5f;
    [SerializeField][Tooltip("How long after Activated until enemies are spawned. 0 is no delay")] float spawnDelay = 0f;

    [Header("Spawn Settings")]
    [SerializeField][Tooltip("How many enemies to spawn, OR how many to pause at for indefinite")] int maxEnemies = 5;
    [HideInInspector][SerializeField][Tooltip("How long after a spawn until the next")] float intervalAmount = 3f; 
    
    float popDuration = 0.25f;

    void Start()
    {
        if(activateType == ActivateTypes.Start)
        {
            Activate();
        }
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
            spawnedEnemies.RemoveAll(spawnedEnemy => !spawnedEnemy.activeSelf);

            if (spawnedEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(intervalAmount);
        }
    }

    public GameObject SpawnEnemy()
    {
        if (possibleEnemies.Count == 0)
        {
            Debug.LogError(this.gameObject + " has no possibleEnemies");
        }

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
                if (enemySpawn.prefab != null)
                {
                    GameObject spawnedEnemy = Instantiate(enemySpawn.prefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-spawnRadius, spawnRadius), transform.position.y, transform.position.z + UnityEngine.Random.Range(-spawnRadius, spawnRadius)), Quaternion.identity);
                    spawnedEnemies.Add(spawnedEnemy);

                    Coroutine popCoroutine = StartCoroutine(PopEnemy(spawnedEnemy));

                    if(spawnedEnemies.Count >= maxEnemies && spawnType != SpawnTypes.Indefinite)
                    {
                        StartCoroutine(DestroyAfterPopping(popCoroutine));
                    }

                    return spawnedEnemy;
                }
                else
                {
                    Debug.LogError("Enemy spawn " + enemySpawn + " has no assigned prefab (" + this.gameObject + ")");

                    #if UNITY_EDITOR
                    EditorGUIUtility.PingObject(this.gameObject);
                    #endif

                    return null;
                }
            }
            else
            {
                randomValue -= enemySpawn.weight;
            }
        }
        Debug.LogError(this.gameObject + " had a total weight of " + totalWeight + " but couldn't find an enemy prefab. Returning null.");
        return null;
    }

    IEnumerator PopEnemy(GameObject enemyToPop)
    {
        Vector3 originalEnemyScale = enemyToPop.transform.localScale;
        enemyToPop.transform.localScale = Vector3.zero;     

        float popCounter = 0f;
        while (popCounter < popDuration)
        {
            float t = DylanTree.EaseOutQuart(popCounter / popDuration);

            popCounter += Time.deltaTime;

            enemyToPop.transform.localScale = Vector3.Lerp(Vector3.zero, originalEnemyScale, t);

            yield return null;
        }

        enemyToPop.transform.localScale = originalEnemyScale;
    }

    IEnumerator DestroyAfterPopping(Coroutine popCoroutine)
    {
        yield return popCoroutine;

        Destroy(this.gameObject);
    }

    [System.Serializable]
    class EnemySpawn
    {
        [SerializeField]public GameObject prefab;
        [SerializeField][Min(0f)] public float weight;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    SerializedProperty spawnerType;

    SerializedProperty intervalAmount;

    void OnEnable()
    {
        spawnerType = serializedObject.FindProperty("spawnType");
        intervalAmount = serializedObject.FindProperty("intervalAmount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        if (spawnerType.enumValueIndex == (int) EnemySpawner.SpawnTypes.Interval || spawnerType.enumValueIndex == (int) EnemySpawner.SpawnTypes.Indefinite)
        {
            EditorGUILayout.PropertyField(intervalAmount);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif