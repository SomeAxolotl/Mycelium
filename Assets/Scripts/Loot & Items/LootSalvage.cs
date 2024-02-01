using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class LootSalvage : MonoBehaviour
{
    [SerializeField] private int nutrientsSalvaged = 200;
    float distance;
    ThirdPersonActionsAsset playerActionsAsset;
    InputAction salvage;
    NutrientTracker nutrientTracker;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        salvage = playerActionsAsset.Player.Salvage;
        player = GameObject.FindWithTag("currentPlayer");
        nutrientTracker = GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>();
    }

    // Update is called once per frame
    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance < 3f && salvage.triggered)
        {
            Debug.Log("Salvaged!");
            SalvageNutrients(nutrientsSalvaged);
        }
    }

    void SalvageNutrients(int nutrientAmount)
    {
        nutrientTracker.AddNutrients(nutrientAmount);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientAmount / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        TooltipManager.Instance.DestroyTooltip();
        Destroy(this.gameObject);
    }
}
