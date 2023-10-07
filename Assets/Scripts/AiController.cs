using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour
{
                   //  Nav mesh agent component
    public float waitTime = 4;                 //  Wait time of every action
    public float rotateTime = 2;                  //  Wait time when the enemy detect near the player without seeing
    public float speedWalk = 6;                     //  Walking speed, speed in the nav mesh agent
    public float speedRun = 9;                      //  Running speed
 
    public float viewRadius = 15;                   //  Radius of the enemy view
    public float viewAngle = 90;
    public Transform playerLocate; //  Angle of the enemy view
    public LayerMask playerMask;                    //  To detect the player with the raycast
    public LayerMask obstacleMask;
    public float wanderRadius;
    public float wanderTimer;//  To detect the obstacules with the raycast
        //  Max distance to calcule the a minumun and a maximum raycast when hits something


    
    public Transform[] points;                   //  All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;
    private NavMeshAgent agent; //  Current waypoint where the enemy is going to

    Vector3 playerLastPosition = Vector3.zero;      //  Last position of the player when was near the enemy
    Vector3 m_PlayerPosition;                       //  Last position of the player when the player is seen by the enemy
 
    float waitTimeDelay;                               //  Variable of the wait time that makes the delay
                              //  Variable of the wait time to rotate when the player is near that makes the delay
    bool seesPlayer;                           //  If the player is in range of vision, state of chasing
    bool m_PlayerNear;                              //  If the player is near, state of hearing
    bool m_IsPatrol;                                //  If the enemy is patrol, state of patroling
    bool m_CaughtPlayer;
    bool canAttack;
    
    //  if the enemy has caught the player
    float attackDistance = 4f;
    public int attackDamage = 5;
    public float attackSpeed = 2f;
    float timer;
     Transform meleeCollider;
 
    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        seesPlayer = false;
        m_PlayerNear = false;
        waitTimeDelay = waitTime;
        //  Set the wait time variable that will change
        meleeCollider = this.gameObject.transform.GetChild(1);
        canAttack = true;


        m_CurrentWaypointIndex = 0;                 //  Set the initial waypoint
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;
        agent.isStopped = false;
        agent.speed = speedWalk;             //  Set the navemesh speed with the normal speed of the enemy
            //  Set the destination to the first waypoint
        meleeCollider.transform.gameObject.GetComponent<Collider>().enabled = false;
    }
 
    private void Update()
    {
        timer += Time.deltaTime;
        
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        LookAround();            //  Check whether or not the player is in the enemy's field of vision
 
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else 
        {
            GetComponent<WanderingAI>();
        }
       
    }
 
    private void Chasing()
    {
        //  The enemy is chasing the player
        m_PlayerNear = false;                       //  Set false that hte player is near beacause the enemy already sees the player
        playerLastPosition = Vector3.zero;          //  Reset the player near position
 
        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            agent.SetDestination(m_PlayerPosition);          //  set the destination of the enemy to the player location
        }
        if (agent.remainingDistance <= agent.stoppingDistance)    //  Control if the enemy arrive to the player location
        {
                if (waitTimeDelay <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                //  Check if the enemy is not near to the player, returns to patrol after the wait time delay
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                
                waitTimeDelay = waitTime;
                
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    //  Wait if the current position is not the player position
                    Stop();
                waitTimeDelay -= Time.deltaTime;
            }
        if (agent.stoppingDistance == attackDistance && canAttack)
            {
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
                float dstToPlayer = Vector3.Distance(transform.position, player.position);          //  Distance of the enmy and the player
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    seesPlayer = true;             //  The player has been seeing by the enemy and then the nemy starts to chasing the player
                    m_IsPatrol = false;                 //  Change the state to chasing the player
                }
                else
                {
                    /*
                     *  If the player is behind a obstacle the player position will not be registered
                     * */
                    seesPlayer = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                /*
                 *  If the player is further than the view radius, then the enemy will no longer keep the player's current position.
                 *  Or the enemy is a safe zone, the enemy will no chase
                 * */
                seesPlayer = false;                //  Change the sate of chasing
            }
            if (seesPlayer)
            {
                /*
                 *  If the enemy no longer sees the player, then the enemy will go to the last position that has been registered
                 * */
                m_PlayerPosition = player.transform.position;       //  Save the player's current position if the player is in range of vision
            }
        }
    }
}
