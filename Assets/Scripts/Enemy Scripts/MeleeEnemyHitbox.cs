using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float knockbackForce = 50f;
    List<GameObject> playerHit = new List<GameObject>();
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false && !playerHit.Contains(collision.gameObject))
        {
            collision.GetComponentInParent<NewPlayerHealth>().PlayerTakeDamage(damage);
            collision.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(collision.gameObject);

        }
    }
    public void ClearPlayerList()
    {
        playerHit.Clear();
    }
}
