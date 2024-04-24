using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MushyAttack : EnemyAttack
{
    private bool canAttack = true;
    private bool attackStarted = false;
    private float attackTimer;
    private float disFromPlayer;
    private float hitStun;
    private float resetAttack;
    private float moveSpeed = 4f;
    [HideInInspector] public bool zombified = false;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] public float damage = 20f;
    [HideInInspector] public float knockbackForce = 30f;
    IEnumerator attack;
    private Animator animator;
    private Transform player;
    private Transform center;
    private Rigidbody rb;
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    [SerializeField] private GameObject mushyWeapon;
    private MushyWeaponCollision mushyWeaponCollision;
    Quaternion targetRotation;
    public LayerMask enviromentLayer;

    // Start is called before the first frame update
    void Start()
    {
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        center = transform.Find("CenterPoint");
        rb = GetComponent<Rigidbody>();
        mushyWeaponCollision = mushyWeapon.GetComponent<MushyWeaponCollision>();
        mushyWeaponCollision.damage = damage;
        mushyWeaponCollision.knockbackForce = knockbackForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (reworkedEnemyNavigation.playerSeen && canAttack)
        {  
            StartCoroutine(Attack());
        }

        if(attackStarted)
        {
            resetAttack += Time.deltaTime;
        }
        else
        {
            resetAttack = 0f;
        }
    }
    private void FixedUpdate()
    {
        if (attackStarted)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(dirToPlayer);
            float desiredYRotation = desiredRotation.eulerAngles.y;
            targetRotation = Quaternion.Euler(0f, desiredYRotation, 0f);
        }

        RaycastHit groundHit;
        if (Physics.Raycast(center.position, -transform.up, out groundHit, 2f, enviromentLayer))
        {
            Quaternion groundRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
            float groundXRotation = groundRotation.eulerAngles.x;
            float groundZRotation = groundRotation.eulerAngles.z;
            if (!canAttack)
            {
                //targetRotation = Quaternion.Euler(groundXRotation, targetRotation.eulerAngles.y, groundZRotation);
                targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            }
            else
            {
                //targetRotation = Quaternion.Euler(groundXRotation, transform.eulerAngles.y, groundZRotation);
                targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            }
        }
        else
        {
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
    }
    public override IEnumerator Attack()
    {
        canAttack = false;
        attackStarted = true;
        disFromPlayer = Vector3.Distance(transform.position, player.position);
        while (disFromPlayer > 2f && attackTimer < 3f)
        {
            reworkedEnemyNavigation.playerSeen = true;
            disFromPlayer = Vector3.Distance(transform.position, player.position);
            Vector3 moveDirection = ObstacleAvoidance(player.position - transform.position);
            rb.velocity = new Vector3((moveDirection * moveSpeed).x, rb.velocity.y, (moveDirection * moveSpeed).z);
            yield return null;
        }
        attackTimer = 0f;
        disFromPlayer = Vector3.Distance(transform.position, player.position);
        yield return null;
        if (disFromPlayer <= 3f)
        {
            yield return new WaitForSeconds(0.3f);
            if (!zombified)
            {
                attackStarted = false;
                animator.SetTrigger("Attack");
                mushyWeaponCollision.StartCoroutine(mushyWeaponCollision.ActivateHitbox());
            }
            yield return new WaitForSeconds(attackCooldown + 1f); //1 sec buffer for the actual animation
        }
        else
        {
            yield return null;
        }
        canAttack = true;
    }
    public override void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        mushyWeapon.GetComponent<Collider>().enabled = false;
        mushyWeaponCollision.playerHit.Clear();
        hitStun = GameObject.FindWithTag("currentWeapon").GetComponent<WeaponStats>().secondsTilHitstopSpeedup;
        attackStarted = false;
        canAttack = true;
    }
    public void StopAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        mushyWeapon.GetComponent<Collider>().enabled = false;
        mushyWeaponCollision.playerHit.Clear();
        zombified = true;
        attackStarted = false;
        canAttack = true;
    }
    Vector3 ObstacleAvoidance(Vector3 desiredDirection)
    {
        Vector3 moveDirection = desiredDirection.normalized;

        RaycastHit hit;
        if (Physics.Raycast(center.position, transform.forward, out hit, 3f, enviromentLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection += avoidanceDirection * 1f;
        }

        return moveDirection.normalized;
    }
}
