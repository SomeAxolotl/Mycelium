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
        Debug.Log("Game Manager: Scene Loaded - " + scene.name);

        if (scene.buildIndex == 0)
        {
            FreezePlayer();
            StartCoroutine(DisableController());
            DestroyMenuCamera();
        }
        else
        {
            UnfreezePlayer();
            StartCoroutine(PlacePlayer());
            StartCoroutine(EnableController());
            RemoveHUDMaterial();
            StartCoroutine(UpdateHUDNutrients());
        }

        StartCoroutine(RefreshCutoutMaskUI());
    }

    public void OnPlayerDeath()
    {
        GameObject.Find("HUD").GetComponent<HUDBoss>().DefeatEnemy();
    }

    public void OnExitToMainMenu()
    {
        GameObject.Find("HUD").GetComponent<HUDBoss>().DefeatEnemy();
    }

    public void OnExitToHub()
    {
        GameObject.Find("HUD").GetComponent<HUDBoss>().DefeatEnemy();
        Debug.Log("DEFEAT ENEMY");
    }

    IEnumerator RefreshCutoutMaskUI()
    {
        CutoutMaskUI[] cutoutMasks = GameObject.Find("HUD").GetComponentsInChildren<CutoutMaskUI>();
        foreach (CutoutMaskUI cutoutMask in cutoutMasks)
        {
            cutoutMask.enabled = !cutoutMask.enabled;
        }
        yield return null;
        foreach (CutoutMaskUI cutoutMask in cutoutMasks)
        {
            cutoutMask.enabled = !cutoutMask.enabled;
        }
    }

    IEnumerator PlacePlayer()
    {
        yield return new WaitForEndOfFrame();

        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        GameObject spawnPoint = GameObject.FindWithTag("PlayerSpawn");

        if (spawnPoint != null)
        {
            currentPlayer.transform.position = spawnPoint.transform.position;
            currentPlayer.transform.rotation = spawnPoint.transform.rotation;
        }
    }

    IEnumerator EnableController()
    {
        yield return new WaitForEndOfFrame();

        if (GameObject.FindWithTag("PlayerParent") != null)
        {
            GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().EnableController();
        }
    }

    IEnumerator DisableController()
    {
        yield return new WaitForEndOfFrame();

        if (GameObject.FindWithTag("PlayerParent") != null)
        {
            GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().DisableController();
        }
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

    void FreezePlayer()
    {
        Rigidbody playerRB = GameObject.FindWithTag("currentPlayer").GetComponent<Rigidbody>();

        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    void UnfreezePlayer()
    {
        Rigidbody playerRB = GameObject.FindWithTag("currentPlayer").GetComponent<Rigidbody>();

        if (playerRB != null)
        {
            playerRB.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    void DestroyMenuCamera()
    {
        Destroy(GameObject.Find("Menu Camera"));
    }
}
