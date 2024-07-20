using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnWhirlpools : MonoBehaviour
{
    private Transform returnSpawnPoint; // The return spawn point to teleport the player to
    [SerializeField] private GameObject pairedWhirlpool; // Paired normal whirlpool for this return whirlpool

    void Start()
    {
        FindReturnSpawnPoint();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("currentPlayer") && other.GetType() == typeof(CapsuleCollider))
        {
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        GlobalData.isAbleToPause = false;

        if (returnSpawnPoint == null)
        {
            Debug.LogError("Return spawn point not found for ReturnWhirlpool.");
            yield break;
        }

        // Fade screen
        HUDController hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        yield return hudController.StartCoroutine(hudController.BlackFade(true));
        yield return new WaitForSeconds(0.5f); // Adjust this duration as needed

        // Teleport player
        player.transform.position = returnSpawnPoint.position;
        GameManager.Instance.NavigateCamera();
        Rigidbody rb = player.GetComponent<Rigidbody>();
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

        yield return new WaitForSeconds(0.5f); // Adjust this duration as needed
        yield return hudController.StartCoroutine(hudController.BlackFade(false));

        GlobalData.isAbleToPause = true;
    }

    private void FindReturnSpawnPoint()
    {
        // Identify the active island
        GameObject activeIsland = FindActiveIsland();
        if (activeIsland == null)
        {
            Debug.LogError("No active island found.");
            return;
        }

        // Find the paired whirlpool associated with the active island
        pairedWhirlpool = FindPairedWhirlpool(activeIsland);
        if (pairedWhirlpool != null)
        {
            // Find the child with the "ReturnSpawn" tag
            returnSpawnPoint = FindChildWithTag(pairedWhirlpool.transform, "ReturnSpawn");
            if (returnSpawnPoint != null)
            {
                Debug.Log($"Return spawn point found: {returnSpawnPoint.name}");
            }
            else
            {
                Debug.LogError($"No ReturnSpawn transform found in paired whirlpool {pairedWhirlpool.name}.");
            }
        }
        else
        {
            Debug.LogError("Paired whirlpool not found.");
        }
    }

    private GameObject FindActiveIsland()
    {
        // Find the active island based on your game's logic
        IslandManager islandManager = IslandManager.Instance;
        if (islandManager != null)
        {
            GameObject activeIsland = islandManager.GetActiveIsland();
            if (activeIsland != null)
            {
                Debug.Log($"Active island found: {activeIsland.name}");
                return activeIsland;
            }
        }
        Debug.LogError("Active island not found.");
        return null;
    }

    private GameObject FindPairedWhirlpool(GameObject activeIsland)
    {
        // This function should contain the logic to find the paired whirlpool based on the active island
        IslandSpawn[] whirlpools = FindObjectsOfType<IslandSpawn>();
        foreach (IslandSpawn whirlpool in whirlpools)
        {
            if (whirlpool.GetPairedIsland() == activeIsland)
            {
                return whirlpool.gameObject;
            }
        }
        return null;
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
