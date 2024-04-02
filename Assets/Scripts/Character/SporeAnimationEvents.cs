using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SporeAnimationEvents : MonoBehaviour
{
    GameObject slashParticle;
    ParticleSystem slashParticleSystem;
    ParticleSystem.EmissionModule slashParticleSystemEmission;

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

        slashParticle = GameObject.FindWithTag("currentWeapon").transform.Find("SlashParticle").gameObject;
        slashParticle.SetActive(true);

        //slashParticleSystem = GameObject.FindWithTag("currentWeapon").transform.Find("SlashParticle").GetComponent<ParticleSystem>();

        /*slashParticleSystemEmission = GameObject.FindWithTag("currentWeapon").transform.Find("SlashParticle").GetComponent<ParticleSystem>().emission;
        if (slashParticleSystem != null)
        {
            slashParticleSystemEmission.enabled = true;
        }*/
    }

    void SlashDone()
    {
        /*if (slashParticleSystem != null)
        {
            slashParticleSystemEmission.enabled = false;
        }*/

        slashParticle.SetActive(false);
    }

    void Stab()
    {
        SoundEffectManager.Instance.PlaySound("Stab", transform.position);
    }

    void Smash()
    {
        SoundEffectManager.Instance.PlaySound("Smash", transform.position);
    }

    void Pant()
    {
        SoundEffectManager.Instance.PlaySound("Panting", transform.position);
    }

    void SmashPart()
    {
        Transform particleHolder = GameObject.FindWithTag("currentWeapon").transform.Find("ParticleHolder");
        SoundEffectManager.Instance.PlaySound("Explosion", transform.position);
        ParticleManager.Instance.SpawnParticles("SmashParticle",particleHolder.position,Quaternion.Euler(90,0,0));
    }
}
