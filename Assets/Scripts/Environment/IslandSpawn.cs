using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawn : MonoBehaviour
{
    [SerializeField] private string whirlpoolName; // Name of this whirlpool
    [SerializeField] private float timeUntilRespawn = 0.2f;
    public GameObject[] potentialIslands; // Potential islands for this whirlpool
    private Transform islandRespawnPoint; // Public transform to be set based on the active island
    [SerializeField] private GameObject pairedIsland; // Paired island for this whirlpool

    private IslandManager islandManager;
    public static bool islandActivated = false; // Static variable to ensure only one island is activated

    void Start()
    {
        islandManager = IslandManager.Instance;
        if (islandManager == null)
        {
            Debug.LogError("IslandManager not found in the scene.");
            return;
        }

        StartCoroutine(ActivateRandomWhirlpool());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("currentPlayer") && other.GetType() == typeof(CapsuleCollider))
        {
            StartCoroutine(RespawnPlayer());
        }
    }

    IEnumerator RespawnPlayer()
    {
        GlobalData.isAbleToPause = false;

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");

        if (islandRespawnPoint == null)
        {
            Debug.LogError($"No island respawn point set for {whirlpoolName}");
            yield break;
        }

        // Fade screen
        HUDController hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        yield return hudController.StartCoroutine(hudController.BlackFade(true));
        yield return new WaitForSeconds(timeUntilRespawn / 2);

        // Teleport player
        currentPlayer.transform.position = islandRespawnPoint.position;
        GameManager.Instance.NavigateCamera();
        Rigidbody rb = currentPlayer.GetComponent<Rigidbody>();
        // Constraints rigidbody for a frame to prevent clipping
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        yield return null;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        yield return new WaitForSeconds(timeUntilRespawn);
        yield return hudController.StartCoroutine(hudController.BlackFade(false));

        GlobalData.isAbleToPause = true;
    }

    private IEnumerator ActivateRandomWhirlpool()
    {
        yield return new WaitForSeconds(2f); // Wait for a few seconds to ensure all whirlpools are initialized

        // Find all active whirlpools in the scene
        IslandSpawn[] whirlpools = FindObjectsOfType<IslandSpawn>();

        if (whirlpools.Length == 0)
        {
            Debug.LogError("No whirlpools found in the scene.");
            yield break;
        }

        // Randomly select one whirlpool to activate an island
        IslandSpawn selectedWhirlpool = whirlpools[Random.Range(0, whirlpools.Length)];
        selectedWhirlpool.ActivateRandomIsland();

        // Set the flag to true so no other whirlpool tries to activate an island
        islandActivated = true;

        // Deactivate all other whirlpools
        foreach (var whirlpool in whirlpools)
        {
            if (whirlpool != selectedWhirlpool)
            {
                whirlpool.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateRandomIsland()
    {
        int randomIndex = Random.Range(0, potentialIslands.Length);
        GameObject selectedIsland = potentialIslands[randomIndex];
        Debug.Log($"Attempting to activate island: {selectedIsland.name}");

        islandManager.SetActiveIsland(selectedIsland);
        islandRespawnPoint = islandManager.GetIslandRespawnPoint(selectedIsland);

        if (islandRespawnPoint == null)
        {
            Debug.LogError($"No IslandSpawner transform found for island {selectedIsland.name}. Ensure it has a child tagged 'IslandSpawner'.");
            // Log the hierarchy of the selected island
            LogHierarchy(selectedIsland.transform);
        }
        else
        {
            pairedIsland = selectedIsland; // Update the paired island
            Debug.Log($"Island {selectedIsland.name} activated with respawn point {islandRespawnPoint.name}");
        }
    }

    private void LogHierarchy(Transform parent, string indent = "")
    {
        foreach (Transform child in parent)
        {
            Debug.Log($"{indent}- {child.name} (Tag: {child.tag})");
            LogHierarchy(child, indent + "  ");
        }
    }

    public GameObject GetPairedIsland()
    {
        return pairedIsland;
    }
}
