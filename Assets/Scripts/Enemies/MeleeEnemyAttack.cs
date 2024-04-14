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
    [SerializeField] private float attackCooldown = 2f;
    private float attackWindupTime;
    [SerializeField] private float attackWindupTimeMin = 0.7f;
    [SerializeField] private float attackWindupTimeMax = 0.9f;
    private float hitStun;
    private float resetAttack;
    [SerializeField] private float damage = 20f;
    private float knockbackForce = 30f;
    [HideInInspector] public float chargeSpeed;
    IEnumerator attack;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();
    private Transform player;
    private Transform center;
    private Rigidbody rb;
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
        if (Physics.Raycast(center.position, -transform.up, out groundHit, 3f, enviromentLayer))
        {
            Quaternion groundRotation = Quaternion.FromToRotation(transform.up, groundHit.normal) * transform.rotation;
            float groundXRotation = groundRotation.eulerAngles.x;
            float groundZRotation = groundRotation.eulerAngles.z;
            if(attackStarted)
            {
                targetRotation = Quaternion.Euler(groundXRotation, targetRotation.eulerAngles.y, groundZRotation);
            }
            else
            {
                targetRotation = Quaternion.Euler(groundXRotation, transform.eulerAngles.y, groundZRotation);
            }
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
    }
    public override IEnumerator Attack()
    {
        canAttack = false;
        attackStarted = true;
        animator.speed = 0.5f;
        reworkedEnemyNavigation.moveSpeed = 0f;
        chargeSpeed = 8f;
        attackWindupTime = Random.Range(attackWindupTimeMin, attackWindupTimeMax);
        yield return new WaitForSeconds(attackWindupTime + hitStun);
        
        if (GlobalData.isAbleToPause)
        {
            SoundEffectManager.Instance.PlaySound("Beetle Charge", transform.position);
        }
        
        animator.speed = 3f;
        attackStarted = false;
        isAttacking = true;
        Transform target = player;
        Vector3 playerPos = player.position;
        Vector3 moveDirection = (target.position - transform.position).normalized;
        moveDirection.y = 0f;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        while (distanceToPlayer > 0.25f && !playerDamaged && resetAttack < 1.5f)
        {
            distanceToPlayer = Vector3.Distance(transform.position, playerPos);
            rb.velocity = new Vector3((moveDirection * chargeSpeed).x, rb.velocity.y, (moveDirection * chargeSpeed).z);
            yield return null;
        }
        animator.speed = 1f;
        animator.SetTrigger("Attack");
        isAttacking = false;
        hitStun = 0f;
        reworkedEnemyNavigation.moveSpeed = 3f;
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
        reworkedEnemyNavigation.moveSpeed = 3f;
        chargeSpeed = 8f;
        playerDamaged = false;
        playerHit.Clear();
        attackStarted = false;
        canAttack = true;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject) && isAttacking)
        {
            playerDamaged = true;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
