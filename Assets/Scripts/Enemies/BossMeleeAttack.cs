using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossMeleeAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject hitbox;
    private Collider[] playerColliders;
    private Transform player;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private bool canAttack = true;
    private bool windupStarted = false;
    private float attackWindup = .8f;
    [SerializeField] float lungeDistance = 0.4f;
    [SerializeField] float lungeDuration = 0.15f;
    [SerializeField] private float attackCooldown = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        hitbox = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(transform.position, 20f, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            if (Vector3.Angle(transform.forward, dirToPlayer) < 30f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) && (dstToPlayer - 0.2f) <= navMeshAgent.stoppingDistance && canAttack && !windupStarted)
            {
                StartCoroutine(AttackWindup());
            }
        }
    }
    IEnumerator AttackWindup()
    {
        windupStarted = true;
        hitbox.GetComponent<Collider>().enabled = false;
        hitbox.GetComponent<Renderer>().enabled = false;
        navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(attackWindup);
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        canAttack = false;
        windupStarted = false;
        navMeshAgent.speed = 4f;
        attackWindup = .8f;
        Vector3 startPosition = transform.position;

        //Lunge Forwards
        for (float t = 0; t < lungeDuration; t += Time.deltaTime)
        {
            float progress = t / lungeDuration;
            transform.position = Vector3.Lerp(startPosition, startPosition + transform.forward * lungeDistance, progress);
            yield return null;
        }

        hitbox.GetComponent<Collider>().enabled = true;
        hitbox.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(0.2f); //Attack animation will go here!

        hitbox.GetComponent<Collider>().enabled = false;
        hitbox.GetComponent<Renderer>().enabled = false;

        //Lunge Backwards
        for (float t = 0; t < lungeDuration; t += Time.deltaTime)
        {
            float progress = t / lungeDuration;
            transform.position = Vector3.Lerp(startPosition + transform.forward * lungeDistance, startPosition, progress);
            yield return null;
        }
        transform.position = startPosition;
        yield return new WaitForSeconds(attackCooldown);
        windupStarted = false;
        canAttack = true;
    }
}
