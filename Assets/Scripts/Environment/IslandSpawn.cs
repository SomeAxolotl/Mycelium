using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandSpawn : MonoBehaviour
{
    [SerializeField] float timeUntilRespawn = 0.2f;

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

        // Find all island respawn points
        GameObject[] islandRespawnPoints = GameObject.FindGameObjectsWithTag("IslandSpawn");

        if (islandRespawnPoints.Length == 0)
        {
            Debug.LogError($"No island respawn points set for {gameObject}");
            yield break;
        }

        // Calculate the respawn point
        GameObject respawnPoint = islandRespawnPoints[Random.Range(0, islandRespawnPoints.Length)];

        // Fade screen
        HUDController hudController = GameObject.Find("HUD").GetComponent<HUDController>();
        yield return hudController.StartCoroutine(hudController.BlackFade(true));
        yield return new WaitForSeconds(timeUntilRespawn / 2);

        // Teleport player
        currentPlayer.transform.position = respawnPoint.transform.position;
        GameManager.Instance.NavigateCamera();

        yield return null;

        yield return new WaitForSeconds(timeUntilRespawn);
        yield return hudController.StartCoroutine(hudController.BlackFade(false));

        GlobalData.isAbleToPause = true;
    }
}
