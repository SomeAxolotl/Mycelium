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
    public bool damaged;
    float deathTimer;
    float flightTimer;
    Rigidbody rb;
    EnemyHealthBar enemyHealthBar;
    Transform player;
    EnemyNavigation enemyNavigation;
    NavMeshAgent navMeshAgent;
    Collider thisCollider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        damaged = false;
        rb = GetComponent<Rigidbody>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        enemyNavigation = GetComponent<EnemyNavigation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        thisCollider = GetComponent<Collider>();
        rb.isKinematic = true;
        this.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
        if(damaged)
        {
            flightTimer += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 1.05f) && flightTimer > 0)
            {
                damaged = false;
                flightTimer = 0;
                navMeshAgent.enabled = true;
                enemyNavigation.enabled = true;
                rb.isKinematic = true;
                Debug.Log("works");

            }
        }
    }
    void Death()
    {
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
            this.gameObject.SetActive(false);
        }
    }
    public void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
        damaged = true;
        enemyNavigation.enabled = false;
        navMeshAgent.enabled = false;
        rb.isKinematic = false;
        Vector3 dirFromPlayer = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(player.position.x, 0f, player.position.z)).normalized;
        StartCoroutine(Knockback(dirFromPlayer, 7f));
        enemyHealthBar.UpdateEnemyHealth();
        enemyHealthBar.DamageNumber(dmgTaken);

        //Particle effect for blood
        ParticleManager.Instance.SpawnParticles("Blood", transform.position, Quaternion.identity);

        //Sound effect
        SoundEffectManager.Instance.PlaySound("impact", transform.position);
    }
    IEnumerator Knockback(Vector3 direction, float force)
    {
        yield return new WaitUntil(() => !enemyNavigation.enabled && !navMeshAgent.enabled);
        Vector3 knockbackForce = direction * force;
        knockbackForce += Vector3.up * 2f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
    }
}
