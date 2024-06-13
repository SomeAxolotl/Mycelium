using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelentlessFury : Skill
{
    //Skill specific fields
    private float frenziedDuration = 8f;
    public bool isFrenzied;

    Fury furyEffect;
    public override void DoSkill()
    {
        if(isPlayerCurrentPlayer()){ 
            furyEffect = playerHealth.gameObject.AddComponent<Fury>();
            furyEffect.EffectEnd += ActualCooldownStart;
            //Starts the UI timer for the effect
            furyEffect.EffectRefresh += RefreshTimer;
            RefreshTimer();
        }
        EndSkill();
    }

    private void RefreshTimer(){
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDEffectCoroutine(hudCooldownCoroutine);
        }
        hudCooldownCoroutine = hudSkills.StartEffectUI(skillSlot, furyEffect.currTimerMax);
    }

    IEnumerator Frenzied()
    {
        yield return new WaitForSeconds(frenziedDuration);
        /*
        isFrenzied = true;
        playerController.canUseDodge = false;
        float storedAnimSpeed = currentAnimator.speed;
        currentAnimator.speed = storedAnimSpeed * 1.3f;
        yield return new WaitForSeconds(frenziedDuration);
        CancelInvoke();
        currentAnimator.speed = storedAnimSpeed;
        playerController.canUseDodge = true;
        isFrenzied = false;
        */
    }
    void HurtPlayer()
    {
        playerHealth.PlayerTakeDamage(playerHealth.maxHealth * .05f);
    }

    float savedCooldown;
    public override void StartCooldown(float skillCooldown){
        savedCooldown = skillCooldown;
        //Does not do cooldown normally
    }

    private void ActualCooldownStart(){
        furyEffect.EffectEnd -= ActualCooldownStart;
        furyEffect.EffectRefresh -= RefreshTimer;

        if(cooldownCoroutine != null){
            StopCoroutine(cooldownCoroutine);
        }
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDCoroutine(hudCooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(Cooldown(savedCooldown));
    }
}
