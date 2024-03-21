using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MonsterBossAttack : MonoBehaviour
{
    private TempMovement bossMovement;
    [SerializeField] public float attractionForce = 100.0f;
    [SerializeField] public float attractionRadius = 20.0f;
    [SerializeField] public float pullInterval = 5.0f;
    private float elapsedTime = 0.0f;
    public Transform pullOrigin;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public bool canAttack = true;
    public bool isAttacking = false;
    public bool playerDamaged = false;
    private bool isTailAttacking = false;
    private bool isSlamAttacking = false;
    private bool isSwipeAttacking = false;
    private bool tailAttackOnCooldown = false;
    private bool isRandomOnCooldown = false;
    [SerializeField] private float attackInterval = 2f;
    private float attackWindupTime = .5f;
    private float resetAttack;
    [SerializeField] private float damage = 20f;
    private float knockbackForce = 30f;
    IEnumerator attack;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();
    private Transform player;
    private Rigidbody rb;
    [SerializeField] private GameObject bossTail;
    PlayerController playerController;
    //[SerializeField] private float tailAttackAnimationDuration = 1.5f;
    [SerializeField] private float swipeAttackAnimationDuration = 1.0f;
    [SerializeField] private float slamAttackAnimationDuration = 2.0f;
    [SerializeField] private float tailAttackDamage = 30f;
    [SerializeField] private float swipeAttackDamage = 25f;
    [SerializeField] private float slamAttackDamage = 35f;
    [SerializeField] private float tailKnockback = 70f;
    private bool collidedWithPlayer = false;


    // Start is called before the first frame update
    void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        bossMovement = GetComponent<TempMovement>();
        attack = this.Attack(damage);
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float dstToPlayer = Vector3.Distance(transform.position, player.position);
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= pullInterval)
        {
            PullPlayers();
            elapsedTime = 0.0f; // Reset the timer
        }
        if (!bossMovement.playerSeen)
        {
            CancelAttack();
        }
        else if (bossMovement.playerSeen && canAttack && dstToPlayer <= 12f)
        {
            isTailAttacking = false;
            if (!isRandomOnCooldown)
            {
                StartCoroutine(TriggerRandomAttacksWithDelay());
            }
        }
        else if (dstToPlayer > 12f && bossMovement.playerSeen && canAttack)
        {

            if (!isTailAttacking && !tailAttackOnCooldown)
            {
                isTailAttacking = true;
                StartCoroutine(TailAttack());
            }
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
    private IEnumerator Attack(float damage)
    {
        canAttack = false;
        isAttacking = true;

        // Windup time before attacking
        yield return new WaitForSeconds(attackWindupTime);

        // Perform attack animation or action
        animator.SetTrigger("Attack");

        // Check if player is within range and not invincible
        if (!playerDamaged && Vector3.Distance(transform.position, player.position) <= 12f &&
            !player.gameObject.GetComponentInParent<PlayerController>().isInvincible && collidedWithPlayer)
        {
            // Inflict damage and any other effects
            player.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            playerDamaged = true;
            // Add knockback or any other effect here if needed
        }


        playerHit.Clear();

        yield return new WaitForSeconds(attackInterval);

        canAttack = true;
        isAttacking = false;
    }

    public void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack(damage);
        isAttacking = false;
        playerDamaged = false;
        playerHit.Clear();
        canAttack = true;
        resetAttack = 0f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && other.gameObject.GetComponentInParent<PlayerController>().isInvincible == false && !playerHit.Contains(other.gameObject) && isAttacking)
        {
            collidedWithPlayer = true;
        
        if (isTailAttacking && !isSlamAttacking || !isSwipeAttacking)
            {
                damage = tailAttackDamage;
                knockbackForce = 300f;
            }
            if (isSlamAttacking && !isSwipeAttacking)
            {
                damage = slamAttackDamage;
                knockbackForce = 100f;
            }
            if (isSwipeAttacking && !isSlamAttacking)
            {
                damage = swipeAttackDamage;
                knockbackForce = 200f;
            }
            playerDamaged = false;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer")
        {
            collidedWithPlayer = false;
        }
    }
    private void PullPlayers()
    {

        playerColliders = Physics.OverlapSphere(pullOrigin.position, attractionRadius, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            if (playerCollider.CompareTag("currentPlayer"))
            {
                StartCoroutine(ApplyAttractionForceOverTime(playerCollider.transform));
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
    private IEnumerator ApplyAttractionForceOverTime(Transform playerTransform)
    {
        float elapsedTime = 0.0f;
        Vector3 initialPosition = playerTransform.position;

        while (elapsedTime < 5.0f) // Change 5.0f to the desired duration
        {
            Vector3 direction = pullOrigin.position - playerTransform.position;
            float distance = direction.magnitude;

            if (distance <= attractionRadius)
            {
                float strength = (1.0f - distance / attractionRadius) * attractionForce;
                playerTransform.GetComponentInParent<Rigidbody>().AddForce(direction.normalized * strength);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator TailAttack()
    {
        tailAttackOnCooldown = true;
        //animator.SetTrigger("TailAttack");
        Debug.Log("TAIL ATTACK!");
        float customDuration = 3.0f; // Replace with your custom duration
        Debug.Log("Waiting for " + customDuration + " seconds...");
        yield return new WaitForSeconds(customDuration);

        Debug.Log("Tail Attack Animation Finished!");
        Vector3 playerPosition = player.position;
        Vector3 spawnPosition = playerPosition + transform.forward * 1f;
        GameObject bossTailInstance = Instantiate(bossTail, spawnPosition, Quaternion.identity, transform);
        StartCoroutine(Attack(tailAttackDamage));
        knockbackForce = tailKnockback;
        ApplyKnockbackToPlayer();

        yield return new WaitForSeconds(5f);
        Debug.Log("Tail Attack off cooldown!");
        Destroy(bossTailInstance);
        tailAttackOnCooldown = false;
        isTailAttacking = false;
    }

    private IEnumerator SwipeAttack()
    {
        isSwipeAttacking = true;
        //animator.SetTrigger("SwipeAttack");
        Debug.Log("SWIPE ATTACK!");
        yield return new WaitForSeconds(swipeAttackAnimationDuration);
        StartCoroutine(Attack(swipeAttackDamage));

        ApplyKnockbackToPlayer();
        isSwipeAttacking = false;
    }

    private IEnumerator SlamAttack()
    {
        isSlamAttacking = true;
        //animator.SetTrigger("SlamAttack");
        Debug.Log("SLAM ATTACK!");
        yield return new WaitForSeconds(slamAttackAnimationDuration);
        StartCoroutine(Attack(slamAttackDamage));

        ApplyKnockbackToPlayer();
        isSlamAttacking = false;
    }

    private void ApplyKnockbackToPlayer()
    {
        // Apply knockback effect to player here
        // playerController.Knockback(this.gameObject, knockbackForce);
    }

    private void TriggerRandomAttack()
    {
        int randomAttack = Random.Range(0, 2);

        switch (randomAttack)
        {
            case 0:
                StartCoroutine(SwipeAttack());
                break;
            case 1:
                StartCoroutine(SlamAttack());
                break;
            default:
                Debug.LogError("Invalid random attack number.");
                break;
        }
    }
    private IEnumerator TriggerRandomAttacksWithDelay()
    {
        while (true)
        {
            if (!isRandomOnCooldown)
            {
                TriggerRandomAttack();
                isRandomOnCooldown = true;
                float customDelayDuration = 5.0f; // Replace with your custom duration
                yield return new WaitForSeconds(customDelayDuration);
                isRandomOnCooldown = false;
            }
            else
            {
                yield return null;
            }
        }
    }
}


