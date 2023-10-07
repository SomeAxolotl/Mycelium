using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour
{
    public float waitTime = 4; //Wait time of every action
    public float rotateTime = 2; //Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6; //Walking speed, speed in the nav mesh agent
    public float speedRun = 9; //Running speed
    public float viewRadius = 15; //Radius of the enemy view
    public float viewAngle = 120; //Angle of the enemy view
    public Transform playerLocate;
    public LayerMask playerMask; //To detect the player with the raycast
    public LayerMask obstacleMask; //To detect the obstacules with the raycast
    private NavMeshAgent agent; 
    Vector3 playerLastPosition = Vector3.zero; //Last position of the player when was near the enemy
    Vector3 m_PlayerPosition; //Last position of the player when the player is seen by the enemy
    float waitTimeDelay; //Variable of the wait time that makes the delay
    bool seesPlayer; //If the player is in range of vision, state of chasing
    bool m_PlayerNear; //If the player is near, state of hearing
    bool m_IsPatrol; //If the enemy is patrol, state of patroling
    bool m_CaughtPlayer; //if the enemy has caught the player
    bool canAttack;
    float attackDistance = 4.5f;
    public float attackDamage = 20f;
    public float attackSpeed = 2f;
    Transform meleeCollider;
    float playerDistance;
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        seesPlayer = false;
        m_PlayerNear = false;
        waitTimeDelay = waitTime; //Set the wait time variable that will change
        meleeCollider = this.gameObject.transform.GetChild(1);
        canAttack = true;
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        agent.isStopped = false;
        agent.speed = speedWalk; //Set the navemesh speed with the normal speed of the enemy
        meleeCollider.transform.gameObject.GetComponent<Collider>().enabled = false;
    }
 
    private void Update()
    {
        
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        LookAround(); //Check whether or not the player is in the enemy's field of vision
 
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else 
        {
            gameObject.GetComponent<WanderingAI>();
        }
    }

    //The enemy is chasing the player
    private void Chasing()
    {
        m_PlayerNear = false; //Set false that the player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero; //Reset the player near position
        playerDistance = Vector3.Distance(GameObject.FindGameObjectWithTag("currentPlayer").transform.position, transform.position);

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(m_PlayerPosition); //Set the destination of the enemy to the player location
        }
        if (agent.remainingDistance <= agent.stoppingDistance) //Control if the enemy arrive to the player location
        {
                if (waitTimeDelay <= 0 && !m_CaughtPlayer && playerDistance >= 6f)
            {
                //Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                
                waitTimeDelay = waitTime;
                
            }
            else
            {
                if (playerDistance >= 2.5f)
                    //Wait if the current position is not the player position
                    Stop();
                waitTimeDelay -= Time.deltaTime;
            }

        if (playerDistance <= attackDistance && canAttack && seesPlayer)
            {
                Debug.Log("Enemy attacking");
                StartCoroutine("AttackMelee");
            }
        }
    }

   

    
    IEnumerator AttackMelee()
    {
        canAttack = false;
        meleeCollider.transform.gameObject.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(0.25f);
        meleeCollider.transform.gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if(meleeCollider.transform.gameObject.GetComponent<Collider>().enabled == true && other.CompareTag("currentPlayer"))
        {
            Debug.Log("Player Hit!");
            GameObject.FindWithTag("currentPlayer").GetComponent<PlayerHealth>().currentHealth -= attackDamage;
        }
    }

    void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }
 
    void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }
 
    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }
 
    void LookingPlayer(Vector3 player)
    {
        agent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (waitTimeDelay <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                
                waitTimeDelay = waitTime;
                
            }
            else
            {
                Stop();
                waitTimeDelay -= Time.deltaTime;
            }
        }
    }
 
    void LookAround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        Transform player = playerLocate.transform;
        foreach (var playerLocate in hitColliders) { 
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position); //Distance between the enemy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    seesPlayer = true; //The player has been seeing by the enemy and then the enemy starts chasing the player
                    m_IsPatrol = false; //Change the state to chasing the player
                }
                else
                {
                    //If the player is behind a obstacle the player position will not be registered
                    seesPlayer = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                
                //If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                //Or the enemy is a safe zone, the enemy will no chase
                seesPlayer = false; //Change the sate of chasing
            }
            if (seesPlayer)
            {
                //If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                m_PlayerPosition = player.transform.position; //Save the player's current position if the player is in range of vision
            }
        }
    }
}
