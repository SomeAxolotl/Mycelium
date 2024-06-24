using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feline : AttributeBase
{
    public override void Initialize()
    {
        attName = "Feline";
        attDesc = "meow";
    }

    public override void Hit(GameObject target, float damage){
        SoundEffectManager.Instance.PlaySound("Meow", GameObject.FindWithTag("currentPlayer").transform);
    }
}
