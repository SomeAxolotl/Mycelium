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
    }

    private IEnumerator angryTimer;
    public override void SporeSelected(){
        StartAngry();
    }
    public override void SporeUnselected(){
        if(angryTimer != null){
            StopCoroutine(angryTimer);
            angryTimer = null;
        }
    }

    private void StartAngry(){
        angryTimer = GetAngry();
        StartCoroutine(angryTimer);
    }

    BonusDamage damageEffect;
    IEnumerator GetAngry(){
        float randomTime = Random.Range(minTime, maxTime);
        Debug.Log("Time till angry: " + randomTime);
        yield return new WaitForSeconds(randomTime);
        Debug.Log("Get angry!");
        damageEffect = O_playerParent.AddComponent<BonusDamage>();
        damageEffect.InitializeDamageChange(duration, damageIncrease);
        StartAngry();
    }
}
