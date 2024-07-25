using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using RonaldSunglassesEmoji.Interaction;
using UnityEditor;

public class GigaShroomInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string materialName = "Rotten Log";

    [SerializeField] private int nutrientsSalvaged = 200;

    [SerializeField] private Color descriptionColor;
    private int salvagedDeposits = 0;
    private const int totalDepositsNeeded = 5;

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
    }
    public void Interact(GameObject interactObject)
    {

        // Check if all deposits have been salvaged
        if (salvagedDeposits >= totalDepositsNeeded)
        {
            // Provide reward to the player
            ProvideReward();
        }
        TooltipManager.Instance.DestroyTooltip();
    }

    public void Salvage(GameObject interactObject)
    {
        // Increment the count of salvaged deposits
        salvagedDeposits++;

        // Check if all deposits have been salvaged
        if (salvagedDeposits >= totalDepositsNeeded)
        {
            // Provide reward to the player
            ProvideReward();
        }
    }

    public void CreateTooltip(GameObject interactObject)
    {
        // Display tooltip informing the player to salvage deposits
        string buttonText = InputManager.Instance.GetLatestController().interactHint.GenerateColoredHintString();
        TooltipManager.Instance.CreateTooltip
        (
            this.gameObject,
            "Loot Cache",
            "Contains Rewards!",
            "Press " + buttonText + " to Open"
        );
    }

    public void DestroyTooltip(GameObject interactObject, bool isFromInteracting = false)
    {
        TooltipManager.Instance.DestroyTooltip();
    }


    void SalvageNutrients(int nutrientAmount)
    {
        if (GlobalData.currentLoop >= 2)
        {
            nutrientAmount = (nutrientAmount * ((GlobalData.currentLoop + 1) / 2));
        }
        nutrientTracker.AddNutrients(nutrientAmount);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientAmount / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }
    private void ProvideReward()
    {
        // Provide reward to the player for salvaging all deposits
        Debug.Log("All deposits salvaged! Providing reward...");
        // Add your reward logic here
    }
}
