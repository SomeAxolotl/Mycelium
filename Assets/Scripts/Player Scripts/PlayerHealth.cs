using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    float finalRegen;
    bool fetchedStats = false;
    bool canRegen;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        canRegen = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fetchedStats == false)
        {
            StartCoroutine("FetchStats");
        }
        
        if(canRegen == true)
        {
            StartCoroutine("Regen");
        }
        
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        if(currentHealth <= 0 && fetchedStats)
        {
            
            currentHealth = 0;
            Debug.Log("You Died");
            gameObject.GetComponent<PlayerController>().enabled = false;
            rb.Sleep();
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        }
    }
    IEnumerator FetchStats()
    {
        yield return new WaitForEndOfFrame();
        
        //This gets the players health and regen values from the StatTracker script one time when the scene is first loaded.
        maxHealth = gameObject.GetComponent<StatTracker>().maxHealth;
        finalRegen = gameObject.GetComponent<StatTracker>().finalRegen;
        currentHealth = maxHealth;
        
        fetchedStats = true;
    }

    IEnumerator Regen()
    {
        canRegen = false;
        currentHealth += finalRegen;
        yield return new WaitForSeconds(1f);
        canRegen = true;
    }
}
