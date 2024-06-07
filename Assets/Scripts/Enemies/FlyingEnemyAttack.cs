using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemyAttack : EnemyAttack
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private bool playerDamaged = false;
    [SerializeField] private bool isKamikaze;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private float diveSpeedMultiplier = 4f;
    private float hitStun;
    private float resetAttack;
    private float attackWindupTime;
    [SerializeField] private float attackWindupTimeMin = 0.7f;
    [SerializeField] private float attackWindupTimeMax = 0.9f;
    [SerializeField] private float damage = 20f;
    private float knockbackForce = 30f;
    private Vector3 targetDestination;
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

        if (isAttacking)
        {
            resetAttack += Time.deltaTime;
        }
        else
        {
            resetAttack = 0f;
        }
    }
    void FixedUpdate()
    {
        if (attackStarted)
        {
            Vector3 dirToPlayer = (player.position - center.position).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(dirToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 8f);
        }
    }
    public override IEnumerator Attack()
    {
        rb.velocity = Vector3.zero;
        canAttack = false;
        attackStarted = true;
        attackWindupTime = UnityEngine.Random.Range(attackWindupTimeMin, attackWindupTimeMax);
        yield return new WaitForSeconds(attackWindupTime + hitStun);
        attackStarted = false;
        isAttacking = true;
        targetDestination = player.position;
        Vector3 storedPosition = center.position;
        while(Vector3.Distance(center.position, targetDestination) > 0.5f && !playerDamaged && resetAttack < 5f)
        {
            Vector3 targetDirection = (targetDestination - center.position).normalized;
            rb.velocity = targetDirection * (reworkedEnemyNavigation.moveSpeed * diveSpeedMultiplier);
            yield return new WaitForFixedUpdate();
        }
        float elapsedTime = 0f;
        Vector3 initialVelocity = rb.velocity;
        while (elapsedTime < .25f)
        {
            elapsedTime += Time.deltaTime;
            rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, elapsedTime / .25f);
            yield return new WaitForFixedUpdate();
        }
        if (isKamikaze)
        {
            Kamikaze();
            yield break;
        }
        isAttacking = false;
        hitStun = 0f;
        playerDamaged = false;
        playerHit.Clear();
        float timer = 0f;
        while (Vector3.Distance(center.position, storedPosition) > 0.5f && timer <= attackCooldown)
        {
            Vector3 targetDirection = (storedPosition - center.position).normalized;
            rb.velocity = targetDirection * reworkedEnemyNavigation.moveSpeed;
            CooldownRotation(targetDirection);
            reworkedEnemyNavigation.playerSeen = true;
            timer += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        float elapsedTime_2 = 0f;
        Vector3 initialVelocity_2 = rb.velocity;
        while (elapsedTime_2 < .25f)
        {
            elapsedTime_2 += Time.deltaTime;
            rb.velocity = Vector3.Lerp(initialVelocity_2, Vector3.zero, elapsedTime_2 / .25f);
            yield return new WaitForFixedUpdate();
        }
        reworkedEnemyNavigation.playerSeen = false;
        canAttack = true;
    }
    void Kamikaze()
    {
        StopAllCoroutines();
    }
    void CooldownRotation(Vector3 direction)
    {
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 8f);
    }
}
