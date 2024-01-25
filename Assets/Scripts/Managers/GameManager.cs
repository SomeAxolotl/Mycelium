using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(PlacePlayer());
        StartCoroutine(EnableController());
        RemoveHUDMaterial();
        StartCoroutine(UpdateHUDNutrients());

        Debug.Log("Game Manager: Scene Loaded - " + scene.name);
    }

    IEnumerator PlacePlayer()
    {
        yield return new WaitForEndOfFrame();

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        GameObject spawnPoint = GameObject.FindWithTag("PlayerSpawn");
        Debug.Log("Player: " + currentPlayer);
        Debug.Log("SpawnPoint: " + spawnPoint);

        currentPlayer.transform.position = spawnPoint.transform.position;
        currentPlayer.transform.rotation = spawnPoint.transform.rotation;
    }

    IEnumerator EnableController()
    {
        yield return new WaitForEndOfFrame();

        GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().EnableController();
    }

    void RemoveHUDMaterial()
    {
        HUDItem hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
        hudItem.LostItem();
    }

    IEnumerator UpdateHUDNutrients()
    {
        yield return null;

        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(0);
    }
}
