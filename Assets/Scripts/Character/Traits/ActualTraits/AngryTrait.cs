using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryTrait : TraitBase
{
    private float minTime = 20;
    private float maxTime = 30;

    private float duration = 5;
    //0.2 = 20%
    private float damageIncrease = 0.2f;

    public override void Start(){
        base.Start();

        traitName = "Angry";
        traitDesc = "Randomly gains damage buffs";

        StartCoroutine(GetAngry());
    }

    IEnumerator GetAngry(){
        float randomTime = Random.Range(minTime, maxTime);
        Debug.Log("Time till angry: " + randomTime);
        yield return new WaitForSeconds(randomTime);
        Debug.Log("Get angry!");
        BonusDamage damageEffect = playerParent.AddComponent<BonusDamage>();
        damageEffect.InitializeDamageChange(duration, damageIncrease);
        StartCoroutine(GetAngry());
    }
}
