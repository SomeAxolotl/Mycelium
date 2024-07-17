using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public static IslandManager Instance { get; private set; }

    private GameObject activeIsland;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetActiveIsland(GameObject island)
    {
        if (activeIsland != null)
        {
            activeIsland.SetActive(false);
        }
        activeIsland = island;
        activeIsland.SetActive(true);
    }

    public bool IsIslandActive(GameObject island)
    {
        return island == activeIsland;
    }

    public GameObject GetActiveIsland()
    {
        return activeIsland;
    }

    public Transform GetIslandRespawnPoint(GameObject island)
    {
        Transform respawnPoint = FindChildWithTag(island.transform, "IslandSpawner");
        return respawnPoint;
    }

    private Transform FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child;
            }
            Transform result = FindChildWithTag(child, tag);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}
