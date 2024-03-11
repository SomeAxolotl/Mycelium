using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombifiedMovement : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float followRange = 10f;
    [SerializeField] private float explosionTimer = 3f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionDamage = 15f;
    private Collider[] enemyColliders;
    List<GameObject> enemiesHit = new List<GameObject>();
    private Transform center;
    private Transform player;
    private Rigidbody rb;
    private float moveSpeed = 2f;
    private float gravityForce = -10;
    Vector3 gravity;
    private Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.layer = 0;
        center = transform.Find("CenterPoint");
        player = GameObject.FindWithTag("currentPlayer").transform;
        rb = GetComponent<Rigidbody>();
        gravity = new Vector3(0f, gravityForce, 0f);
        StartCoroutine(ExplosionCountdown());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);

        if (GetClosestEnemy() != null)
        {
            moveDirection = ObstacleAvoidance(GetClosestEnemy().transform.position - transform.position);
            rb.velocity = new Vector3((moveDirection * moveSpeed).x, rb.velocity.y, (moveDirection * moveSpeed).z);
        }
        else
        {
            moveDirection = ObstacleAvoidance(transform.position - player.position);
            rb.velocity = new Vector3((moveDirection * moveSpeed).x, rb.velocity.y, (moveDirection * moveSpeed).z);
        }

        Quaternion desiredRotation = Quaternion.LookRotation(moveDirection);
        float desiredYRotation = desiredRotation.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0f, desiredYRotation, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);

    }
    GameObject GetClosestEnemy()
    {
        enemyColliders = Physics.OverlapSphere(transform.position, followRange, enemyLayer);
        GameObject closestEnemy = null;
        float closestDistance = followRange;
        foreach (Collider collider in enemyColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = collider.gameObject;
                closestDistance = distance;
            }
        }
        return closestEnemy;
    }
    Vector3 ObstacleAvoidance(Vector3 desiredDirection)
    {
        Vector3 movingDirection = desiredDirection.normalized;
        RaycastHit hit;
        if (Physics.Raycast(center.position, transform.forward, out hit, 3f, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            movingDirection += avoidanceDirection * 2f;
        }
        return movingDirection.normalized;
    }
    IEnumerator ExplosionCountdown()
    {
        yield return new WaitForSeconds(explosionTimer);
        enemyColliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider collider in enemyColliders)
        {
            if(collider.GetComponent<EnemyHealth>() != null && !enemiesHit.Contains(collider.gameObject))
            {
                enemiesHit.Add(collider.gameObject);
                collider.GetComponent<EnemyHealth>().EnemyTakeDamage(explosionDamage);
            }
        }
        gameObject.GetComponent<EnemyHealth>().EnemyTakeDamage(100f);
    }
}
