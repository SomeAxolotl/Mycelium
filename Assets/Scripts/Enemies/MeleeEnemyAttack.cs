using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : EnemyAttack
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private bool playerDamaged = false;
    private bool onGround = true;
    [SerializeField] private float attackCooldown = 2f;
    private float attackWindupTime;
    [SerializeField] private float attackWindupTimeMin = 0.7f;
    [SerializeField] private float attackWindupTimeMax = 0.9f;
    private float hitStun;
    private float resetAttack;
    [SerializeField] private float damage = 20f;
    private float knockbackForce = 30f;
    public float chargeSpeed = 8f;
    private float storedMoveSpeed;
    private float storedChargeSpeed;
    IEnumerator attack;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();
    private Transform player;
    private Transform center;
    private Rigidbody rb;
    Quaternion targetRotation;
    public LayerMask enviromentLayer;
    private GameObject edgeChecker;
    [SerializeField] private EnemyHealth enemyHealth;

    // Start is called before the first frame update
    void Start()
    {
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        center = transform.Find("CenterPoint");
        rb = GetComponent<Rigidbody>();
        storedMoveSpeed = reworkedEnemyNavigation.moveSpeed;
        storedChargeSpeed = chargeSpeed;
        if (enemyHealth == null)
        {
            enemyHealth = GetComponent<EnemyHealth>();
            if (enemyHealth == null)
            {
                Debug.LogError("EnemyHealth component not found on the enemy.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (reworkedEnemyNavigation.playerSeen && canAttack)
        {  
            StartCoroutine(Attack());
        }

        if(isAttacking)
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
            if (attackStarted)
            {
                targetRotation = Quaternion.Euler(groundXRotation, targetRotation.eulerAngles.y, groundZRotation);
            }
            else
            {
                targetRotation = Quaternion.Euler(groundXRotation, transform.eulerAngles.y, groundZRotation);
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
        animator.speed = 0.5f;
        reworkedEnemyNavigation.moveSpeed = 0f;
        attackWindupTime = UnityEngine.Random.Range(attackWindupTimeMin, attackWindupTimeMax);
        yield return new WaitForSeconds(attackWindupTime + hitStun);
        
        if (GlobalData.isAbleToPause)
        {
            BeetleAnimationEvents beetleAnimationEvents = GetComponent<BeetleAnimationEvents>();
            SoundEffectManager.Instance.PlaySound("Beetle Charge", transform, beetleAnimationEvents.GetVolumeModifier(), beetleAnimationEvents.GetPitchMultiplier());
        }
        
        animator.speed = 3f;
        attackStarted = false;
        isAttacking = true;
        edgeChecker = new GameObject("EdgeChecker");
        edgeChecker.transform.localPosition = center.transform.position + transform.forward * 2f + new Vector3(0f, 0.75f, 0f);
        edgeChecker.transform.parent = transform;
        Transform target = player;
        Vector3 playerPos = player.position;
        Vector3 moveDirection = (target.position - transform.position).normalized;
        moveDirection.y = 0f;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        CheckGround();
        if (!onGround)
        {
            CancelAttack();
        }
        while (distanceToPlayer > 0.25f && !playerDamaged && resetAttack < 1.5f && onGround)
        {
            distanceToPlayer = Vector3.Distance(transform.position, playerPos);
            rb.velocity = new Vector3((moveDirection * chargeSpeed).x, rb.velocity.y, (moveDirection * chargeSpeed).z);
            CheckGround();
            yield return new WaitForFixedUpdate();
        }
        animator.speed = 1f;
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (playerHit.Count  > 0 || distanceToPlayer < 5f)
        {
            animator.SetTrigger("Attack");
        }
        isAttacking = false;
        if(edgeChecker != null)
        {
            Destroy(edgeChecker);
        }
        hitStun = 0f;
        reworkedEnemyNavigation.moveSpeed = storedMoveSpeed;
        playerDamaged = false;
        playerHit.Clear();
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public override void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        animator.speed = 1f;
        isAttacking = false;
        hitStun = GameObject.FindWithTag("currentWeapon").GetComponent<WeaponStats>().secondsTilHitstopSpeedup;
        reworkedEnemyNavigation.moveSpeed = storedMoveSpeed;
        chargeSpeed = storedChargeSpeed;
        playerDamaged = false;
        playerHit.Clear();
        attackStarted = false;
        canAttack = true;
        if (edgeChecker != null)
        {
            Destroy(edgeChecker);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject) && isAttacking)
        {
            playerDamaged = true;
            float dmgDealt = damage * GlobalData.currentLoop;
            HitEnemy?.Invoke(other.gameObject, damage);
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage * GlobalData.currentLoop);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
            if (enemyHealth != null)
            {
                enemyHealth.OnDamageDealt(dmgDealt);
            }
            else
            {
                Debug.LogError("EnemyHealth component is not assigned on " + gameObject.name);
            }
        }
    }

    bool CheckGround()
    {
        Vector3 origin = edgeChecker.transform.position;
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity))
        {
            if(!hit.collider.gameObject.name.Contains("Death"))
            {
                onGround = true;
            }
            else
            {
                rb.velocity = Vector3.zero;
                onGround = false;
            }
        }
        else
        {
            onGround = true;
        }

        return onGround;
    }
}
