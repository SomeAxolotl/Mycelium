using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RelentlessFury : Skill
{
    //Skill specific fields
    private float frenziedDuration = 5f;
    public GameObject relentlessFuryParticles;
    public bool isFrenzied;

    public override void DoSkill()
    {
        //Skill specific stuff
        FrenzyParticles();
        if (isPlayerCurrentPlayer())
        { 
            StartCoroutine(Frenzied());  
            InvokeRepeating("HurtPlayer", 0f, 1f);
        }
        EndSkill();
    }
    void FrenzyParticles()
    {
        Instantiate(relentlessFuryParticles, player.transform.position, Quaternion.Euler(-90f, 0f, 0f), player.transform);
    }

    IEnumerator Frenzied()
    {
        isFrenzied = true;
        playerController.canUseDodge = false;
        float storedAnimSpeed = currentAnimator.speed;
        currentAnimator.speed = storedAnimSpeed * 1.3f;
        yield return new WaitForSeconds(frenziedDuration);
        CancelInvoke();
        currentAnimator.speed = storedAnimSpeed;
        playerController.canUseDodge = true;
        isFrenzied = false;
    }
    void HurtPlayer()
    {
        playerHealth.PlayerTakeDamage(playerHealth.maxHealth * .05f);
    }
}
