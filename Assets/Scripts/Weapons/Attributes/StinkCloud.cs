using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkCloud : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageInterval = 2.0f;
    public float reentryCooldown = 2.0f;

    public CharacterStats stats;

    private List<GameObject> targets = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();
    private Dictionary<GameObject, float> lastExitTime = new Dictionary<GameObject, float>();

    private void OnTriggerEnter(Collider other)
    {
        if (!targets.Contains(other.gameObject))
        {
            if (lastExitTime.ContainsKey(other.gameObject))
            {
                float lastTime = lastExitTime[other.gameObject];
                if (Time.time - lastTime < reentryCooldown)
                {
                    return; // If cooldown hasn't passed, don't re-add the target
                }
            }

            targets.Add(other.gameObject);
            Coroutine damageCoroutine = StartCoroutine(DamageCoroutine(other));
            activeCoroutines[other.gameObject] = damageCoroutine;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (targets.Contains(other.gameObject))
        {
            targets.Remove(other.gameObject);
            if (activeCoroutines.ContainsKey(other.gameObject))
            {
                if (activeCoroutines[other.gameObject] != null)
                {
                    StopCoroutine(activeCoroutines[other.gameObject]);
                    activeCoroutines.Remove(other.gameObject);
                }
            }
            lastExitTime[other.gameObject] = Time.time; // Store the time when the target exited the trigger
        }
    }

    private IEnumerator DamageCoroutine(Collider target)
    {
        EnemyHealth targetHealth = target.GetComponent<EnemyHealth>();
        while (targetHealth != null && targetHealth.currentHealth > 0)
        {
            float statEffectiveness = 0.25f;
            float damage = Mathf.Clamp(damageAmount + (stats.sentienceLevel * statEffectiveness), 2, Mathf.Infinity);
            targetHealth.EnemyTakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
