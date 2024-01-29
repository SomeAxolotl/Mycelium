using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class LootSalvage : MonoBehaviour
{
    [SerializeField] private int nutrientMin = 50;
    [SerializeField] private int nutrientMax = 200;
    ThirdPersonActionsAsset playerActionsAsset;
    private InputAction salvage;
    private SwapWeapon swapWeapon;
    GameObject player;
    GameObject currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
        playerActionsAsset = new ThirdPersonActionsAsset();
        playerActionsAsset.Player.Enable();
        salvage = playerActionsAsset.Player.Salvage;
        player = GameObject.FindWithTag("currentPlayer");
        currentWeapon = GameObject.FindWithTag("currentWeapon");
    }

    // Update is called once per frame
    private void Update()
    {
        if (salvage.triggered)
        {
            TooltipManager.Instance.DestroyTooltip();
            
            if (GameObject.FindWithTag("currentWeapon") != null && currentWeapon) 
            {
                return;
            }
          
             Destroy(this.gameObject);
             SalvageNutrients();
            
        }
    }

    public void SalvageNutrients()
    {
        int randomNutrientValue = Random.Range(nutrientMin, nutrientMax);
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(randomNutrientValue);
        ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", randomNutrientValue / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
    }
}
