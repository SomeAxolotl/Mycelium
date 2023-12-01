using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NewEnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;
    float deathTimer;
    Rigidbody rb;
    List<BaseEnemyHealthBar> enemyHealthBars = new List<BaseEnemyHealthBar>();
    Transform player;
    EnemyNavigation enemyNavigation;
    NavMeshAgent navMeshAgent;
    CamTracker camTracker;
    Collider thisCollider;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        camTracker = GameObject.FindWithTag("Camtracker").GetComponent<CamTracker>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        enemyNavigation = GetComponent<EnemyNavigation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        thisCollider = GetComponent<Collider>();
        rb.isKinematic = true;
        this.transform.parent = null;

        foreach (BaseEnemyHealthBar enemyHealthBar in GetComponentsInChildren<BaseEnemyHealthBar>())
        {
            enemyHealthBars.Add(enemyHealthBar);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
    }
    void Death()
    {
        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            enemyHealthBar.DefeatEnemy();
        }

        if(gameObject.GetComponent<MeleeEnemyAttack>() != null)
        {
            gameObject.GetComponent<MeleeEnemyAttack>().enabled = false;
        }
        else if (gameObject.GetComponent<RangedEnemyShoot>() != null)
        {
            gameObject.GetComponent<RangedEnemyShoot>().enabled = false;
        }
        deathTimer += Time.deltaTime;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        thisCollider.enabled = false;
        enemyNavigation.enabled = false;
        navMeshAgent.enabled = false;
        rb.Sleep();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        if(deathTimer >= 2f)
        {
            GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
            ParticleManager.Instance.SpawnParticleFlurry("NutrientParticles", nutrientDrop / 20, 0.1f, this.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
            camTracker.ToggleLockOn();
            this.gameObject.SetActive(false);
        }
    }
    public void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;

        foreach (BaseEnemyHealthBar enemyHealthBar in enemyHealthBars)
        {
            enemyHealthBar.UpdateEnemyHealth(currentHealth, maxHealth);
            enemyHealthBar.DamageNumber(dmgTaken);
        }
        //Particle effect for blood
        ParticleManager.Instance.SpawnParticles("Blood", transform.position, Quaternion.identity);
    }
}
