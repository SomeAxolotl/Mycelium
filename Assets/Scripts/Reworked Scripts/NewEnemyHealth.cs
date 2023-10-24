using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    Rigidbody rb;
    NewPlayerAttack newPlayerAttack;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        newPlayerAttack = GetComponent<NewPlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
        Debug.Log(currentHealth);
    }
    void Death()
    {

    }
    public void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
    }
}
