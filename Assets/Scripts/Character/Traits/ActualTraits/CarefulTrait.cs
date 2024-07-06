using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarefulTrait : TraitBase
{
    private float outOfCombatTime = 12;
    private float defensePercent = 75;

    DefenseChange defenseChangeEffect;
    DefenseChange.defenseChangeInfo defChange;

    public override void Start(){
        base.Start();

        traitName = "Careful";
        traitDesc = "Gains a shield after not taking damage";
    }
    public override void SporeSelected(){
        O_health.TakeDamage += TakeDamage;
        combatTimer = OutOfCombatTimer(0.2f);
        StartCoroutine(combatTimer);
    }
    public override void SporeUnselected(){
        if (GlobalData.isQuitting) return;

        O_health.TakeDamage -= TakeDamage;
        //Stops all coroutines and turns off the bubble
        if(combatTimer != null){
            StopCoroutine(combatTimer);
            combatTimer = null;
        }
        if(delayTimer != null){
            StopCoroutine(delayTimer);
            delayTimer = null;
        }
        if(defChange != null){
            defChange.changeDuration = 0;
        }
    }

    private void GainSheild(){
        defenseChangeEffect = O_health.gameObject.AddComponent<DefenseChange>();
        defChange = defenseChangeEffect.InitializeDefenseChange(Mathf.Infinity, defensePercent, false, this);
    }

    private void TakeDamage(float dmgTaken){
        if(defenseChangeEffect != null && delayTimer == null){
            delayTimer = RemoveDelay();
            StartCoroutine(delayTimer);
        }
        ResetCombatTimer();
    }

    private void ResetCombatTimer(){
        if(combatTimer != null){
            StopCoroutine(combatTimer);
            combatTimer = null;
        }
        combatTimer = OutOfCombatTimer(outOfCombatTime);
        StartCoroutine(combatTimer);
    }

    private IEnumerator delayTimer;
    IEnumerator RemoveDelay(){
        if(defChange != null){
            defChange.changeDuration = 0.2f;
            defenseChangeEffect.ManageBubbles();
        }
        yield return new WaitForSeconds(0.2f);
        RemoveShield();
    }

    private void RemoveShield(){
        delayTimer = null;
        if(defenseChangeEffect != null){
            Debug.Log("Remove careful shield");
            defenseChangeEffect.RemoveBySource(this);
        }
    }

    private IEnumerator combatTimer;
    IEnumerator OutOfCombatTimer(float timer){
        yield return new WaitForSeconds(timer);
        GainSheild();
    }
}
