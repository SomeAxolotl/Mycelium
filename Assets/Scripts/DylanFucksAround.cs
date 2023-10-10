using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DylanFucksAround : MonoBehaviour
{
    [Header("Chunk Lists")]
    [SerializeField][Tooltip("Chunks which end branches.")] private List<GameObject> CapPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which have 2 seams. This is the majority of chunks.")] private List<GameObject> MiddleChunkPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which begin a level.")] private List<GameObject> StartChunkPrefabs = new List<GameObject>();
    [SerializeField][Tooltip("Chunks which end a level.")] private List<GameObject> GoalChunkPrefabs = new List<GameObject>();

    [Header("Level Configuration")]
    [SerializeField] private int MaxNumberOfPieces;
    private int NumberOfPieces = 0;

    void Start()
    {
        GameObject StartPiece = Instantiate(StartChunkPrefabs[Random.Range(0,StartChunkPrefabs.Count)], transform);
        NumberOfPieces++;
        
        List<Transform> attachPoints = new List<Transform>();
        foreach(Transform child in StartPiece.transform)
        {
            if (child.tag == "Attach")
                attachPoints.Add(child);
        }

        if(attachPoints.Count == 0)
        {
            Debug.Log("ERROR: NO ATTACH POINTS ON STARTUP PIECE");
            return;
        }

        for(int i = 1; i < attachPoints.Count; i++)
        {
            GenerateEndcap(attachPoints[i]);
        }

        GenerateSHIT(attachPoints[0]);
    }

    private GameObject GenerateSHIT(Transform transform)
    {
        GameObject Piece = Instantiate(MiddleChunkPrefabs[Random.Range(0, MiddleChunkPrefabs.Count)], transform);
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
                    GenerateSHIT(attachPoints[i]);
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

    /*private Transform NewTransform(Transform oldTransform)
    {
        oldTransform.localScale = Vector3.one;
        Debug.Log("Name: "+ oldTransform);
        Debug.Log("Position: "+ oldTransform.position);
        Debug.Log("Rotation: "+ oldTransform.rotation);
        Debug.Log("Scale: "+ oldTransform.localScale);
        Debug.Log("_____________");
        return oldTransform;
    }*/
}