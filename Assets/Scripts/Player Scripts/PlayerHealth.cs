using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    float finalRegen;
    bool fetchedStats = false;
    bool canRegen;
    Rigidbody rb;
    bool respawned = true;
    private HUDHealth hudHealth;

    // Start is called before the first frame update
    void Start()
    {
        canRegen = true;
        rb = GetComponent<Rigidbody>();

        hudHealth = GameObject.Find("HUD").GetComponent<HUDHealth>();
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
        
        if(currentHealth <= 0 && fetchedStats && respawned)
        {
            currentHealth = 0f;
            StartCoroutine("Death");
        }
    }
    IEnumerator Death()
    {
        respawned = false;
        gameObject.GetComponent<PlayerController>().enabled = false;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("HubWorldPlaceholder");
        gameObject.GetComponent<PlayerController>().enabled = true;
        transform.position = new Vector3(0, 1.5f, 1.0f);
        currentHealth = maxHealth;
        transform.rotation = Quaternion.identity;
        respawned = true;
        
    }

    public void UpdateStats()
    {
        StartCoroutine("FetchStats");
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
        Heal(finalRegen);
        hudHealth.UpdateHealthUI();
        yield return new WaitForSeconds(1f);
        canRegen = true;
    }

    public void Hurt(float damage)
    {
        currentHealth -= damage;
        hudHealth.UpdateHealthUI();
    }

    public void Heal(float healing)
    {
        currentHealth += healing;
        hudHealth.UpdateHealthUI();
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }
}
