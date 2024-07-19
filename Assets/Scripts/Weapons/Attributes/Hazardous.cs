using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazardous : AttributeBase
{
    public override void Initialize(){
        attName = "Hazardous";
        attDesc = "Apply 25% vulnerable for 3 seconds";
    }

    public override void Hit(GameObject target, float damage){
        StartCoroutine(ApplyVulnerable(target));
    }

    private IEnumerator ApplyVulnerable(GameObject target){
        yield return new WaitForEndOfFrame();
        if(target.GetComponent<EnemyHealth>().currentHealth > 0){
            DefenseChange defenseChangeEffect = target.AddComponent<DefenseChange>();
            defenseChangeEffect.InitializeDefenseChange(3, -25);
        }
    }
}
