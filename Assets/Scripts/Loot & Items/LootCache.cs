using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Progress;


public class LootCache : MonoBehaviour
{
    float distance;
    [SerializeField] private List<GameObject> possibleDrops;
    [SerializeField] private int nutrientMin = 50;
    [SerializeField] private int nutrientMax = 200;
    ThirdPersonActionsAsset playerActionsAsset;
    private InputAction interact;
    GameObject player;
    private void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        interact = playerActionsAsset.Player.Interact;
        player = GameObject.FindWithTag("currentPlayer");

        //THIS IS A TEMPORARY FIX FOR CACHES!!! PLEASE GO INTO THE CHUNKS AND MANUALLY FIX THE NUTRIENT VALUES.
        if(nutrientMin >= 60)
        {
            nutrientMin = nutrientMin / 20;
        }

        if(nutrientMax >= 60)
        {
            nutrientMax = nutrientMax / 20;
        }
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < 3)
        {
            Debug.Log("SPAWNING TOOLTIP FOR WEAPON");
            string buttonText = "<color=#3cdb4e>A</color>";
            TooltipManager.Instance.CreateTooltip(this.gameObject, "Loot Cache", "Contains Rewards!", "Press "+buttonText+" to Open");
            if (interact.triggered)
            {
                SoundEffectManager.Instance.PlaySound("Pickup", transform.position);

                TooltipManager.Instance.DestroyTooltip();
                GetLoot();
                Destroy(this.gameObject);
            }
        }
        else if (distance < 5)
        {
            TooltipManager.Instance.DestroyTooltip();
        }
    }
    public void GetLoot()
    {
        int randomDropIndex = Random.Range(0, possibleDrops.Count);
        if (possibleDrops[randomDropIndex] != null)
        {
        	Instantiate(possibleDrops[randomDropIndex], new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
        }

        int randomNutrientValue = Random.Range(nutrientMin, nutrientMax);
        //GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(randomNutrientValue);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randomNutrientValue, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }
}
