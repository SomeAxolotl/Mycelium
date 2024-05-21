using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazardous : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Hazardous";
        attDesc = "\nApply 25% vulnerable for 3 seconds";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Hit(GameObject target, float damage){
        StartCoroutine(ApplyVulnerable(target));
    }

    private IEnumerator ApplyVulnerable(GameObject target){
        yield return new WaitForEndOfFrame();
        DefenseChange defenseChangeEffect = target.AddComponent<DefenseChange>();
        defenseChangeEffect.InitializeDefenseChange(3, -25);
    }
}
