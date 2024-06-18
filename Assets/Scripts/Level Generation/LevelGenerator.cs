using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

[System.Serializable]
public class SkyboxProfilePair
{
    public GameObject skyboxPrefab;
    public VolumeProfile postProcessProfile;
}

public enum ChunkTheme
{
    Theme1A,
    Theme1B
}
public class LevelGenerator : MonoBehaviour
{
    /*[Header("Chunk Lists")]
    [SerializeField][Tooltip("Chunks which end branches.")] private List<GameObject> CapPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which have 2 seams. This is the majority of chunks.")] private List<GameObject> MiddleChunkPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which begin a level.")] private List<GameObject> StartChunkPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which end a level.")] private List<GameObject> GoalChunkPrefabs = new List<GameObject>();*/

    [Header("Theme 1A Lists")]
    [SerializeField] private List<GameObject> Theme1A_StartChunkPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> Theme1A_MiddleChunkPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> Theme1A_CapPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> Theme1A_GoalChunkPrefabs = new List<GameObject>();
    [SerializeField] private GameObject CheckpointAChunk;

    [Header("Theme 1B Lists")]
    [SerializeField] private List<GameObject> Theme1B_StartChunkPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> Theme1B_MiddleChunkPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> Theme1B_CapPrefabs = new List<GameObject>();
    [SerializeField] private List<GameObject> Theme1B_GoalChunkPrefabs = new List<GameObject>();
    [SerializeField] private GameObject CheckpointBChunk;

    [Header("Level Configuration")]
    [SerializeField] private int MaxNumberOfPieces;
    private int NumberOfPieces = 0;
    private int recentChunkIndex = 0;

    [SerializeField] private List<GameObject> spawnedMiddleChunks = new List<GameObject>();
    private GameObject currentMiddleChunk;
    [SerializeField] private bool allowDuplicateChunks = false;

    [Header("Theme Selection")]
    [SerializeField] private ChunkTheme themeForLevel;

    [Header("Skybox and Post-Processing Pairs")]
    public SkyboxProfilePair[] skyboxProfilePairs;
    public GameObject volumeGameObject; // Assign the GameObject with the Volume component in the Inspector

    private GameObject currentSkybox;


