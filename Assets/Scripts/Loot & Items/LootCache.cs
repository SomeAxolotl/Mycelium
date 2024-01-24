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
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, this.transform.position);
        if (distance < 3)
        {
            TooltipManager.Instance.CreateTooltip(this.gameObject, "Loot Cache", "Contains Rewards!", "Press [BUTTON] to Open");
            Debug.Log("distance =" + distance);
            if (interact.triggered)
            {
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
        	Instantiate(possibleDrops[randomDropIndex], new Vector3(transform.position.x, transform.position.y + .8f, transform.position.z), Quaternion.identity);
        }

        int randomNutrientValue = Random.Range(nutrientMin, nutrientMax);
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(randomNutrientValue);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randomNutrientValue / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }
}
