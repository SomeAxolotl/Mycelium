using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(PlacePlayer());
    }

    IEnumerator PlacePlayer()
    {
        yield return new WaitForEndOfFrame();
        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");

        currentPlayer.transform.position = transform.position;
        currentPlayer.transform.rotation = transform.rotation;

        GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().EnableController();
    }
}
