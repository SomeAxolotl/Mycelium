using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetMaterial : MonoBehaviour
{
    float distance;
    [SerializeField] private GameObject log;
    [SerializeField] private GameObject exoskeleton;
    [SerializeField] private GameObject calcite;
    [SerializeField] private GameObject flesh;

    ThirdPersonActionsAsset playerActionsAsset;
    private InputAction interact;
    GameObject player;
    NutrientTracker nutrientTracker;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        interact = playerActionsAsset.Player.Interact;
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
    }
    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (interact.triggered && distance < 2)
        {
            AddMaterial();
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
        }
        gameObject.SetActive(false);
    }
}
