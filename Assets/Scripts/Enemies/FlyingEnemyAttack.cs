using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float explosionDelay = 3f;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private float explosionKnockback = 10f;
    [SerializeField] private float explosionDamage = 40f;
    private float hitStun;
    private float resetAttack;
    private float attackWindupTime;
    [SerializeField] private float attackWindupTimeMin = 0.7f;
    [SerializeField] private float attackWindupTimeMax = 0.9f;
    [SerializeField] private float meleeDamage = 20f;
    private float knockbackForce = 30f;
    private Vector3 previousPosition;
    private Vector3 targetDestination;
    IEnumerator attack;
    Animator animator;
    List<GameObject> objectHit = new List<GameObject>();
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
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (reworkedEnemyNavigation.playerSeen && canAttack)
        {
            StartCoroutine(Attack());
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
        resetAttack = 0f;
        while(Vector3.Distance(center.position, targetDestination) > 0.5f && !playerDamaged && resetAttack < 5f)
        {
            resetAttack += Time.deltaTime;
            Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
            previousPosition = transform.position;
            float speed = currentVelocity.magnitude;
            if (speed > 2f)
            {
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
            Vector3 targetDirection = (targetDestination - center.position).normalized;
            rb.velocity = targetDirection * (reworkedEnemyNavigation.moveSpeed * diveSpeedMultiplier);
            yield return new WaitForFixedUpdate();
        }
        isAttacking = false;
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
        hitStun = 0f;
        playerDamaged = false;
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
        objectHit.Clear();
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
        while(explosionDelay > 0f)
        { 
            explosionDelay -= Time.deltaTime;
        }
        dmgDealt = explosionDamage;
        ParticleManager.Instance.SpawnParticles("SporeBurstPart", center.position, Quaternion.Euler(-90, 0, 0), null, new Vector3(2, 2, 2));
        Collider[] colliders = Physics.OverlapSphere(center.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.GetComponent<EnemyHealth>() != null && !objectHit.Contains(collider.gameObject))
            {
                collider.GetComponent<EnemyHealth>().EnemyTakeDamage(dmgDealt/4f);
                objectHit.Add(collider.gameObject);
            }
            if (collider.GetComponent<EnemyKnockback>() != null)
            {
                collider.GetComponent<EnemyKnockback>().Knockback(explosionKnockback, transform, collider.transform, false);
            }
            if (collider.GetComponentInParent<PlayerHealth>() != null && !objectHit.Contains(collider.gameObject))
            {
                collider.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(dmgDealt);
                objectHit.Add(collider.gameObject);
            }
            if (collider.GetComponentInParent<PlayerController>() != null)
            {
                collider.GetComponentInParent<PlayerController>().Knockback(gameObject, explosionKnockback, false);
            }
        }
        Destroy(gameObject);
    }
    void CooldownRotation(Vector3 direction)
    {
        Quaternion desiredRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * 8f);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !objectHit.Contains(other.gameObject) && isAttacking)
        {
            playerDamaged = true;
            dmgDealt = meleeDamage;
            HitEnemy?.Invoke(other.gameObject, dmgDealt);
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(dmgDealt * GlobalData.currentLoop);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            objectHit.Add(other.gameObject);
        }
    }
}
