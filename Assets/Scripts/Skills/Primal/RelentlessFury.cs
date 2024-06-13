using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelentlessFury : Skill
{
    //Skill specific fields
    private float frenziedDuration = 5f;
    public bool isFrenzied;

    public override void DoSkill()
    {
        if(isPlayerCurrentPlayer()){ 
            Fury furyEffect = playerHealth.gameObject.AddComponent<Fury>();
        }
        EndSkill();
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
}
