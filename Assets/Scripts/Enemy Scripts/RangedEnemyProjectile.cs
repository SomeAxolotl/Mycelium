using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && collision.GetComponentInParent<PlayerController>().isInvincible == false)
        {
            collision.GetComponentInParent<NewPlayerHealth>().PlayerTakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag != "currentPlayer")
        {
            Destroy(gameObject);
        }
    }
}
