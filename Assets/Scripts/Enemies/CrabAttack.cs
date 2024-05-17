using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrabAttack : EnemyAttack
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private EnemyHealth enemyHealth;
    private bool canAttack = true;
    private bool meleeAttackStarted = false;
    private bool attackStarted = false;
    private float attackTimer;
    private float disFromPlayer;
    private bool holdingShell = true;
    private bool digging = false;
    [HideInInspector] public bool zombified = false;
    private float attackCooldown = 1.5f;
    [SerializeField] private float meleeDamage = 50f;
    [SerializeField] private float movementSpeed = 3f;
    private float shellthrowWindup = 1.5f;
    private float knockbackForce = 30f;
    IEnumerator attack;
    Animator animator;
    private Transform player;
    private Transform center;
    private Rigidbody rb;
    private Collider bodyCollider;
    [SerializeField] private GameObject shellProjectile;
    [SerializeField] private GameObject shell;
    [SerializeField] private GameObject meleeHitbox;
    private CrabMeleeHitbox crabMeleeHitbox;
    Quaternion targetRotation;
    public LayerMask enviromentLayer;
    public LayerMask obstacleLayer;
    // Start is called before the first frame update
    void Start()
    {
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        enemyHealth = GetComponent<EnemyHealth>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        animator.SetBool("HasShell", true);
        player = GameObject.FindWithTag("currentPlayer").transform;
        center = transform.Find("CenterPoint");
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<CapsuleCollider>();
        crabMeleeHitbox = meleeHitbox.GetComponent<CrabMeleeHitbox>();
        crabMeleeHitbox.damage = meleeDamage;
        crabMeleeHitbox.knockbackForce = knockbackForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (reworkedEnemyNavigation.playerSeen && canAttack && !digging)
        {
            StartCoroutine(Attack());
        }

        if (meleeAttackStarted)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0f;
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
        if(holdingShell)
        {
            holdingShell = false;
            yield return new WaitForSeconds(shellthrowWindup);
            Destroy(shell);
            GameObject spawnedShell = Instantiate(shellProjectile, transform.position + new Vector3(0f, 3.2f, 2f), Quaternion.Euler(25f, targetRotation.eulerAngles.y, 0f));
            spawnedShell.GetComponent<ShellVelocity>().LaunchShell();
            animator.SetBool("HasShell", false);
            attackStarted = false;
            yield return new WaitForSeconds(attackCooldown/2f);
            canAttack = true;
        }
        else if(!holdingShell)
        {
            meleeAttackStarted = true;
            disFromPlayer = Vector3.Distance(transform.position, player.position);
            while (disFromPlayer > 4f && attackTimer < 3f)
            {
                reworkedEnemyNavigation.playerSeen = true;
                disFromPlayer = Vector3.Distance(transform.position, player.position);
                Vector3 moveDirection = ObstacleAvoidance(player.position - transform.position);
                rb.velocity = new Vector3((moveDirection * movementSpeed).x, rb.velocity.y, (moveDirection * movementSpeed).z);
                yield return null;
            }
            meleeAttackStarted = false;
            attackTimer = 0f;
            disFromPlayer = Vector3.Distance(transform.position, player.position);
            yield return null;
            if (disFromPlayer <= 7f)
            {
                yield return new WaitForSeconds(0.5f);
                if(!zombified)
                {
                    attackStarted = false;
                    animator.SetTrigger("Attack");
                    crabMeleeHitbox.StartCoroutine(crabMeleeHitbox.ActivateHitbox());
                }
                yield return new WaitForSeconds(attackCooldown + 1.7f); //1.7 buffer for the actual animation
                canAttack = true;
            }
            else
            {
                digging = true;
                StartCoroutine(DigAttack());
                yield return null;
            }
        }
    }
    private IEnumerator DigAttack()
    {
        //bodyCollider.enabled = false;
        float timeElapsed = 0f;
        float digDuration = 1.5f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + new Vector3(0f, -10f, 0f);
        while (timeElapsed < digDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / digDuration); // Crab digs underground
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        transform.position = player.position + new Vector3(0f, -10f, 0f); // Moves below where the player is
        //bodyCollider.enabled = true;
        float timeElapsed_02 = 0f;
        float digDuration_02 = 1f;
        Vector3 startPosition_02 = transform.position;
        Vector3 endPosition_02 = transform.position + new Vector3(0f, 12f, 0f);
        while (timeElapsed_02 < digDuration_02)
        {
            transform.position = Vector3.Lerp(startPosition_02, endPosition_02, timeElapsed_02 / digDuration_02); // Tail retracts back down
            timeElapsed_02 += Time.deltaTime;
            yield return null;
        }
        digging = false;
        canAttack = true;
        yield return null;
    }
    public void StopAttack()
    { 
        StopAllCoroutines();
        zombified = true;
        attackStarted = false;
        canAttack = true;
    }

    Vector3 ObstacleAvoidance(Vector3 desiredDirection)
    {
        Vector3 moveDirection = desiredDirection.normalized;

        RaycastHit hit;
        if (Physics.Raycast(center.position, transform.forward, out hit, 3f, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection += avoidanceDirection * 1f;
        }

        return moveDirection.normalized;
    }
}
