using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawn : MonoBehaviour
{
    [SerializeField] private string whirlpoolName; // Name of this whirlpool
    [SerializeField] private float timeUntilRespawn = 0.2f;
    [SerializeField] private GameObject[] potentialIslands; // Potential islands for this whirlpool
    [SerializeField] private Transform islandRespawnPoint; // Public transform to be set based on the active island

    private IslandManager islandManager;
    private GameObject activeIsland;

    void Start()
    {
        islandManager = IslandManager.Instance;
        if (islandManager == null)
        {
            Debug.LogError("IslandManager not found in the scene.");
            return;
        }

        ActivateRandomIsland();
        UpdateRespawnPoint();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && other.GetType() == typeof(CapsuleCollider))
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

    private void UpdateRespawnPoint()
    {
        if (activeIsland != null)
        {
            GameObject respawnPointObject = GameObject.FindWithTag("IslandSpawn");
            if (respawnPointObject != null)
            {
                islandRespawnPoint = respawnPointObject.transform;
            }
            else
            {
                Debug.LogError("No IslandSpawn tagged object found in the active island.");
            }
        }
    }

    private void ActivateRandomIsland()
    {
        GameObject selectedIsland = null;

        // Find a random island that does not activate a duplicate island
        while (selectedIsland == null || islandManager.IsIslandActive(selectedIsland))
        {
            selectedIsland = potentialIslands[Random.Range(0, potentialIslands.Length)];
        }

        // Set the selected island active
        islandManager.SetIslandActive(selectedIsland, true);
        activeIsland = selectedIsland;

        // Update the respawn point for this whirlpool
        UpdateRespawnPoint();
    }
}
