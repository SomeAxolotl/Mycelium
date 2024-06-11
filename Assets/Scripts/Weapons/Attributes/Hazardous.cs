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
            defenseChangeEffect.InitializeDefenseChange(7, -25);
            yield return new WaitForSeconds(2);
            DefenseChange defenseChangeEffect2 = target.AddComponent<DefenseChange>();
            defenseChangeEffect2.InitializeDefenseChange(2, 55);
        }
    }
}
