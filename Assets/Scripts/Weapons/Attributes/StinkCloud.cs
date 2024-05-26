using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinkCloud : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageInterval = 2.0f;

    private List<GameObject> targets = new List<GameObject>();
    private Dictionary<GameObject, Coroutine> activeCoroutines = new Dictionary<GameObject, Coroutine>();

    private void OnTriggerEnter(Collider other){
        if(!targets.Contains(other.gameObject)){
            targets.Add(other.gameObject);
            Coroutine damageCoroutine = StartCoroutine(DamageCoroutine(other));
            activeCoroutines[other.gameObject] = damageCoroutine;
        }
    }

    private void OnTriggerExit(Collider other){
        if(targets.Contains(other.gameObject)){
            targets.Remove(other.gameObject);
            if(activeCoroutines.ContainsKey(other.gameObject)){
                if(activeCoroutines[other.gameObject] != null){
                    StopCoroutine(activeCoroutines[other.gameObject]);
                    activeCoroutines.Remove(other.gameObject);
                }
            }
        }
    }

    private IEnumerator DamageCoroutine(Collider target){
        EnemyHealth targetHealth = target.GetComponent<EnemyHealth>();
        while(targetHealth != null && targetHealth.currentHealth > 0){
            targetHealth.EnemyTakeDamage(damageAmount);
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
