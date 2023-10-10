using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyHitbox : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
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
        if (collision.gameObject.tag == "currentPlayer")
        {
            Debug.Log("Player Hit!");
            GameObject.FindWithTag("currentPlayer").GetComponent<PlayerHealth>().currentHealth -= damage;
        }
    }
}
