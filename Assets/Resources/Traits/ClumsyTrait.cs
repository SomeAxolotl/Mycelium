using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClumsyTrait : TraitBase
{
    private GameObject clumsyPrefab;
    private GameObject clumsyArea;

    public override void Start(){
        base.Start();

        traitName = "Clumsy";
        traitDesc = "Damage enemies when dashing";

        clumsyPrefab = Resources.Load("Attributes/Clumsy", typeof(GameObject)) as GameObject;
        clumsyArea = Instantiate(clumsyPrefab, player.transform);

        DamageRoll rollScript = clumsyArea.GetComponent<DamageRoll>();
        if(rollScript != null){
            rollScript.player = gameObject;
            rollScript.characterStats = GetComponent<CharacterStats>();
        }else{
            Debug.Log("How is there no damage roll script please fix this");
        }

        DisableArea();
    }

    public override void SporeSelected(){
        Actions.ActivatedDodge += DamageArea;
        Actions.FinishedDodge += DisableArea;
    }
    public override void SporeUnselected(){
        Actions.ActivatedDodge -= DamageArea;
        Actions.FinishedDodge -= DisableArea;
    }

    public void DamageArea(){
        clumsyArea.SetActive(true);
    }
    public void DisableArea(){
        clumsyArea.SetActive(false);
    }
}
