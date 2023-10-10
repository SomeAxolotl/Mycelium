using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 25f;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer")
        {
            Debug.Log("Player Hit!");
            GameObject.FindWithTag("currentPlayer").GetComponent<PlayerHealth>().Hurt(damage);
        }
    }
}
