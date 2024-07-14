using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    public static IslandManager Instance { get; private set; }

    [SerializeField] private IslandSetup[] islandSetups;

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

    public bool IsIslandActive(GameObject island)
    {
        return island.activeSelf;
    }

    public void SetIslandActive(GameObject island, bool isActive)
    {
        island.SetActive(isActive);
    }

    public bool CheckForDuplicateIslands()
    {
        HashSet<GameObject> activeIslands = new HashSet<GameObject>();
        foreach (var setup in islandSetups)
        {
            if (setup.island.activeSelf)
            {
                if (!activeIslands.Add(setup.island))
                {
                    return true; // Duplicate found
                }
            }
        }
        return false; // No duplicates
    }

    [System.Serializable]
    public class IslandSetup
    {
        public GameObject island;
    }
}
