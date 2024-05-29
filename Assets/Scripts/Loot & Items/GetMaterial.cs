using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using RonaldSunglassesEmoji.Interaction;
using UnityEditor;

public class GetMaterial : MonoBehaviour, IInteractable
{
    [SerializeField] private string materialName = "Rotten Log";

    [SerializeField] private int nutrientsSalvaged = 200;

    [SerializeField] private Color descriptionColor;

    float distance;
    [SerializeField] private GameObject log;
    [SerializeField] private GameObject exoskeleton;
    [SerializeField] private GameObject calcite;
    [SerializeField] private GameObject flesh;

    //ThirdPersonActionsAsset playerActionsAsset;
    GameObject player;
    NutrientTracker nutrientTracker;

    private HUDItem hudItem;

    // Start is called before the first frame update
    void Start()
    {
        //playerActionsAsset = new ThirdPersonActionsAsset();
        //playerActionsAsset.Player.Enable();
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
    }

    void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            nutrientTracker.KeepMaterials();
        }
    }

    public void Interact(GameObject interactObject)
    {
        SoundEffectManager.Instance.PlaySound("Pickup", transform);

        AddMaterial();
        TooltipManager.Instance.DestroyTooltip();
    }

    public void Salvage(GameObject interactObject)
    {
        SalvageNutrients(nutrientsSalvaged);
    }

    public void CreateTooltip(GameObject interactObject)
    {
        string subspeciesText = "N/A";
        int materialIconIndex = 8;
        switch (materialName)
        {
            case "Rotten Log":
                subspeciesText = "sporelius";
                materialIconIndex = 8;
                break;
            case "Fresh Exoskeleton":
                subspeciesText = "toxitious";
                materialIconIndex = 9;
                break;
            case "Calcite Deposit":
                subspeciesText = "costaalis";
                materialIconIndex = 10;
                break;
            case "Flesh":
                subspeciesText = "gloomacea";
                materialIconIndex = 11;
                break;
        }
        string subspeciesColoredText = "<color=#" + ColorUtility.ToHtmlStringRGB(descriptionColor) + ">"+subspeciesText+"</color>";

        string interactText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        string salvageText = InputManager.Instance.GetLatestController().salvageHint.GenerateColoredHintString();
        TooltipManager.Instance.CreateTooltip
        (
            gameObject, 
            "<sprite=" + materialIconIndex + ">  " + materialName + "  <sprite=" + materialIconIndex + ">", 
            "Used to grow and upgrade " + subspeciesColoredText + " Spores", 
            "Press "+interactText+" to Pick Up", 
            "(Press "+salvageText+" to Salvage)"
        );
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }

    private void AddMaterial()
    {
        nutrientTracker.LoseMaterials();
        if (gameObject.name == "RottenLog" || gameObject.name == "RottenLog(Clone)")
        {
            nutrientTracker.heldLog++;
            if (nutrientTracker.heldItem == null)
            { 
                nutrientTracker.heldItem = log;
            }
            else
            {
                nutrientTracker.heldItem.transform.position = gameObject.transform.position;
                nutrientTracker.heldItem.SetActive(true);
                nutrientTracker.heldItem = log;
            }
            hudItem.PickUpItem("Rotten Log");
        }

        if (gameObject.name == "Exoskeleton" || gameObject.name == "Exoskeleton(Clone)")
        {
            nutrientTracker.heldExoskeleton++;
            if (nutrientTracker.heldItem == null)
            {
                nutrientTracker.heldItem = exoskeleton;
            }
            else
            {
                nutrientTracker.heldItem.transform.position = gameObject.transform.position;
                nutrientTracker.heldItem.SetActive(true);
                nutrientTracker.heldItem = exoskeleton;
            }
            hudItem.PickUpItem("Fresh Exoskeleton");
        }

        if (gameObject.name == "Calcite" || gameObject.name == "Calcite(Clone)")
        {
            nutrientTracker.heldCalcite++;
            if (nutrientTracker.heldItem == null)
            {
                nutrientTracker.heldItem = calcite;
            }
            else
            {
                nutrientTracker.heldItem.transform.position = gameObject.transform.position;
                nutrientTracker.heldItem.SetActive(true);
                nutrientTracker.heldItem = calcite;
            }
            hudItem.PickUpItem("Calcite Deposit");
        }

        if (gameObject.name == "Flesh" || gameObject.name == "Flesh(Clone)")
        {
            nutrientTracker.heldFlesh++;
            if (nutrientTracker.heldItem == null)
            {
                nutrientTracker.heldItem = flesh;
            }
            else
            {
                nutrientTracker.heldItem.transform.position = gameObject.transform.position;
                nutrientTracker.heldItem.SetActive(true);
                nutrientTracker.heldItem = flesh;
            }
            hudItem.PickUpItem("Flesh");
        }
        gameObject.SetActive(false);
    }

    void SalvageNutrients(int nutrientAmount)
    {
        if (GlobalData.currentLoop >= 2)
        {
            nutrientAmount = (nutrientAmount * (GlobalData.currentLoop / 2));
        }
        nutrientTracker.AddNutrients(nutrientAmount);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientAmount / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }
}
