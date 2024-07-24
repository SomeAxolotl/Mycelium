using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adaptive : EnemyAttributeBase
{
    private float damageBuffMultiplier = 1.5f; // Buff multiplier
    private float buffDuration = 10f; // Duration of the buff in seconds
    private bool isBuffActive = false;
    private Coroutine damageBuffCoroutine;
    private GameObject adaptiveParticles;

    protected override void OnInitialize() 
    {
        adaptiveParticles = transform.Find("AdaptiveParticles").gameObject;
        adaptiveParticles.SetActive(true);
    }

    public void ApplyDamageBuff()
    {
        if (damageBuffCoroutine != null)
        {
            StopCoroutine(damageBuffCoroutine);
        }
        damageBuffCoroutine = StartCoroutine(DamageBuffCoroutine());
    }

    private IEnumerator DamageBuffCoroutine()
    {
        isBuffActive = true;
        foreach (var attackScript in GetComponentsInChildren<IDamageBuffable>())
        {
            attackScript.ApplyDamageBuff(damageBuffMultiplier, buffDuration);
        }
        yield return new WaitForSeconds(buffDuration);
        foreach (var attackScript in GetComponentsInChildren<IDamageBuffable>())
        {
            attackScript.RemoveDamageBuff();
        }
        isBuffActive = false;
        damageBuffCoroutine = null;
    }

    public interface IDamageBuffable
    {
        void ApplyDamageBuff(float multiplier, float duration);
        void RemoveDamageBuff();
    }
}
