using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunk Lists")]
    [SerializeField][Tooltip("Chunks which end branches.")] private List<GameObject> CapPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which have 2 seams. This is the majority of chunks.")] private List<GameObject> MiddleChunkPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which begin a level.")] private List<GameObject> StartChunkPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which end a level.")] private List<GameObject> GoalChunkPrefabs = new List<GameObject>();

    [Header("Level Configuration")]
    [SerializeField] private int MaxNumberOfPieces;
    private int NumberOfPieces = 0;
    private int recentChunkIndex = 0;

    void Awake()
    {
        NavMesh.RemoveAllNavMeshData();

        GameObject[] tooltips = GameObject.FindGameObjectsWithTag("Tooltip");
        foreach (GameObject tooltip in tooltips)
        {
            Destroy(tooltip);
        }

        GameObject StartPiece = Instantiate(StartChunkPrefabs[Random.Range(0,StartChunkPrefabs.Count)], transform);
        NumberOfPieces++;
        List<Transform> attachPoints = new List<Transform>();
        foreach(Transform child in StartPiece.transform)
        {
            //if (child.tag.CompareTo("PlayerSpawn")==0)
                //spawnPlayer(child.transform);
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }

        if(attachPoints.Count == 0)
        {
            //Debug.Log("ERROR: NO ATTACH POINTS ON STARTUP PIECE");
            return;
        }

        for(int i = 1; i < attachPoints.Count; i++)
        {
            GenerateEndcap(attachPoints[i]);
        }

        GenerateChunk(attachPoints[0]);
    }
    void Start()
    {
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
            GameObject previousWeapon = Instantiate(Resources.Load(GlobalData.currentWeapon), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            StartCoroutine(SetPreviousWeaponStats(previousWeapon));
            if (previousWeapon.GetComponent<WeaponStats>().wpnName != "Stick")
            {
                previousWeapon.transform.localScale = previousWeapon.transform.localScale / 0.03f / 100f / 1.2563f;
            }
            previousWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            previousWeapon.GetComponent<Collider>().enabled = false;
            GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().curWeapon = previousWeapon;
            previousWeapon.tag = "currentWeapon";

            GlobalData.currentWeapon = null;

            //Debug.Log("GLOBAL DATA WEAPON FOUND");
        }

        if (GameObject.FindWithTag("currentWeapon") == null)
        {
            GameObject startingWeapon = Instantiate(Resources.Load("Daybreak Arboretum/Slash/Stick"), GameObject.FindWithTag("WeaponSlot").transform) as GameObject;
            startingWeapon.layer = LayerMask.NameToLayer("currentWeapon");
            startingWeapon.GetComponent<Collider>().enabled = false;
            GameObject.Find("PlayerParent").GetComponent<SwapWeapon>().curWeapon = startingWeapon;

            //Debug.Log("NO GLOBAL DATA WEAPON FOUND");
        }
    }

    private GameObject GenerateChunk(Transform transform)
    {
        int chunkIndex = Random.Range(0, MiddleChunkPrefabs.Count);
        if (MiddleChunkPrefabs.Count > 1)
        {
            while (chunkIndex == recentChunkIndex)
            {
                chunkIndex = Random.Range(0, MiddleChunkPrefabs.Count);
            }
        }
        recentChunkIndex = chunkIndex;

        //Debug.Log("Chunk Index: " + chunkIndex);
        GameObject Piece = Instantiate(MiddleChunkPrefabs[chunkIndex], transform);
        NumberOfPieces++;

        List<Transform> attachPoints = new List<Transform>();
        foreach(Transform child in Piece.transform)
        {
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }
        int selectedAttatchPoint = Random.Range(0,attachPoints.Count);
        for(int i = 0; i < attachPoints.Count; i++)
        {
            if (i == selectedAttatchPoint)
            {
                if(NumberOfPieces<=MaxNumberOfPieces)
                    GenerateChunk(attachPoints[i]);
                else
                    Instantiate(GoalChunkPrefabs[Random.Range(0, GoalChunkPrefabs.Count)], attachPoints[i]);
            }
            else
                GenerateEndcap(attachPoints[i]);
        }
        return Piece;
    }

    private void GenerateEndcap(Transform transform)
    {
        Instantiate(CapPrefabs[Random.Range(0,CapPrefabs.Count)], transform);
    }

    private void spawnPlayer(Transform spawnpoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("currentPlayer");
        player.transform.position = spawnpoint.position;
        player.transform.rotation = spawnpoint.rotation;
    }

    IEnumerator SetPreviousWeaponStats(GameObject previousWeapon)
    {
        yield return null;
        previousWeapon.GetComponent<WeaponStats>().wpnDamage = GlobalData.currentWeaponDamage;
        previousWeapon.GetComponent<WeaponStats>().wpnKnockback = GlobalData.currentWeaponKnockback;
    }
}