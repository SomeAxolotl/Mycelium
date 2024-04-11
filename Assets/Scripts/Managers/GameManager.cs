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

    GameObject vcamHolder;

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
        vcamHolder = GameObject.Find("VCamHolder");

        //RemoveHUDMaterial();
        if (GameObject.FindWithTag("currentPlayer") != null)
        {
            StartCoroutine(UpdateHUDNutrients());
            PlaceSpore(GameObject.FindWithTag("currentPlayer"));
            StartCoroutine(RefreshCutoutMaskUI());
        }

        if (scene.buildIndex == 2)
        {
            if (GlobalData.sporePermaDied != null)
            {
                HappinessManager.Instance.FriendlySporePermaDied();
            }
            else
            {
                StartCoroutine(SproutPlayer());

                GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");
                currentPlayer.GetComponent<CharacterStats>().ModifyEnergy(-GlobalData.areasCleared);

                HappinessManager.Instance.RestOtherSpores(currentPlayer);
            }

            GlobalData.areasCleared = 0;
            GlobalData.sporePermaDied = null;
        }
        else
        {
            if (GameObject.Find("HUD") != null)
            {
                HUDHappiness hudHappiness = GameObject.Find("HUD").GetComponent<HUDHappiness>();
                hudHappiness.HideColonyHappinessMeter();
            }
        }

        if (scene.buildIndex == 3)
        {
            StartCoroutine(ApplyHappinessBuffToCurrentPlayer());
        }

        if (scene.buildIndex > 2)
        {
            NavigateCamera();
            StartCoroutine(DrainNutrients());
        }
    }

    public void NavigateCamera()
    {
        StartCoroutine(NavigateCameraCoroutine());
    }

    IEnumerator NavigateCameraCoroutine()
    {
        yield return null;

        vcamHolder.GetComponent<VCamRotator>().OnNavigateCamera();
    }

    IEnumerator ApplyHappinessBuffToCurrentPlayer()
    {
        yield return null;

        CharacterStats characterStats = GameObject.FindWithTag("currentPlayer").GetComponent<CharacterStats>();

        if (HappinessManager.Instance.doesHappinessMultiply)
        {
            characterStats.MultiplyStat("All", GlobalData.happinessStatMultiplier);
        }
        else
        {
            characterStats.AddStat("All", GlobalData.happinessStatIncrement);
        }

        GlobalData.happinessStatMultiplier = 1f;
        GlobalData.happinessStatIncrement = 0;
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

    public void PlaceSpore(GameObject spore)
    {
        StartCoroutine(PlaceSporeCoroutine(spore));
    }

    IEnumerator PlaceSporeCoroutine(GameObject spore)
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawn");
        Rigidbody sporeRB = spore.GetComponent<Rigidbody>();

        int randomSpawnNumber = Random.Range(0, spawnPoints.Length);
        GameObject selectedSpawnPoint = spawnPoints[randomSpawnNumber];

        if (spore != null && selectedSpawnPoint != null)
        {
            Vector3 randomOffset = Random.insideUnitCircle * 0.5f;
            Vector3 newPosition = selectedSpawnPoint.transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);
            spore.transform.position = newPosition;
            spore.transform.rotation = selectedSpawnPoint.transform.rotation;
            sporeRB.constraints = RigidbodyConstraints.FreezePosition; //To avoid falling out of map with high velocity

            yield return new WaitForEndOfFrame();
            sporeRB.constraints = RigidbodyConstraints.FreezeRotation;
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
            yield return new WaitForSeconds(nutrientDrainRate);

            nutrientTracker.SubtractNutrients(1);
        }
    }
}
