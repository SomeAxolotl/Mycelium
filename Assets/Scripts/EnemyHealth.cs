using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;
    public int nutrientDrop;
    float dmgTaken;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    { 
        //Debug.Log("Enemy Health: " + currentHealth);
        //Debug.Log("Dmg Taken: " + dmgTaken);
        
        if(currentHealth <= 0)
        {
            StartCoroutine("Death");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "currentWeapon")
        {
            //Gets the damage value from the players melee attack script and subtracts that from its own health.
            dmgTaken = GameObject.FindWithTag("currentPlayer").GetComponent<MeleeAttack>().finalDmg;
            currentHealth -= dmgTaken;
        }
    }
    IEnumerator Death()
    {
        //This is just a placeholder death animation which also gives the player the amount of nutrients that this enemy is supposed to drop.
        this.gameObject.GetComponent<Collider>().enabled = false;
        rb.Sleep();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        yield return new WaitForSeconds(2f);
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().currentNutrients += nutrientDrop;
        this.gameObject.SetActive(false);
    }
}
