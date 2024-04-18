using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushyWeaponCollision : MonoBehaviour
{
    [SerializeField] private MushyAttack mushyAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !mushyAttack.playerHit.Contains(other.gameObject) && mushyAttack.isAttacking)
        {
            mushyAttack.playerDamaged = true;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(mushyAttack.damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, mushyAttack.knockbackForce);
            mushyAttack.playerHit.Add(other.gameObject);
        }
    }
}
