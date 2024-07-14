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

    public Transform GetIslandRespawnPoint(GameObject island)
    {
        var respawnPoint = island.transform.Find("IslandSpawn");
        return respawnPoint != null ? respawnPoint : null;
    }
}
