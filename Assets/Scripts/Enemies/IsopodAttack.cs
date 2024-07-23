using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Adaptive;

public class IsopodAttack : EnemyAttack, IDamageBuffable
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private EnemyHealth enemyHealth;
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
    private float damageBuffMultiplier = 1f;
    private EnemyAttributeManager attributeManager; // Reference to the attribute manager

    [SerializeField] bool doesFlee = false;
    float fleeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        enemyHealth = GetComponent<EnemyHealth>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        center = transform.Find("CenterPoint");
        rb = GetComponent<Rigidbody>();
        attributeManager = GetComponentInParent<EnemyAttributeManager>(); // Get the attribute manager

        fleeMultiplier = doesFlee ? -1f : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (reworkedEnemyNavigation.playerSeen && canAttack)
        {
            StartCoroutine(Attack());
        }

        if (isAttacking)
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
            Vector3 dirToPlayer = (player.position - transform.position).normalized * fleeMultiplier;
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
        reworkedEnemyNavigation.enabled = false;
        chargeSpeed = 2.66666f * reworkedEnemyNavigation.moveSpeed;
        attackWindupTime = Random.Range(attackWindupTimeMin, attackWindupTimeMax);
        yield return new WaitForSeconds(attackWindupTime + hitStun);

        if (GlobalData.isAbleToPause)
        {
            if (!doesFlee) SoundEffectManager.Instance.PlaySound("Beetle Charge", transform);
        }
        animator.speed = 2f;
        attackStarted = false;
        isAttacking = true;
        Transform target = player;
        Vector3 playerPos = player.position;
        Vector3 moveDirection = (target.position - transform.position).normalized * fleeMultiplier;
        moveDirection.y = 0f;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Walking"));
        while (distanceToPlayer > 0.25f && !playerDamaged && resetAttack < 1.5f)
        {
            chargeSpeed = 2.66666f * reworkedEnemyNavigation.moveSpeed;
            distanceToPlayer = Vector3.Distance(transform.position, playerPos);
            rb.velocity = new Vector3((moveDirection * chargeSpeed).x, rb.velocity.y, (moveDirection * chargeSpeed).z);
            yield return null;
        }
        animator.speed = 1f;
        if (!doesFlee) animator.SetTrigger("StartAttack");
        yield return new WaitForEndOfFrame();
        if(playerHit.Count == 0)
        {
            animator.SetTrigger("MissAttack");
        }
        isAttacking = false;
        hitStun = 0f;
        reworkedEnemyNavigation.enabled = true;
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
        animator.SetBool("IsMoving", true);
        isAttacking = false;
        hitStun = GameObject.FindWithTag("currentWeapon").GetComponent<WeaponStats>().secondsTilHitstopSpeedup;
        reworkedEnemyNavigation.enabled = true;
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
            animator.SetTrigger("HitAttack");
            playerDamaged = true;
            float dmgDealt = damage * GlobalData.currentLoop * damageBuffMultiplier;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(dmgDealt);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
            if (enemyHealth != null)
            {
                enemyHealth.OnDamageDealt(dmgDealt);
            }
        }
        // Check for Blinding attribute and apply blinding effect
        if (attributeManager != null)
        {
            var blindingAttribute = attributeManager.GetComponent<Blinding>();
            if (blindingAttribute != null)
            {
                Debug.Log("Blinding Att applied!");
                blindingAttribute.ApplyBlindingEffect(5f); // Apply blinding effect for 5 seconds
            }
            else
            {
                Debug.Log("Blinding Att not found!");
            }
        }
    }
    public void ApplyDamageBuff(float multiplier, float duration)
    {
        StartCoroutine(DamageBuffCoroutine(multiplier, duration));
    }

    private IEnumerator DamageBuffCoroutine(float multiplier, float duration)
    {
        damageBuffMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        RemoveDamageBuff();
    }

    public void RemoveDamageBuff()
    {
        damageBuffMultiplier = 1f;
    }
}
