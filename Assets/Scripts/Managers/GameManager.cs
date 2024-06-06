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

    [SerializeField] CarcassEnvironmentManager environmentManager;

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
        GameObject currentPlayer = GameObject.FindWithTag("currentPlayer");

        //If there's a current player,
        if (currentPlayer != null)
        {
            StartCoroutine(UpdateHUDNutrients());
            PlaceSpore(currentPlayer);
            StartCoroutine(RefreshCutoutMaskUI());
        }

        //At the Carcass,
        if (scene.buildIndex == 2)
        {
            if (GlobalData.isDay)
            {
                environmentManager.Selection = "Day";
                environmentManager.UpdateEnvironments();
            }
            else
            {
                environmentManager.Selection = "Dawn";
                environmentManager.UpdateEnvironments();
            }
            GlobalData.isDay = !GlobalData.isDay;

            //if a spore permanently died,
            if (GlobalData.sporePermaDied != null)
            {
                //Reduce all happiness
                HappinessManager.Instance.FriendlySporePermaDied();
                
                //and if an area was cleared, rest EVERY Spore (since the current one died)
                if (GlobalData.areaCleared)
                {
                    HappinessManager.Instance.RestAllSpores();
                }
            }
            //if a spore DIDN'T permanently die,
            else
            {
                //but if it still died, reduce its happiness
                if (GlobalData.sporeDied != null)
                {
                    currentPlayer.GetComponent<CharacterStats>().ModifyHappiness(HappinessManager.Instance.happinessOnDying);
                }

                //and if an area was cleared, spend an energy from the current Spore, and rest every OTHER Spore
                if (GlobalData.areaCleared)
                {
                    currentPlayer.GetComponent<CharacterStats>().ModifyEnergy(-1);
                    HappinessManager.Instance.RestOtherSpores(currentPlayer);
                }

                //sprout the current Spore
                StartCoroutine(SproutPlayer());
            }

            //Only hide colony happiness if the character count is less than 2
            HUDHappiness hudHappiness = GameObject.Find("HUD").GetComponent<HUDHappiness>();
            if (GameObject.FindWithTag("PlayerParent").GetComponent<SwapCharacter>().characters.Count < 2)
            {
                hudHappiness.HideColonyHappinessMeter();
            }
        }
        else
        {
            //Always hide colony happiness if not at the Carcass
            if (GameObject.Find("HUD") != null)
            {
                HUDHappiness hudHappiness = GameObject.Find("HUD").GetComponent<HUDHappiness>();
                hudHappiness.HideColonyHappinessMeter();
            }
        }

        //Apply the happiness stat boost in the first area
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

        characterStats.transform.parent.GetComponent<PlayerHealth>().ResetHealth();

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
        GameObject.Find("HUD").GetComponent<HUDNutrients>().UpdateNutrientMultiplierUI();
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
