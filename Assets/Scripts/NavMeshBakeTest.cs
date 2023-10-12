using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBakeTest : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] navMeshSurfaces;

    void Awake()
    {
        for(int i = 0; i < navMeshSurfaces.Length; i++)
        {
            navMeshSurfaces[i].BuildNavMesh();
        }
    }
}
