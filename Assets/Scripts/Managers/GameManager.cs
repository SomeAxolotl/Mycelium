using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float nutrientDrainRate = 1f;
    bool shouldDrainNutrients;

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

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();

        //RemoveHUDMaterial();
        if (GameObject.FindWithTag("currentPlayer") != null)
        {
            StartCoroutine(UpdateHUDNutrients());
            StartCoroutine(PlacePlayer());
            RefreshCutoutMaskUI();
        }

        if (scene.buildIndex > 2)
        {
            StartCoroutine(DrainNutrients());
        }
    }

    public void RefreshCutoutMaskUI()
    {
        StartCoroutine(RefreshCutoutMaskUICoroutine());
    }

    IEnumerator RefreshCutoutMaskUICoroutine()
    {
        CutoutMaskUI[] cutoutMasks = GameObject.FindObjectsOfType<CutoutMaskUI>();
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
        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
        GameObject spawnPoint = GameObject.FindWithTag("PlayerSpawn");
        Rigidbody playerRB = currentPlayer.GetComponent<Rigidbody>();

        if (currentPlayer != null && spawnPoint != null)
        {
            currentPlayer.transform.position = spawnPoint.transform.position;
            currentPlayer.transform.rotation = spawnPoint.transform.rotation;
            playerRB.constraints = RigidbodyConstraints.FreezePosition; //To avoid falling out of map with high velocity

            yield return new WaitForEndOfFrame();
            playerRB.constraints = RigidbodyConstraints.FreezeRotation;
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

    IEnumerator DrainNutrients()
    {
        NutrientTracker nutrientTracker = GameObject.FindWithTag("Tracker").GetComponent<NutrientTracker>();

        while (shouldDrainNutrients)
        {
            nutrientTracker.SubtractNutrients(1);

            yield return new WaitForSeconds(nutrientDrainRate);
        }
    }
}
