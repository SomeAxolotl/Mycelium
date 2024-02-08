using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockback : MonoBehaviour
{
    public bool damaged;
    [HideInInspector] public float flightTimer;
    Rigidbody rb;
    Transform player;
    EnemyNavigation enemyNavigation;
    NavMeshAgent navMeshAgent;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        damaged = false;
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        enemyNavigation = GetComponent<EnemyNavigation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (damaged)
        {
            flightTimer += Time.deltaTime;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, .35f) && flightTimer > .3)
            {
                damaged = false;
                flightTimer = 0;
                navMeshAgent.Warp(transform.position);
                navMeshAgent.enabled = true;
                animator.enabled = true;
                enemyNavigation.enabled = true;
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }
    public void Knockback(float knockbackForce)
    {
        damaged = true;
        enemyNavigation.enabled = false;
        navMeshAgent.enabled = false;
        animator.enabled = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        Vector3 dirFromPlayer = (new Vector3(transform.position.x, 0f, transform.position.z) - new Vector3(player.position.x, 0f, player.position.z)).normalized;
        StartCoroutine(StartKnockback(dirFromPlayer, knockbackForce));
    }
    IEnumerator StartKnockback(Vector3 direction, float force)
    {
        yield return new WaitUntil(() => !enemyNavigation.enabled && !navMeshAgent.enabled && !animator.enabled);
        Vector3 knockbackForce = direction * force;
        knockbackForce += Vector3.up * 2f;
        rb.AddForce(knockbackForce, ForceMode.Impulse);
    }
}
