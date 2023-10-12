using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    float currentHealth;
    public int nutrientDrop;
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
        if(currentHealth <= 0)
        {
            StartCoroutine("Death");
        }
        Debug.Log(currentHealth);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "currentWeapon")
        {
            //Gets the damage value from the players melee attack script and subtracts that from its own health.
            var dmgTaken = GameObject.FindWithTag("currentPlayer").GetComponent<MeleeAttack>().finalDmg;
            currentHealth -= dmgTaken;
        }
        if(other.gameObject.tag == "AoEHitbox")
        {
            var dmgTaken = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").GetChild(0).GetComponent<EruptionSkill>().finalEruptionDmg;
            currentHealth -= dmgTaken;
            StartCoroutine("Stunned");
        }
    }
    IEnumerator Death()
    {
        //This is just a placeholder death animation which also gives the player the amount of nutrients that this enemy is supposed to drop.
        Debug.Log("enemy dead");
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        this.gameObject.GetComponent<Collider>().enabled = false;
        rb.Sleep();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        yield return new WaitForSeconds(1f);
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
        this.gameObject.SetActive(false);
    }
    IEnumerator Stunned()
    {
        gameObject.GetComponent<EnemyNavigation>().enabled = false;

        if(gameObject.GetComponent<MeleeEnemyAttack>() == null)
        {
            gameObject.GetComponent<RangedEnemyShoot>().enabled = false;
        }
        if(gameObject.GetComponent<RangedEnemyShoot>() == null)
        {
            gameObject.GetComponent<MeleeEnemyAttack>().enabled = false;
        }
        
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<EnemyNavigation>().enabled = true;
        
        if(gameObject.GetComponent<MeleeEnemyAttack>() == null)
        {
            gameObject.GetComponent<RangedEnemyShoot>().enabled = true;
        }
        if(gameObject.GetComponent<RangedEnemyShoot>() == null)
        {
            gameObject.GetComponent<MeleeEnemyAttack>().enabled = true;
        }
    }
}
