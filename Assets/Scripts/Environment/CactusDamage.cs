using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusDamage : MonoBehaviour
{
    private float damage = 20;
    private float knockbackForce = 140;

    [SerializeField] private GameObject launchPoint;

    private List<GameObject> hitTargets = new List<GameObject>();

    private void OnTriggerStay(Collider collider){
        damage = 20;
        knockbackForce = 140;

        PlayerHealth player = collider.GetComponentInParent<PlayerHealth>();
        if(player != null && hitTargets.Contains(player.gameObject) == false){
            StartCoroutine(HurtCooldown(player.gameObject));
            player.PlayerTakeDamage(damage);
            player.transform.GetComponent<PlayerController>().Knockback(gameObject, knockbackForce);
        }
    }

    IEnumerator HurtCooldown(GameObject thing){    
        hitTargets.Add(thing);
        Debug.Log(thing);
        yield return new WaitForSeconds(0.5f);
        hitTargets.Remove(thing);

    }

}
