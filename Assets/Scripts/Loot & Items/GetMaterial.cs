using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetMaterial : MonoBehaviour
{
    [SerializeField] private string materialName = "Rotten Log";

    [SerializeField] private Color descriptionColor;

    float distance;
    [SerializeField] private GameObject log;
    [SerializeField] private GameObject exoskeleton;
    [SerializeField] private GameObject calcite;
    [SerializeField] private GameObject flesh;

    ThirdPersonActionsAsset playerActionsAsset;
    private InputAction interact;
    GameObject player;
    NutrientTracker nutrientTracker;

    private HUDItem hudItem;

    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        interact = playerActionsAsset.Player.Interact;
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
        hudItem = GameObject.Find("HUD").GetComponent<HUDItem>();
    }
    private void Update()
    {
        transform.Rotate(0, 60 * Time.deltaTime, 0);

        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < 3f)
        {
            string subspeciesText = "N/A";
            switch (materialName)
            {
                case "Rotten Log":
                    subspeciesText = "basic";
                    break;
                case "Fresh Exoskeleton":
                    subspeciesText = "poisonous";
                    break;
                case "Calcite Deposit":
                    subspeciesText = "coral";
                    break;
                case "Flesh":
                    subspeciesText = "cordyceps";
                    break;
            }
            string subspeciesColoredText = "<color=#" + ColorUtility.ToHtmlStringRGB(descriptionColor) + ">"+subspeciesText+"</color>";

            TooltipManager.Instance.CreateTooltip(gameObject, materialName, "Used to grow and upgrade " + subspeciesColoredText + " Spores", "Press A to Pick Up\n(Hold A to salvage)");
            if (interact.triggered)
            {
                SoundEffectManager.Instance.PlaySound("Pickup", transform.position);

                AddMaterial();
                TooltipManager.Instance.DestroyTooltip();
            }
        }
        else if (distance > 3f && distance < 5f)
        {
            TooltipManager.Instance.DestroyTooltip();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            nutrientTracker.KeepMaterials();
        }
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
            hudItem.PickUpItem("RottenLog");
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
            hudItem.PickUpItem("FreshExoskeleton");
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
            hudItem.PickUpItem("CalciteDeposit");
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
}
