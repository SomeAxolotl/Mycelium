using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SporeAnimationEvents : MonoBehaviour
{
    [SerializeField] float slashDonePercent = 0.5f;
    [SerializeField] float stabDonePercent = 0.8f;

    ParticleSystem currentSlashParticle;

    ParticleSystem currentStabParticle;

    void Footstep1()
    {
        SoundEffectManager.Instance.PlaySound("Footstep 1", transform.position);
    }

    void Footstep2()
    {
        SoundEffectManager.Instance.PlaySound("Footstep 2", transform.position);
    }

    void Slash()
    {
        SoundEffectManager.Instance.PlaySound("Slash", transform.position);

        GameObject currentWeapon = GameObject.FindWithTag("currentWeapon");

        if (currentWeapon != null)
        {
            Transform particleHolder = currentWeapon.transform.Find("ParticleHolder");

            currentSlashParticle = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("SlashParticle", particleHolder.position, Quaternion.identity, particleHolder.gameObject);
        
            StartCoroutine(DestroyParticleAfterDone(currentSlashParticle.gameObject, "Slash", slashDonePercent));
        }
    }

    void Stab()
    {
        SoundEffectManager.Instance.PlaySound("Stab", transform.position);
    }

    void StabDone()
    {
        //StartCoroutine(FadeOutParticle(currentSlashParticle));
        if (currentStabParticle != null)
        {
            Destroy(currentStabParticle.gameObject);
        }
    }

    void StabTip()
    {
        GameObject currentWeapon = GameObject.FindWithTag("currentWeapon");
        //leo code here
         if (currentWeapon != null)
        {
            Transform particleHolder = currentWeapon.transform.Find("ParticleHolder");

            currentStabParticle = ParticleManager.Instance.SpawnParticlesAndGetParticleSystem("StabParticle", particleHolder.position, particleHolder.transform.rotation, particleHolder.gameObject);
            StartCoroutine(DestroyParticleAfterDone(currentStabParticle.gameObject, "Stab", stabDonePercent)); 
        }
    }

    void Smash()
    {
        if (gameObject.tag == "currentPlayer")
        {
            SoundEffectManager.Instance.PlaySound("Smash", transform.position);
        }
    }

    void Pant()
    {
        SoundEffectManager.Instance.PlaySound("Panting", transform.position);
    }
    void SmashPart()
    {
        if (gameObject.tag == "currentPlayer")
        {
            Transform particleHolder = GameObject.FindWithTag("currentWeapon").transform.Find("ParticleHolder");
            SoundEffectManager.Instance.PlaySound("Explosion", transform.position);
            ParticleManager.Instance.SpawnParticles("SmashParticle",particleHolder.position,Quaternion.Euler(90,0,0));
        }
    }

    IEnumerator DestroyParticleAfterDone(GameObject particleObject, string animationName, float percentUntilDestroy)
    {
        Animator sporeAnimator = GetComponent<Animator>();
        yield return new WaitUntil(() => sporeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= percentUntilDestroy || !sporeAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName));
    
        Destroy(particleObject);
    }
}
