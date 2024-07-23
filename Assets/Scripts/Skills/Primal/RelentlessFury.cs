using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelentlessFury : Skill
{
    //Skill specific fields
    public bool isFrenzied;

    Fury furyEffect;
    public override void DoSkill()
    {
        if(isPlayerCurrentPlayer()){ 
            furyEffect = playerHealth.gameObject.AddComponent<Fury>();
            furyEffect.EffectEnd += ActualCooldownStart;
            //Starts the UI timer for the effect
            furyEffect.EffectRefresh += RefreshTimer;
            SoundEffectManager.Instance.PlaySound("FuryBuff", player.transform);
            RefreshTimer();
        }
        EndSkill();
    }

    private void RefreshTimer(){
        hudSkills.ToggleActiveBorder(skillSlot, true);
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDEffectCoroutine(hudCooldownCoroutine);
        }
        hudCooldownCoroutine = hudSkills.StartEffectUI(skillSlot, furyEffect.currTimerMax);
    }

    void HurtPlayer()
    {
        playerHealth.PlayerTakeDamage(playerHealth.maxHealth * .05f);
    }

    float savedCooldown;
    public override void StartCooldown(float skillCooldown){
        savedCooldown = skillCooldown;
        //Does not do cooldown normally
        canSkill = false;
    }

    protected override void ActualCooldownStart(){
        furyEffect.EffectEnd -= ActualCooldownStart;
        furyEffect.EffectRefresh -= RefreshTimer;

        hudSkills.ToggleActiveBorder(skillSlot, false);

        if(cooldownCoroutine != null){
            StopCoroutine(cooldownCoroutine);
        }
        if(hudCooldownCoroutine != null){
            hudSkills.StopHUDCoroutine(hudCooldownCoroutine);
        }

        cooldownCoroutine = StartCoroutine(Cooldown(savedCooldown));
    }
}
