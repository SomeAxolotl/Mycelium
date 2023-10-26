using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    float deathTimer;
    Rigidbody rb;
    NewPlayerAttack newPlayerAttack;
    EnemyHealthBar enemyHealthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        newPlayerAttack = GetComponent<NewPlayerAttack>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
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
        deathTimer += Time.deltaTime;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        this.gameObject.GetComponent<Collider>().enabled = false;
        rb.Sleep();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
        if(deathTimer >= 2f)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
        enemyHealthBar.UpdateEnemyHealth();
        enemyHealthBar.DamageNumber(dmgTaken);
    }
}
