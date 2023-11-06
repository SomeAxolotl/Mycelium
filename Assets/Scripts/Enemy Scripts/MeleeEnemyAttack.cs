using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private NewEnemyHealth newEnemyHealth;
    private Collider[] playerColliders;
    private GameObject hitbox;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    [SerializeField] private float attackCooldown = 1.8f;
    private float attackWindup = 1f;
    [SerializeField] float lungeDistance = 0.4f;
    [SerializeField] float lungeDuration = 0.15f;
    IEnumerator attack;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        newEnemyHealth = GetComponent<NewEnemyHealth>();
        hitbox = this.transform.GetChild(1).gameObject;
        attack = this.Attack();
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
            if(newEnemyHealth.damaged)
            {
                StopCoroutine(Attack());
                attack = Attack();
                transform.position = transform.position;
                attackWindup = 1.2f;
                canAttack = true;
            }
            else
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) && (dstToPlayer - .1f) <= navMeshAgent.stoppingDistance && canAttack)
                {
                    attackWindup -= Time.deltaTime;
                    if (attackWindup <= 0)
                    {
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    attackWindup = 1f;
                }
            }
        }
    }
    IEnumerator Attack()
    {
        canAttack = false;

        Vector3 startPosition = transform.position;

        // Lunge forward
        for (float t = 0; t < lungeDuration; t += Time.deltaTime)
        {
            float progress = t / lungeDuration;
            transform.position = Vector3.Lerp(startPosition, startPosition + transform.forward * lungeDistance, progress);
            yield return null;
        }

        hitbox.GetComponent<Collider>().enabled = true;
        hitbox.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(0.2f); //Attack animation will go on this line!

        hitbox.GetComponent<Collider>().enabled = false;
        hitbox.GetComponent<Renderer>().enabled = false;

        for (float t = 0; t < lungeDuration; t += Time.deltaTime)
        {
            float progress = t / lungeDuration;
            transform.position = Vector3.Lerp(startPosition + transform.forward * lungeDistance, startPosition, progress);
            yield return null;
        }

        transform.position = startPosition;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
