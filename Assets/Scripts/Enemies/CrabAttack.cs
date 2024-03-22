using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabAttack : EnemyAttack
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private bool playerDamaged = false;
    [SerializeField] private bool holdingShell = true;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float damage = 20f;
    private float shellthrowWindup = 1.5f;
    private float knockbackForce = 30f;
    IEnumerator attack;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();
    private Transform player;
    private Transform center;
    private Rigidbody rb;
    [SerializeField] private GameObject shellProjectile;
    [SerializeField] private GameObject shell;
    Quaternion targetRotation;
    public LayerMask enviromentLayer;
    // Start is called before the first frame update
    void Start()
    {
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        animator.SetBool("HasShell", true);
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
        if (Physics.Raycast(center.position, -transform.up, out groundHit, 5f, enviromentLayer))
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
        }
        else
        {
            Debug.Log("melee attack!");
        }
        playerHit.Clear();
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public override void CancelAttack()
    {
        StopAllCoroutines();
        if(shell != null)
        {
            Destroy(shell);
        }
        holdingShell = false;
        attack = Attack();
        isAttacking = false;
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
