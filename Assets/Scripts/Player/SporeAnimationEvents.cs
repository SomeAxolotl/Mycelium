using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SporeAnimationEvents : MonoBehaviour
{
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

        ParticleManager.Instance.SpawnParticles("SmashParticle",particleHolder.position,Quaternion.Euler(90,0,0));
    }
}
