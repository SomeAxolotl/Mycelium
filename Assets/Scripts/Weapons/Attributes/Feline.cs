using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feline : AttributeBase
{
    public override void Initialize()
    {
        if(stats == null || hit == null){return;}
        attName = "Feline";
        attDesc = "\nmeow";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Hit(GameObject target, float damage){
        SoundEffectManager.Instance.PlaySound("Walking", GameObject.FindWithTag("currentPlayer").transform.position);
    }
}
