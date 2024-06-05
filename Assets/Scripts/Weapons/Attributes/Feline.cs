using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feline : AttributeBase
{
    public override void Initialize()
    {
        attName = "Feline";
        attDesc = "meow";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Hit(GameObject target, float damage){
        SoundEffectManager.Instance.PlaySound("Walking", GameObject.FindWithTag("currentPlayer").transform);
    }
}