    void Awake()
    {
        themeForLevel = (Random.value < 0.5f) ? ChunkTheme.Theme1A : ChunkTheme.Theme1B;
        NavMesh.RemoveAllNavMeshData();

        GameObject[] tooltips = GameObject.FindGameObjectsWithTag("Tooltip");
        foreach (GameObject tooltip in tooltips)
        {
            Destroy(tooltip);
        }

        GameObject StartPiece = Instantiate(GetStartChunkPrefab(), transform);
        NumberOfPieces++;
        List<Transform> attachPoints = new List<Transform>();
        foreach (Transform child in StartPiece.transform)
        {
            //if (child.tag.CompareTo("PlayerSpawn")==0)
            //spawnPlayer(child.transform);
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }

        if (attachPoints.Count == 0)
        {
            //Debug.Log("ERROR: NO ATTACH POINTS ON STARTUP PIECE");
            return;
        }

        for (int i = 1; i < attachPoints.Count; i++)
        {
            if (themeForLevel == ChunkTheme.Theme1A)
            {
                GenerateAEndcap(attachPoints[i]);
            }
            if (themeForLevel == ChunkTheme.Theme1B)
            {
                GenerateBEndcap(attachPoints[i]);
            }
        }

        if (themeForLevel == ChunkTheme.Theme1A)
        {
            GenerateAChunk(attachPoints[0]);
        }

        if (themeForLevel == ChunkTheme.Theme1B)
        {
            GenerateBChunk(attachPoints[0]);
        }

    }
    void Start()
    {
        int index = Random.Range(0, skyboxProfilePairs.Length);
        SkyboxProfilePair selectedPair = skyboxProfilePairs[index];

        // Instantiate the skybox prefab
        if (currentSkybox != null)
        {
            Destroy(currentSkybox);
        }
        currentSkybox = Instantiate(selectedPair.skyboxPrefab);

        // Set the post-processing profile
        if (volumeGameObject != null)
        {
            Volume volume = volumeGameObject.GetComponent<Volume>();
            if (volume != null)
            {
                volume.profile = selectedPair.postProcessProfile;
                volume.enabled = true;
            }
            else
            {
                Debug.LogError("Volume component not found on the specified GameObject.");
            }
        }
        else
        {
            Debug.LogError("Volume GameObject is not assigned.");
        }

        GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().currentCharacter = GameObject.FindWithTag("currentPlayer");

        Transform[] playerChildren = GameObject.FindWithTag("currentPlayer").GetComponentsInChildren<Transform>();
        foreach (Transform child in playerChildren)
        {
            if (child.tag == "WeaponSlot")
            {
                GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().weaponHolder = child;
            }
        }

        if (GlobalData.currentWeapon != null)
        {
            //Restore Previous Weapon
            GameObject previousWeapon = Instantiate(Resources.Load(GlobalData.currentWeapon), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            previousWeapon.GetComponent<WeaponStats>().acceptingAttribute = false;
            StartCoroutine(SetPreviousWeaponStats(previousWeapon));
            if (previousWeapon.GetComponent<WeaponStats>().wpnName != "Stick")
            {
                previousWeapon.transform.localScale = previousWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
            }
            previousWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            previousWeapon.GetComponent<Collider>().enabled = false;
            GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().O_curWeapon = previousWeapon;
            previousWeapon.tag = "currentWeapon";

            //Restore Previous Stats
            CharacterStats stats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();
            stats.primalLevel = GlobalData.currentSporeStats[0];
            stats.speedLevel = GlobalData.currentSporeStats[1];
            stats.sentienceLevel = GlobalData.currentSporeStats[2];
            stats.vitalityLevel = GlobalData.currentSporeStats[3];

            GlobalData.currentWeapon = null;
            GlobalData.currentSporeStats.Clear();

            //Debug.Log("GLOBAL DATA WEAPON FOUND");
        }

        if (GameObject.FindWithTag("currentWeapon") == null)
        {
            GameObject startingWeapon = Instantiate(Resources.Load("Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().O_curWeapon = startingWeapon;

            //Debug.Log("NO GLOBAL DATA WEAPON FOUND");
        }
    }

    IEnumerator SetPreviousWeaponStats(GameObject previousWeapon)
    {
        yield return null;
        previousWeapon.GetComponent<WeaponStats>().statNums = GlobalData.currentWeaponStats;

        List<AttributeBase> atts = new List<AttributeBase>();
        for(int i = 0; i < GlobalData.currentAttribute.Count; i++){
            AttributeBase newAtt = AttributeAssigner.Instance.PickAttFromString(previousWeapon, GlobalData.currentAttribute[i].attName);
            newAtt.specialAttNum = GlobalData.currentAttribute[i].attValue;
            newAtt.rating = GlobalData.currentAttribute[i].rating;
            atts.Add(newAtt);
        }
        GlobalData.currentAttribute.Clear();
        yield return new WaitForSeconds(0.1f);
        previousWeapon.GetComponent<WeaponInteraction>().ApplyWeaponPositionAndRotation();
        for(int i = 0; i < atts.Count; i++){
            atts[i].Equipped();
        }

    }

    private GameObject GetStartChunkPrefab()
    {

        switch (themeForLevel)
        {
            case ChunkTheme.Theme1A:
                return Theme1A_StartChunkPrefabs[Random.Range(0, Theme1A_StartChunkPrefabs.Count)];
            case ChunkTheme.Theme1B:
                return Theme1B_StartChunkPrefabs[Random.Range(0, Theme1B_StartChunkPrefabs.Count)];
            default:
                Debug.LogError("Unknown theme: " + themeForLevel);
                return null;
        }
    }

    private GameObject GenerateAChunk(Transform transform)
    {
        GameObject selectedChunk = null;
        if (!allowDuplicateChunks)
        {
            while (selectedChunk == null)
            {
                int chunkIndex = Random.Range(0, Theme1A_MiddleChunkPrefabs.Count);
                GameObject chunk = Theme1A_MiddleChunkPrefabs[chunkIndex];

                if (!spawnedMiddleChunks.Contains(chunk))
                {
                    selectedChunk = chunk;
                    spawnedMiddleChunks.Add(chunk);
                    recentChunkIndex = chunkIndex;
                }
            }
            // If all chunks have been used this allows duplicates as a fallback
            if (selectedChunk == null)
            {
                selectedChunk = Theme1A_MiddleChunkPrefabs[Random.Range(0, Theme1A_MiddleChunkPrefabs.Count)];
            }
        }
        else
        {
            int achunkIndex = Random.Range(0, Theme1A_MiddleChunkPrefabs.Count);
            if (Theme1A_MiddleChunkPrefabs.Count > 1)
            {
                while (achunkIndex == recentChunkIndex)
                {
                    achunkIndex = Random.Range(0, Theme1A_MiddleChunkPrefabs.Count);
                }
            }
            recentChunkIndex = achunkIndex;
            selectedChunk = Theme1A_MiddleChunkPrefabs[achunkIndex];
        }
        currentMiddleChunk = Instantiate(selectedChunk, transform);
        NumberOfPieces++;

        List<Transform> attachPoints = new List<Transform>();
        foreach (Transform child in currentMiddleChunk.transform)
        {
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }
        int selectedAttatchPoint = Random.Range(0, attachPoints.Count);
        for (int i = 0; i < attachPoints.Count; i++)
        {
            if (i == selectedAttatchPoint)
            {
                if(CheckpointAChunk!=null && NumberOfPieces==(MaxNumberOfPieces+1)/2)
                    GenerateAMidpoint(attachPoints[i]);
                else if (NumberOfPieces <= MaxNumberOfPieces)
                    GenerateAChunk(attachPoints[i]);
                else
                    Instantiate(Theme1A_GoalChunkPrefabs[Random.Range(0, Theme1A_GoalChunkPrefabs.Count)], attachPoints[i]);
            }
            else
                GenerateAEndcap(attachPoints[i]);
        }
        return currentMiddleChunk;
    }

    private GameObject GenerateAMidpoint(Transform transform)
    {
        GameObject Apiece = Instantiate(CheckpointAChunk, transform);
        NumberOfPieces++;

        List<Transform> attachPoints = new List<Transform>();
        foreach (Transform child in Apiece.transform)
        {
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }
        int selectedAttatchPoint = Random.Range(0, attachPoints.Count);
        for (int i = 0; i < attachPoints.Count; i++)
        {
            if (i == selectedAttatchPoint)
            {
                if (NumberOfPieces <= MaxNumberOfPieces)
                    GenerateAChunk(attachPoints[i]);
                else
                    Instantiate(Theme1A_GoalChunkPrefabs[Random.Range(0, Theme1A_GoalChunkPrefabs.Count)], attachPoints[i]);
            }
            else
                GenerateAEndcap(attachPoints[i]);
        }
        return Apiece;
    }

    private GameObject GenerateBChunk(Transform transform)
    {
        GameObject selectedChunk = null;
        if (!allowDuplicateChunks)
        {
            while (selectedChunk == null)
            {
                int chunkIndex = Random.Range(0, Theme1A_MiddleChunkPrefabs.Count);
                GameObject chunk = Theme1A_MiddleChunkPrefabs[chunkIndex];

                if (!spawnedMiddleChunks.Contains(chunk))
                {
                    selectedChunk = chunk;
                    spawnedMiddleChunks.Add(chunk);
                    recentChunkIndex = chunkIndex;
                }
            }
            // If all chunks have been used this allows duplicates as a fallback
            if (selectedChunk == null)
            {
                selectedChunk = Theme1A_MiddleChunkPrefabs[Random.Range(0, Theme1A_MiddleChunkPrefabs.Count)];
            }
        }
        else
        {
            int bchunkIndex = Random.Range(0, Theme1A_MiddleChunkPrefabs.Count);
            if (Theme1A_MiddleChunkPrefabs.Count > 1)
            {
                while (bchunkIndex == recentChunkIndex)
                {
                    bchunkIndex = Random.Range(0, Theme1A_MiddleChunkPrefabs.Count);
                }
            }
            recentChunkIndex = bchunkIndex;
            selectedChunk = Theme1A_MiddleChunkPrefabs[bchunkIndex];
        }
        currentMiddleChunk = Instantiate(selectedChunk, transform);
        NumberOfPieces++;

        List<Transform> attachPoints = new List<Transform>();
        foreach (Transform child in currentMiddleChunk.transform)
        {
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }
        int selectedAttatchPoint = Random.Range(0, attachPoints.Count);
        for (int i = 0; i < attachPoints.Count; i++)
        {
            if (i == selectedAttatchPoint)
            {
                if(CheckpointAChunk!=null && NumberOfPieces==(MaxNumberOfPieces+1)/2)
                    GenerateBMidpoint(attachPoints[i]);
                else if (NumberOfPieces <= MaxNumberOfPieces)
                    GenerateBChunk(attachPoints[i]);
                else
                    Instantiate(Theme1B_GoalChunkPrefabs[Random.Range(0, Theme1B_GoalChunkPrefabs.Count)], attachPoints[i]);
            }
            else
                GenerateBEndcap(attachPoints[i]);
        }
        return currentMiddleChunk;


        //Debug.Log("Chunk Index: " + chunkIndex);





        //Debug.Log("Chunk Index: " + chunkIndex);



    }
    /*private GameObject GetMiddleChunkPrefab()
    {

        switch (themeForLevel)
        {
            case ChunkTheme.Theme1A:
                return Theme1A_MiddleChunkPrefabs[Random.Range(0, Theme1A_MiddleChunkPrefabs.Count)];
            case ChunkTheme.Theme1B:
                return Theme1B_MiddleChunkPrefabs[Random.Range(0, Theme1B_StartChunkPrefabs.Count)];
            default:
                Debug.LogError("Unknown theme: " + themeForLevel);
                return null;
        }
    }*/

    private void GenerateAEndcap(Transform transform)
    {
        Instantiate(Theme1A_GoalChunkPrefabs[Random.Range(0, Theme1A_GoalChunkPrefabs.Count)], transform);
    }
    private void GenerateBEndcap(Transform transform)
    {
        Instantiate(Theme1B_GoalChunkPrefabs[Random.Range(0, Theme1B_GoalChunkPrefabs.Count)], transform);
    }
    private void spawnPlayer(Transform spawnpoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("currentPlayer");
        player.transform.position = spawnpoint.position;
        player.transform.rotation = spawnpoint.rotation;
    }

    
    private GameObject GenerateBMidpoint(Transform transform)
    {
        GameObject Bpiece = Instantiate(CheckpointBChunk, transform);
        NumberOfPieces++;

        List<Transform> attachPoints = new List<Transform>();
        foreach (Transform child in Bpiece.transform)
        {
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }
        int selectedAttatchPoint = Random.Range(0, attachPoints.Count);
        for (int i = 0; i < attachPoints.Count; i++)
        {
            if (i == selectedAttatchPoint)
            {
                if (NumberOfPieces <= MaxNumberOfPieces)
                    GenerateAChunk(attachPoints[i]);
                else
                    Instantiate(Theme1A_GoalChunkPrefabs[Random.Range(0, Theme1A_GoalChunkPrefabs.Count)], attachPoints[i]);
            }
            else
                GenerateAEndcap(attachPoints[i]);
        }
        return Bpiece;
    }
}
