using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float knockbackForce = 30f;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            collision.GetComponentInParent<NewPlayerHealth>().PlayerTakeDamage(damage);
            collision.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag != "currentPlayer")
        {
            Destroy(gameObject);
        }
    }
}
