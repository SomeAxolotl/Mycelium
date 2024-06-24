using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    private bool digAttack = false;
    [HideInInspector] public bool zombified = false;
    private float attackCooldown = 1.5f;
    [SerializeField] private float meleeDamage = 50f;
    [SerializeField] private float movementSpeed = 3f;
    private float shellthrowWindup = 1.5f;
    private float knockbackForce = 30f;
    IEnumerator attack;
    private Animator animator;
    private Transform player;
    private Transform center;
    private Rigidbody rb;
    private Collider bodyCollider;
    private CrabMeleeHitbox crabMeleeHitbox;
    [SerializeField] private GameObject shellProjectile;
    [SerializeField] private GameObject shell;
    [SerializeField] private GameObject meleeHitbox;
    [SerializeField] private GameObject isopod;
    private List<GameObject> playerHit = new List<GameObject>();
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
            if (GameObject.Find("KingCrab") != null)
            {
                GameObject spawnedIsopod = Instantiate(isopod, transform.position + new Vector3(0f, 3.2f, 2f), Quaternion.Euler(25f, targetRotation.eulerAngles.y, 0f));
                spawnedIsopod.GetComponent<ShellVelocity>().LaunchShell();
            }
            else
            {
                GameObject spawnedShell = Instantiate(shellProjectile, transform.position + new Vector3(0f, 3.2f, 2f), Quaternion.Euler(25f, targetRotation.eulerAngles.y, 0f));

                spawnedShell.GetComponent<ShellVelocity>().LaunchShell();
            }
            animator.SetBool("HasShell", false);
            attackStarted = false;
            yield return new WaitForSeconds(attackCooldown/2f);
            canAttack = true;
        }
        else if(!holdingShell)
        {
            meleeAttackStarted = true;
            while (attackTimer <= 3f)
            {
                reworkedEnemyNavigation.playerSeen = true;
                Vector3 moveDirection = ObstacleAvoidance(player.position - transform.position);
                float currMovement = movementSpeed;
                if(GetComponent<SpeedChange>() != null){
                    currMovement *= ((GetComponent<SpeedChange>().speedChangePercent + 100) / 100);
                }
                rb.velocity = new Vector3((moveDirection * currMovement).x, rb.velocity.y, (moveDirection * currMovement).z);
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
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .6f); // Waits until animation is in correct position to activate hitbox
                    crabMeleeHitbox.StartCoroutine(crabMeleeHitbox.ActivateHitbox());
                }
                yield return new WaitForSeconds(attackCooldown);
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
        animator.SetTrigger("Burrow");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Burrow")); 
        yield return new WaitForSeconds(1.25f); // Lets player actually see burrow anim
        float timeElapsed = 0f;
        float digDuration = 2.5f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + new Vector3(0f, -10f, 0f);
        while (timeElapsed < digDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / digDuration); // Crab digs underground
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        transform.position = player.position + new Vector3(0f, -10f, 0f); // Moves below where the player is
        digAttack = true;
        ParticleManager.Instance.SpawnParticles("Dust", player.position + new Vector3(0f, 0.25f, 0f), Quaternion.Euler(-90, 0, 0), null, new Vector3(.3f, .3f, .3f)); // Warns the player
        float timeElapsed_02 = 0f;
        float digDuration_02 = 3f;
        Vector3 startPosition_02 = transform.position;
        Vector3 endPosition_02 = transform.position + new Vector3(0f, 9.5f, 0f);
        bool jumpStarted = false;
        while (timeElapsed_02 < digDuration_02)
        {
            transform.position = Vector3.Lerp(startPosition_02, endPosition_02, timeElapsed_02 / digDuration_02); // Crab pops up from underground
            timeElapsed_02 += Time.deltaTime;
            if(Vector3.Distance(center.position, endPosition_02) < 6f && !jumpStarted)
            {
                animator.SetTrigger("Jump"); // Premtively starts jump anim
                jumpStarted = true;
            }
            yield return null;
        }
        rb.AddForce(Vector3.up * 80f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.75f);
        digAttack = false;
        digging = false;
        canAttack = true;
        reworkedEnemyNavigation.playerSeen = true;
        playerHit.Clear();
        yield return null;
    }
    public void StopAttack()
    { 
        StopAllCoroutines();
        zombified = true;
        attackStarted = false;
        meleeAttackStarted = false;
        canAttack = true;
        digAttack = false;
        digging = false;
        playerHit.Clear();
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
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && digAttack && !playerHit.Contains(other.gameObject) && !enemyHealth.alreadyDead)
        {
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(meleeDamage * GlobalData.currentLoop);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
