using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public int nutrientDrop;

    public bool damaged;
    private float deathTimer;
    private float flightTimer;
    private Rigidbody rb;
    private EnemyHealthBar enemyHealthBar;
    private Transform player;

    private MeleeEnemyAttack meleeEnemyAttack;
    private RangedEnemyShoot rangedEnemyShoot;
    BossBehavior bossBehavior;
    NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        damaged = false;
        rb = GetComponentInChildren<Rigidbody>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        rb.isKinematic = true;
        this.transform.parent = null;


        // Find child objects and their components
        /*meleeEnemyAttack = GetComponentInChildren<MeleeEnemyAttack>();
        rangedEnemyShoot = GetComponentInChildren<RangedEnemyShoot>();

        // Check if the components are found
        if (meleeEnemyAttack == null)
        {
            Debug.LogError("MeleeEnemyAttack component not found on child objects.");
        }

        if (rangedEnemyShoot == null)
        {
            Debug.LogError("RangedEnemyShoot component not found on child objects.");
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
        if (damaged)
        {
            flightTimer += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 1.05f) && flightTimer > 0)
            {
                damaged = false;
                flightTimer = 0;
                navMeshAgent.enabled = true;
                bossBehavior.enabled = true;
                rb.isKinematic = true;
            }
        }
    }

    void Death()
    {
        deathTimer += Time.deltaTime;
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        enemyHealthBar.GetComponent<Canvas>().enabled = false; // Disable health bar canvas
        bossBehavior.enabled = false;
        navMeshAgent.enabled = false;
        rb.Sleep();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90f), Time.deltaTime);
        if (deathTimer >= 2f)
        {
            GameObject.Find("NutrientCounter").GetComponent<NutrientTracker>().AddNutrients(nutrientDrop);
            this.gameObject.SetActive(false);
        }
    }

    public void BossTakeDamage(float dmgTaken)
    {
        currentHealth -= dmgTaken;
        damaged = true;
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
        yield return new WaitUntil(() => !bossBehavior.enabled && !navMeshAgent.enabled);
        Vector3 knockbackForce = direction * force;
        knockbackForce += Vector3.up * 2f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
    }
}

