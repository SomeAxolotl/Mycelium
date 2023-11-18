using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyShoot : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private BossHealth bossHealth;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canShoot = true;
    [SerializeField] private float fireRate = 1.5f;
    private float attackWindup = 0.8f;
    public GameObject projectile;
    IEnumerator shoot;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        bossHealth = GetComponentInParent<BossHealth>();
        shoot = this.Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(transform.position, 25f, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            if (bossHealth.damaged)
            {
                StopCoroutine(Shoot());
                shoot = Shoot();
                transform.position = transform.position;
                attackWindup = 1.2f;
                canShoot = true;
            }
            else
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) && (dstToPlayer - .1f) <= navMeshAgent.stoppingDistance && canShoot)
                {
                    attackWindup -= Time.deltaTime;
                    if (attackWindup <= 0)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    attackWindup = 0.8f;
                }
            }
        }
    }
    IEnumerator Shoot()
    {
        canShoot = false;
        Vector3 dirToPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z) - transform.position;
        GameObject tempProj = Instantiate(projectile, transform.position, transform.rotation);
        tempProj.transform.right = dirToPlayer;
        tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer.normalized * 18f;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
