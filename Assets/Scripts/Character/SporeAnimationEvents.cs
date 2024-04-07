using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SporeAnimationEvents : MonoBehaviour
{
    [SerializeField] float particleFadeOutTime = 0.5f;

    ParticleSystem currentSlashParticle;

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
        }
    }

    void SlashDone()
    {
        //StartCoroutine(FadeOutParticle(currentSlashParticle));
        if (currentSlashParticle != null)
        {
            Destroy(currentSlashParticle.gameObject);
        }
    }

    IEnumerator FadeOutParticle(ParticleSystem.MainModule slashParticleMain)
    {
        Color startingColor = slashParticleMain.startColor.color;

        float elapsedTime = 0f;
        while (elapsedTime < particleFadeOutTime)
        {
            float t = elapsedTime / particleFadeOutTime;

            slashParticleMain.startColor = new Color(startingColor.r, startingColor.g, startingColor.b, Mathf.Lerp(1f, 0f, t));

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        slashParticleMain.startColor = new Color(startingColor.r, startingColor.g, startingColor.b, 0f);
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
