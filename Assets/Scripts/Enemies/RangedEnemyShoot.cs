using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyShoot : EnemyAttack
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private Transform player;
    private Transform center;
    public Transform launchPoint;
    private bool canAttack = true;
    private bool attackStarted = false;
    [SerializeField]private float attackCooldown = 2f;
    [SerializeField]private float attackWindupTime = 2f;
    public GameObject projectile;
    private List<GameObject> activeProjectiles = new List<GameObject>(); // List of active projectiles
    IEnumerator attack;
    Animator animator;
    Quaternion targetRotation;
    public LayerMask enviromentLayer;

    // Start is called before the first frame update
    void Start()
    {
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform.Find("CenterPoint");
        center = transform.Find("CenterPoint");
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
        yield return new WaitForSeconds(attackWindupTime);
        attackStarted = false;
        animator.SetTrigger("Attack");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .6f);
        Vector3 dirToPlayer = (player.position - launchPoint.position).normalized;
        GameObject tempProj = Instantiate(projectile, launchPoint.position, Quaternion.identity);
        tempProj.transform.right = dirToPlayer;
        tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer * 15f;

        // Set the instantiator enemy health reference
        RangedEnemyProjectile rangedEnemyProjectile = tempProj.GetComponent<RangedEnemyProjectile>();
        if (rangedEnemyProjectile != null)
        {
            rangedEnemyProjectile.SetInstantiatorEnemyHealth(GetComponent<EnemyHealth>());
        }

        // Add the projectile to the list of active projectiles
        activeProjectiles.Add(tempProj);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public override void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        canAttack = true;
    }
    public void RemoveProjectile(GameObject projectile)
    {
        activeProjectiles.Remove(projectile);
    }
}
