using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryTrait : TraitBase
{
    public override void Start(){
        base.Start();

        traitName = "Angry";
        traitDesc = "\nRandomly gain damage buffs";

        StartCoroutine(GetAngry());
    }

    IEnumerator GetAngry(){
        float randomTime = Random.Range(20, 30);
        Debug.Log("Time till angry: " + randomTime);
        yield return new WaitForSeconds(randomTime);
        Debug.Log("Get angry!");
        BonusDamage damageEffect = playerParent.AddComponent<BonusDamage>();
        damageEffect.InitializeDamageChange(5, 0.2f);
        StartCoroutine(GetAngry());
    }
}
