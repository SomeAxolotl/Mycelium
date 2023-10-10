using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyShoot : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canShoot = true;
    private float fireRate = 2f;
    public GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
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
            
            if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) && (dstToPlayer - .1f) <= navMeshAgent.stoppingDistance && canShoot)
            {
                StartCoroutine("Shoot");
            }
        }
    }
    IEnumerator Shoot()
    {
        Debug.Log("Shooting");
        canShoot = false;
        Vector3 dirToPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z) - transform.position;
        GameObject tempProj = Instantiate(projectile, transform.position, transform.rotation);
        tempProj.transform.right = dirToPlayer;
        tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer.normalized * 18f;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
