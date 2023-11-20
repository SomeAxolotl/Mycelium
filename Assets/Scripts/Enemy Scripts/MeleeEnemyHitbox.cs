using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float knockbackForce = 50f;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            collision.GetComponentInParent<NewPlayerHealth>().PlayerTakeDamage(damage);
            collision.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
        }
    }
}
