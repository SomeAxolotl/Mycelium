using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 20f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer" && GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>().isInvincible == false)
        {
            GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerHealth>().PlayerTakeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.tag != "currentPlayer")
        {
            Destroy(gameObject);
        }
    }
}
