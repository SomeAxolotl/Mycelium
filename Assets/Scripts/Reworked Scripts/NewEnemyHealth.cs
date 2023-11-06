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
    float groundedTimer;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
        //Debug.Log(currentHealth);
        Debug.DrawRay(transform.position, -transform.up * 1.05f, Color.red, 2f);
        while(groundedTimer > 0f) 
        {
            groundedTimer += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 1.05f))
            {
                groundedTimer = 0;
                damaged = false;
                navMeshAgent.updatePosition = true;
            }
        }
        Debug.Log("grounded timer: " + groundedTimer);
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
        GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
        if(deathTimer >= 2f)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void EnemyTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
        navMeshAgent.updatePosition = false;
        damaged = true;
        groundedTimer += Time.deltaTime;
        Vector3 dirFromPlayer = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(player.position.x, 0f, player.position.z)).normalized;
        StartCoroutine(Knockback(dirFromPlayer, 5f));
        enemyHealthBar.UpdateEnemyHealth();
        enemyHealthBar.DamageNumber(dmgTaken);

        //Particle effect for blood
        ParticleManager.Instance.SpawnParticles("Blood", transform.position, Quaternion.identity);
    }
    IEnumerator Knockback(Vector3 direction, float force)
    {
        yield return new WaitUntil(() => !navMeshAgent.updatePosition);
        Vector3 knockbackForce = direction * force;
        knockbackForce += Vector3.up * 3f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
    }
}
