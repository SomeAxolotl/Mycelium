using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MonsterBossAttack : MonoBehaviour
{
    private MonsterBossMovement bossMovement;
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
    PlayerController playerController;
    [SerializeField] private float tailAttackAnimationDuration = 1.5f;
    [SerializeField] private float swipeAttackAnimationDuration = 1.0f;
    [SerializeField] private float slamAttackAnimationDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        bossMovement = GetComponent<MonsterBossMovement>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
        else
        {
            if (bossMovement.playerSeen && canAttack)
            {
                StartCoroutine(Attack());
                StartCoroutine(TriggerRandomAttacksWithDelay());
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
    IEnumerator Attack()
    {
        canAttack = false;
        isAttacking = true;

        // Windup time before attacking
        yield return new WaitForSeconds(attackWindupTime);

        // Perform attack animation or action
        animator.SetTrigger("Attack");

        // Check if player is within range and not invincible
        if (!playerDamaged && Vector3.Distance(transform.position, player.position) <= 12f &&
            !player.gameObject.GetComponentInParent<PlayerController>().isInvincible)
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
        attack = Attack();
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
            playerDamaged = false;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
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
        //animator.SetTrigger("TailAttack");
        Debug.Log("TAIL ATTACK!");
        yield return new WaitForSeconds(tailAttackAnimationDuration);

        ApplyKnockbackToPlayer();
    }

    private IEnumerator SwipeAttack()
    {
        //animator.SetTrigger("SwipeAttack");
        Debug.Log("SWIPE ATTACK!");
        yield return new WaitForSeconds(swipeAttackAnimationDuration);

        ApplyKnockbackToPlayer();
    }

    private IEnumerator SlamAttack()
    {
        //animator.SetTrigger("SlamAttack");
        Debug.Log("SLAM ATTACK!");
        yield return new WaitForSeconds(slamAttackAnimationDuration);

        ApplyKnockbackToPlayer();
    }

    private void ApplyKnockbackToPlayer()
    {
        // Apply knockback effect to player here
        // Example: playerController.Knockback(this.gameObject, knockbackForce);
    }

    private void TriggerRandomAttack()
    {
        int randomAttack = Random.Range(0, 3);

        switch (randomAttack)
        {
            case 0:
                StartCoroutine(TailAttack());
                break;
            case 1:
                StartCoroutine(SwipeAttack());
                break;
            case 2:
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
            TriggerRandomAttack();

            yield return new WaitForSeconds(5.0f);
        }
    }
}

