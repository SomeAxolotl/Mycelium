using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float nutrientDrainRate = 1f;
    [SerializeField] float timeUntilHubSprout = 10f;

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
            StartCoroutine(RefreshCutoutMaskUI());
        }

        if (scene.buildIndex == 2)
        {
            StartCoroutine(SproutPlayer());
        }

        if (scene.buildIndex > 2)
        {
            StartCoroutine(DrainNutrients());
        }
    }

    IEnumerator SproutPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        
        GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().DisableController();

        yield return new WaitForSeconds(timeUntilHubSprout);

        GameObject.FindWithTag("currentPlayer").GetComponent<Animator>().Play("Sprout");
        yield return new WaitUntil(() => GameObject.FindWithTag("currentPlayer").GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1f);

        GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().EnableController();
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

        while (true)
        {
            nutrientTracker.SubtractNutrients(1);

            yield return new WaitForSeconds(nutrientDrainRate);
        }
    }
}
