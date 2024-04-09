using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MonsterBossAttack : MonoBehaviour
{
    [SerializeField] public float attractionForce = 200f;
    [SerializeField] public float attractionRadius = 20.0f;
    [SerializeField] public float pullInterval = 5.0f;
    public float pullDistance = 2f;
    public float pullHeightOffset = 1f;
    private float elapsedTime = 0.0f;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public bool canAttack = true;
    public bool isAttacking = false;
    public bool playerDamaged = false;
    public bool isTailAttacking = false;
    private bool isSlamAttacking = false;
    private bool isSwipeAttacking = false;
    public bool tailAttackOnCooldown = false;
    private bool isRandomOnCooldown = false;
    private float attackInterval = 2f;
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
    private float swipeAttackAnimationDuration = 1.0f;
    private float slamAttackAnimationDuration = 2.0f;
    [SerializeField] private float tailAttackDamage = 30f;
    [SerializeField] private float swipeAttackDamage = 25f;
    [SerializeField] private float slamAttackDamage = 35f;
    [SerializeField] private float tailKnockback = 50f;
    private bool collidedWithPlayer = false;
    [SerializeField] private float distanceInFront = 2.0f;
    [SerializeField] private float customDelayDuration;

    //BossProcedualAnimation bossProcedualAnimation;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        attack = this.Attack(damage);
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        //bossProcedualAnimation = GetComponent<BossProcedualAnimation>();          <--REMOVED FOR OTHER ANIMATIONS
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float dstToPlayer = Vector3.Distance(transform.position, player.position);
        elapsedTime += Time.deltaTime;
        Debug.Log("attacking? " + isAttacking);
        if (elapsedTime >= pullInterval)
        {
            PullPlayers();
            
            elapsedTime = 0.0f; // Reset the timer
        }
        
        
        if (!tailAttackOnCooldown && this.gameObject != null && dstToPlayer > 12f)
        {
            StartCoroutine(TailAttack());
        }
        
        
        if (!isRandomOnCooldown && this.gameObject != null && dstToPlayer <= 12f)
        {
           StartCoroutine(TriggerRandomAttacksWithDelay());
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

        // Windup time before attacking

        // Perform attack animation or action
        //animator.SetTrigger("Attack");

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

    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && !other.gameObject.GetComponentInParent<PlayerController>().isInvincible && !playerHit.Contains(other.gameObject) && isAttacking)
        {
            Debug.Log("collided with player");
            collidedWithPlayer = true;
        
        if (isTailAttacking && !isSlamAttacking || !isSwipeAttacking)
            {
                damage = tailAttackDamage;
                knockbackForce = 150f;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("ArmLayer") && isSlamAttacking && !isSwipeAttacking)
            {
                damage = slamAttackDamage;
                knockbackForce = 50f;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("ArmLayer") && isSwipeAttacking && !isSlamAttacking)
            {
                damage = swipeAttackDamage;
                knockbackForce = 100f;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("BodyLayer"))
            {
                collidedWithPlayer = false;
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
        ParticleManager.Instance.SpawnParticles("BossSandPullParticles", gameObject.transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(-90, 0, 0));
        float disToPlayer = Vector3.Distance(transform.position, player.position);
        if(disToPlayer <= attractionRadius)
        {
            StartCoroutine(ApplyAttractionForceOverTime());
            Debug.Log("PULLED");
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
    private IEnumerator ApplyAttractionForceOverTime()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 5.0f) // Change 5.0f to the desired duration
        {
            Vector3 direction = new Vector3(transform.position.x, player.position.y, transform.position.z) - player.position;
            float distance = direction.magnitude;

            float strength = (1.0f - distance / attractionRadius) * attractionForce;
            Vector3 pullForce = (direction.normalized * strength);
            player.GetComponent<Rigidbody>().AddForce(pullForce, ForceMode.Force);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator TailAttack()
    {
        tailAttackOnCooldown = true;
        Vector3 spawnPosition = player.position + new Vector3(0f, -4.5f, 0f);
        Quaternion tailRotation = Quaternion.Euler(-21.7f, 0f, 0f);
        GameObject bossTailInstance = Instantiate(bossTail, spawnPosition, tailRotation, null);
        bossTailInstance.GetComponentInChildren<TailCollision>().damage = tailAttackDamage;
        ParticleManager.Instance.SpawnParticles("TrophicCascadePoof", spawnPosition + new Vector3(0f, 2.75f, 0f), Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds(1.75f);
        float timeElapsed = 0f;
        float duration = 0.5f;
        Vector3 startPosition = bossTailInstance.transform.position;
        Vector3 endPosition = bossTailInstance.transform.position + new Vector3(0f, 4f, 0f);
        while (timeElapsed < duration)
        {
            bossTailInstance.transform.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        float timeElapsed_02 = 0f;
        float duration_02 = 1f;
        while (timeElapsed_02 < duration_02)
        {
            bossTailInstance.transform.position = Vector3.Lerp(endPosition, startPosition + new Vector3(0f, -3f, 0f), timeElapsed_02 / duration_02);
            timeElapsed_02 += Time.deltaTime;
            yield return null;
        }
        Destroy(bossTailInstance);
        yield return new WaitForSeconds(5f);
        tailAttackOnCooldown = false;
        isTailAttacking = false;   
    }
    private IEnumerator SwipeAttack()
    {
        isSwipeAttacking = true;
        isAttacking = true;
        int randomSwipeAttack = Random.Range(0, 2);

        switch (randomSwipeAttack)
        {
            case 0:
                animator.SetTrigger("AttackLeft");
                break;
            case 1:
                animator.SetTrigger("AttackRight");
                break;
            default:
                Debug.LogError("Invalid random attack number.");
                break;
        }

        //animator.SetTrigger("SwipeAttack");
        Debug.Log("SWIPE ATTACK!");
        yield return new WaitForSeconds(swipeAttackAnimationDuration);
        StartCoroutine(Attack(swipeAttackDamage));
        ApplyKnockbackToPlayer();
        isSwipeAttacking = false;
        isAttacking = false;
    }

    private IEnumerator SlamAttack()
    {
        isSlamAttacking = true;
        isAttacking = true;
        //StartCoroutine(bossProcedualAnimation.SmashAttack());
        animator.SetTrigger("Smash");
        // Debug.Log("SLAM ATTACK!");
        yield return new WaitForSeconds(slamAttackAnimationDuration);
        StartCoroutine(Attack(slamAttackDamage));

        ApplyKnockbackToPlayer();
        isSlamAttacking = false;
        isAttacking = false;
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
            if (!isRandomOnCooldown)
            {
                TriggerRandomAttack();
                isRandomOnCooldown = true;
                yield return new WaitForSeconds(customDelayDuration);
                isRandomOnCooldown = false;
            }
            else
            {
                yield return null;
            }
        
    }
}


