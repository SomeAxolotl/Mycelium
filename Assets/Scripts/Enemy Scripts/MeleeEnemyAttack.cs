using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    private float attackCooldown = 2f;
    
    //private float damage = 25f;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
            
            if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) && (dstToPlayer - .1f) <= navMeshAgent.stoppingDistance && canAttack)
            {
                StartCoroutine("Attack");
            }
        }
    }
    IEnumerator Attack()
    {
        canAttack = false;
        //Melee enemy currently has a child gameobject that has a collider and acts as a hitbox
        this.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(.2f); //Attack animation goes here
        this.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    
    /*private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "currentPlayer")
        {
            Debug.Log("Player Hit!");
            GameObject.FindWithTag("currentPlayer").GetComponent<PlayerHealth>().currentHealth -= damage;
        }
    }*/
}
